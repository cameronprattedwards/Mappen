using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Mappen.Domain.Entities.Locations;
using Mappen.Domain.Entities.Times;
using Mappen.Domain.Meetup.Models;
using Cibus.Infrastructure.Utils;

namespace Mappen.Domain.Entities.Events
{
	public class Event
	{
		public int Id { get; set; }
		public string MeetupId { get; set; }
		public EventInfo Info { get; set; }
		public Venue Venue { get; set; }
		public Timespan Timespan { get; set; }
		public double Distance { get; set; }

		public Event() { }

		public Event(MeetupEvent meetupEvent)
		{
			MeetupId = meetupEvent.Id;
			MeetupVenue venue = meetupEvent.Venue;

			EventInfo info = new EventInfo();
			info.Name = meetupEvent.Group.Name + ": " + meetupEvent.Name;
			info.Description = meetupEvent.Description;
			info.Website = meetupEvent.Event_Url;
			info.PhotoUrl = meetupEvent.Photo_Url ?? 
				(meetupEvent.Group != null && meetupEvent.Group.Group_Photo != null ? 
					meetupEvent.Group.Group_Photo.HighRes_Link : 
					string.Empty);
			if (venue != null && venue.Phone != null)
			{
				string purged = Regex.Replace(meetupEvent.Venue.Phone, @"[^\d]", "");
				if (!string.IsNullOrEmpty(purged))
					try
					{
						info.Phone = Convert.ToInt64(purged);
					}
					catch (OverflowException e)
					{ }
			}

			Geocode geocode = new Geocode(venue.Lat, venue.Lon);

			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			long startTicks = epoch.Ticks + (meetupEvent.Time * TimeSpan.TicksPerMillisecond),
				endTicks = startTicks +
						(meetupEvent.Duration != 0 ?
							(meetupEvent.Duration * TimeSpan.TicksPerMillisecond) :
							(2 * TimeSpan.TicksPerHour)
						);

			DateTime start = new DateTime(startTicks),
				end = new DateTime(endTicks);
			Timespan span = new Timespan(start, end);

			Address address = new Address();

			address.Street1 = venue.Address_1;
			address.Street2 = venue.Address_2;
			address.City = venue.City;
			address.State = venue.State;
			address.Zip = venue.Zip != null ? Convert.ToInt64(venue.Zip.Split('-')[0]) : 0;
			address.Geocode = geocode;

			Venue myVenue = new Venue();
			myVenue.Name = venue.Name;
			myVenue.Address = address;

			this.Venue = myVenue;
			this.Timespan = span;
			this.Info = info;
		}

		public Event Sync(Event newEvent)
		{
			this.Venue.Sync(newEvent.Venue);
			this.Info.Sync(newEvent.Info);
			this.Timespan.Sync(newEvent.Timespan);
			return this;
		}
	}
}
