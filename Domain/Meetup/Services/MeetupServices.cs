using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Mappen.Domain.Meetup.Models;
using Mappen.Data.Events;
using Mappen.Domain.Entities.Events;

namespace Mappen.Domain.Meetup.Services
{
	public class MeetupServices
	{
		private static Dictionary<string, EventCategory> categories { get; set; }

		private static Dictionary<string, EventCategory> Categories { get {
			if (categories == null)
			{
				using (EventUnitOfWork uow = new EventUnitOfWork())
				{
					categories = uow.EventCategories.All.ToDictionary(e => e.Name);
				}
			}
			return categories;
		} }
		private static EventCategory GetCategory(string category)
		{
			switch (category.ToLower())
			{
				case "business":
				case "dancing":
				case "education":
				case "fashion":
				case "fitness":
				case "games":
				case "literature":
				case "music":
				case "spirituality":
				case "recreation":
					return Categories[new CultureInfo("en-us", false).TextInfo.ToTitleCase(category)];

				case "fashion-beauty":
					return Categories["Fashion"];
				case "career-business":
					return Categories["Business"];
				case "literature-writing":
					return Categories["Literature"];
				case "education-learning":
					return Categories["Education"];
				case "arts":
				case "arts-culture":
					return Categories["Being Human"];
				case "auto":
				case "cars-motorcycles":
					return Categories["Auto and Tech"];
				case "well-being":
				case "health-wellbeing":
					return Categories["Fitness"];
				case "food & drink":
				case "food-drink":
					return Categories["Gastronomy"];
				case "movements":
				case "government-politics":
					return Categories["Politics"];
				case "crafts":
				case "hobbies-crafts":
					return Categories["Hobbies"];
				case "languages":
				case "language":
					return Categories["Ethnicity"];
				case "lgbt":
					return Categories["Gender"];
				case "lifestyle":
					return Categories["Being Human"];
				case "films":
				case "movies-film":
					return Categories["Film"];
				case "outdoors":
				case "outdoors-adventure":
					return Categories["Getting Out"];
				case "religion-beliefs":
				case "paranormal":
				case "new-age-spirituality":
					return Categories["Spirituality"];
				case "moms & dads":
				case "parents-family":
					return Categories["Family"];
				case "pets":
				case "pets-animals":
					return Categories["Animals"];
				case "photography":
					return Categories["Art"];
				case "beliefs":
					return Categories["Spirituality"];
				case "sci fi":
				case "sci-fi-fantasy":
				case "social":
				case "community":
				case "community-environment":
				case "socializing":
					return Categories["Tribes"];
				case "support":
					return Categories["Solidarity"];
				case "tech":
					return Categories["Auto and Tech"];
				case "women":
					return Categories["Gender"];
				case "singles":
					return Categories["Relationships"];
				case "sports-recreation":
					return Categories["Recreation"];
			}
			return null;
		}

		public static void Load(int page, int zip)
		{
			List<MeetupEvent> events = MeetupApi.GetEvents(page, zip);

			using (EventUnitOfWork uow = new EventUnitOfWork())
			{
				categories = uow.EventCategories.All.ToDictionary(e => e.Name);
				foreach (MeetupEvent currEvent in events)
				{
					Event myEvent = new Event(currEvent);
					Event duplicate = uow.Events.WhereIncluding(
						e => e.MeetupId == myEvent.MeetupId,
						e => e.Venue,
						e => e.Venue.Address,
						e => e.Venue.Address.Geocode,
						e => e.Timespan,
						e => e.Info
					).FirstOrDefault();
					EventCategory category = GetCategory(currEvent.Group.Category.ShortName);
					myEvent.Info.Category = category;
					if (duplicate != null)
					{
						duplicate.Sync(myEvent);
					}
					else
					{
						uow.Events.Add(myEvent);
					}
					Console.WriteLine(currEvent.Name);
				}
				Console.WriteLine("Persisting " + zip.ToString());
				uow.Save();
			}
		
		}
	}
}
