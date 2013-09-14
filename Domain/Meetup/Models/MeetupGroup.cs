using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Meetup.Models
{
	public class MeetupGroup
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public GroupPhoto Group_Photo { get; set; }
		public MeetupCategory Category { get; set; }
	}
}
