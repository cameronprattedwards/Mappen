using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.EventsDomain.Models;
using Mappen.Domain.Entities.Locations;
using Mappen.Domain.Entities.Events;
using Mappen.Data.Events;
using Mappen.Domain.Search;
using Cibus.Infrastructure.IocBox;

namespace Mappen.Domain.EventsDomain.Services
{
	class EventsServices
	{
		public static List<EventVm> Circle(Geocode geocode)
		{
			using (EventUnitOfWork uow = new EventUnitOfWork())
			{
				List<Event> events = uow.Events.OrderByDistance(geocode).ToList();
				return events
					.Take(100)
					.Select(e => new EventVm(e))
					.ToList();
			}
		}

		public static List<EventVm> Search(string query, Geocode geocode)
		{
			ISearchApi api = Box.Get<ISearchApi>();
			List<SearchEvent> searchEvents = api.SearchEvents(query);
			if (searchEvents == null)
				return new List<EventVm>();
			IEnumerable<int> ids = searchEvents.Select(e => e.Id);
			using (EventUnitOfWork uow = new EventUnitOfWork())
			{
				return uow
					.Events
					.WhereDeep(e => ids.Contains(e.Info.Id))
					.OrderBy(e => 				
						Math.Pow(
							Math.Pow((double)(Math.Abs(e.Venue.Address.Geocode.Lat - geocode.Lat)), 2) +
							Math.Pow((double)(Math.Abs(e.Venue.Address.Geocode.Lng - geocode.Lng)), 2) +
							Math.Pow((double)(((e.Timespan.Start.Ticks - DateTime.Now.Ticks) / TimeSpan.TicksPerHour)/2), 2),
						.5)
					)
					.Select(e => new EventVm(e))
					.ToList();
			}
		}
	}
}
