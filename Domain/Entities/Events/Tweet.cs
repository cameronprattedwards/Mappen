using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Entities.Locations;

namespace Mappen.Domain.Entities.Events
{
	public class Tweet
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public Geocode Geocode { get; set; }
	}
}
