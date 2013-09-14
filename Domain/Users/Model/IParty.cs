using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Entities.Events;
using Mappen.Domain.Entities.Locations;

namespace Users.Model
{
	public class IParty
	{
		public DateTime JoinDate;
		public string Name;
		public string Email;
		public string Bio;
		public List<Event> EventsAttending;
		public List<Event> OwnedEvents;
		public List<Venue> OwnedVenues;
		public List<Tweet> OwnedTweets;
		public string Password;
	}
}
