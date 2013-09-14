using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Entities.Events;
using Mappen.Domain.Entities.Locations;
using Mappen.Domain.Entities.Times;

namespace Mappen.Domain.EventsDomain.Models
{
	public class EventVm
	{
		public int Id { get; set; }
		public Venue Venue { get; set; }
		public Timespan Timespan { get; set; }
		public EventInfo Info { get; set; }

		public EventVm(Event eventModel)
		{
			Id = eventModel.Id;
			Venue = eventModel.Venue;
			Info = eventModel.Info;
			Timespan = eventModel.Timespan;
		}
	}
}