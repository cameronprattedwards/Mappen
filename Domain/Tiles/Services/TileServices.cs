using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiles.Models;
using Mappen.Domain.Entities.Locations;
using Mappen.Domain.Entities.Times;
using Mappen.Domain.Entities.Events;
using Mappen.Data.Events;
using Mappen.Domain.EventsDomain.Models;

namespace Tiles.Services
{
	public class TileServices
	{
		private static Bounds MakeBounds(Geocode geocode)
		{
			Geocode sw = new Geocode(Math.Floor(geocode.Lat), Math.Floor(geocode.Lng));
			Geocode ne = new Geocode(Math.Ceiling(geocode.Lat), Math.Ceiling(geocode.Lng));
			return new Bounds(ne, sw);
		}

		private static Timespan MakeSpan(DateTime date)
		{
			long oneDay = TimeSpan.TicksPerDay,
				startTicks = (long) Math.Floor((decimal)(date.Ticks / oneDay)) * oneDay;
			DateTime start = new DateTime(startTicks),
				end = new DateTime(startTicks + TimeSpan.TicksPerDay - 1);

			return new Timespan(start, end);
		}

		private static Tile MakeTile(Corner corner, EventUnitOfWork uow)
		{
			Bounds bounds = MakeBounds(corner.Geocode);
			Timespan span = MakeSpan(corner.Date);
			Tile tile = new Tile(bounds, span);
			tile = FillTileWithEvents(tile, uow);
			return tile;
		}

		private static Tile FillTileWithEvents(Tile tile, EventUnitOfWork uow)
		{
			Geocode ne = tile.Bounds.NorthEast,
				sw = tile.Bounds.SouthWest;
			Timespan span = tile.Timespan;
			tile.Events = uow.Events.WhereIncluding(e =>
				e.Venue.Address.Geocode.Lat < ne.Lat &&
				e.Venue.Address.Geocode.Lat >= sw.Lat &&
				e.Venue.Address.Geocode.Lng <= ne.Lng &&
				e.Venue.Address.Geocode.Lng >= sw.Lng &&
				e.Timespan.Start <= span.End &&
				e.Timespan.End >= span.Start,

				e => e.Venue,
				e => e.Venue.Address,
				e => e.Venue.Address.Geocode,
				e => e.Timespan,
				e => e.Info,
				e => e.Info.Category

			).Select(e => new EventVm(e)).ToList();
			return tile;
		}

		public static List<Tile> GetTiles(List<Corner> corners)
		{
			List<Tile> output = new List<Tile>();
			using (EventUnitOfWork uow = new EventUnitOfWork())
			{
				foreach (Corner corner in corners)
				{
					Tile tile = MakeTile(corner, uow);
					output.Add(tile);
				}
				uow.Save();
			}
			return output;
		}
	}
}
