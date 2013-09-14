using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Mappen.Domain.Entities.Events
{
	public class EventCategory
	{
		public int Id { get; set; }
		public EventCategory Parent { get; set; }
		public string Name { get; set; }

		public EventCategory() { }

		public EventCategory(string name)
		{
			Name = name;
		}

		public EventCategory(string name, EventCategory parent)
		{
			Name = name;
			Parent = parent;
		}
	}
}
