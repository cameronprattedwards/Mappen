using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Entities.Events;
using Mappen.Domain.Entities.Times;
using Mappen.Data.Events;
using Mappen.Domain.Entities.Locations;
using Mappen.Data.IocContainersData;
using Cibus.Infrastructure.IocBox;
using Ninject;

namespace EntityTester
{
	class Program
	{
		static void Main(string[] args)
		{
			Box.Module = new StandardKernel(new RealDataModule());
			CreateEvent();
		}

		private static void CreateEvent()
		{
			using (EventUnitOfWork uow = new EventUnitOfWork())
			{
				Event myEvent = new Event();
				EventInfo info = new EventInfo();
				info.Description = "New York's hottest club is 'Kevin.'";
				info.Email = "stefon@weekendupdate.com";
				info.Name = "What to do in New York this weekend.";
				info.Phone = 8019896481;
				info.Website = "snl.com";

				Timespan span = new Timespan(DateTime.Now, new DateTime(DateTime.Now.Ticks + (2 * TimeSpan.TicksPerHour)));

				Venue venue = new Venue();
				Address address = new Address();
				address.Name = "NBC Studios";
				address.Street1 = "123 Fake St";
				address.City = "Anytown";
				address.State = "NY";
				address.Zip = 10029;
				address.Geocode = new Geocode(-110, 40);

				venue.Address = address;
				venue.Email = "myemail@nbc.com";
				venue.SubaddressNumber = 23;
				venue.SubaddressType = SubaddressType.Apartment;

				myEvent.Venue = venue;
				myEvent.Timespan = span;
				myEvent.Info = info;

				uow.Events.Add(myEvent);
				uow.Save();
			}
		}
	}
}
