using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Entities.Locations;
using Mappen.Domain.Entities.Times;
using Mappen.Domain.Entities.Events;
using Mappen.Domain.EventsDomain.Models;

namespace Tiles.Models
{
	public class Tile
	{
		public List<EventVm> Events { get; set; }
		public Bounds Bounds { get; set; }
		public Timespan Timespan { get; set; }

		public bool Contains(Geocode geocode)
		{ 
			return Bounds.Contains(geocode);
		}
		public bool Contains(DateTime time)
		{
			return Timespan.Contains(time);
		}

		public Tile(Bounds bounds, Timespan span)
		{
			Bounds = bounds;
			Timespan = span;
		}
	}
}
