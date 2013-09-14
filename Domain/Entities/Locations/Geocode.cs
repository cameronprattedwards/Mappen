using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Mappen.Domain.Entities.Locations
{
	public class Geocode
	{
		public int Id { get; set; }
		
		public decimal Lat { get; set; }
		public decimal Lng { get; set; }
		public Geocode() { }
		public Geocode(decimal lat, decimal lng)
		{
			Lat = lat;
			Lng = lng;
		}
	}
}
