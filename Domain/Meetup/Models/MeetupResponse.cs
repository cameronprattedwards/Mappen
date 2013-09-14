using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Meetup.Models
{
	class MeetupEventResponse
	{
		public List<MeetupEvent> Results { get; set; }
	}

	class MeetupGroupResponse
	{
		public List<MeetupGroup> Results { get; set; }
	}
}
