using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Entities.Locations
{
	public class Bounds
	{
		public int Id;
		public Geocode NorthEast { get; set; }
		public Geocode SouthWest { get; set; }

		public Bounds() { }

		public Bounds(Geocode northEast, Geocode southWest)
		{
			NorthEast = northEast;
			SouthWest = southWest;
		}

		public bool Contains(Geocode geocode)
		{
			return SouthWest.Lat <= geocode.Lat && 
				geocode.Lat <= NorthEast.Lat &&
				SouthWest.Lng <= geocode.Lng && 
				geocode.Lng <= NorthEast.Lng;
		}
		public bool Contains(Bounds bounds)
		{
			return Contains(bounds.NorthEast) && Contains(bounds.SouthWest);
		}
	}
}
