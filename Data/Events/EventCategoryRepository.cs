using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Data.BaseData;
using Mappen.Domain.Entities.Events;

namespace Mappen.Data.Events
{
	public class EventCategoryRepository : Repository<EventCategory>
	{
		public EventCategoryRepository(IDbContext context) : base(context) { }
	}
}
