using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Meetup.Models
{
	public class MeetupEvent
	{
		public string Id { get; set; }
		public long Created { get; set; }
		public string Description { get; set; }
		public double Distance { get; set; }
		public long Duration { get; set; }
		public string Event_Url { get; set; }
		public string Name { get; set; }
		public long Time { get; set; }
		public string Photo_Url { get; set; }
		public MeetupVenue Venue { get; set; }
		public MeetupGroup Group { get; set; }
	}
}
