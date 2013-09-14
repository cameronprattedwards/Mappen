using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Objects;
using Mappen.Data.BaseData;
using Mappen.Domain.Entities.Events;
using Mappen.Domain.Entities.Locations;
using System.Linq.Expressions;

namespace Mappen.Data.Events
{
	public class EventRepository : Repository<Event>
	{
		public EventRepository(IDbContext context)
			: base(context)
		{
			
		}

		public IEnumerable<Event> WhereDeep(Expression<System.Func<Event, bool>> expr)
		{
			return WhereIncluding(expr,
				e => e.Venue,
				e => e.Venue.Address,
				e => e.Venue.Address.Geocode,
				e => e.Info,
				e => e.Timespan,
				e => e.Info.Category
			);
		}

		public IEnumerable<Event> OrderByDistance(Geocode geocode)
		{
			return DbSet
				.Where(e => e.Timespan.End > DateTime.Now)
				.OrderBy(e =>
					Math.Pow(
						Math.Pow((double)(Math.Abs(e.Venue.Address.Geocode.Lat - geocode.Lat)), 2) +
						Math.Pow((double)(Math.Abs(e.Venue.Address.Geocode.Lng - geocode.Lng)), 2) +
						Math.Pow((double)(EntityFunctions.DiffHours(e.Timespan.Start, DateTime.Now) / 10), 2),
					.5)
				)
				.Take(100)
				.Include(e => e.Venue).Include(e => e.Venue.Address).Include(e => e.Venue.Address.Geocode).Include(e => e.Info)
				.Include(e => e.Info.Category).Include(e => e.Timespan);
		}
	}
}