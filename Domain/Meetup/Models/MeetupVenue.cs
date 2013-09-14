using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Meetup.Models
{
	public class MeetupVenue
	{
		public string Address_1 { get; set; }
		public string Address_2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public decimal Lat { get; set; }
		public decimal Lon { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
	}
}
