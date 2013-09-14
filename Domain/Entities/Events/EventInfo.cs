using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mappen.Domain.Entities.Events
{

	[Table("EventInfos")]
	public class EventInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Parking { get; set; }
		public string Website { get; set; }
		public string PhotoUrl { get; set; }
		public string Email { get; set; }
		public long Phone { get; set; }
		public EventCategory Category { get; set; }
		public List<string> Tags { get; set; }

		public EventInfo Sync(EventInfo newInfo)
		{
			Name = newInfo.Name;
			Description = newInfo.Description;
			Parking = newInfo.Parking;
			Website = newInfo.Website;
			Email = newInfo.Email;
			Phone = newInfo.Phone;
			Category = newInfo.Category;
			Tags = newInfo.Tags;

			return this;
		}
	}
}