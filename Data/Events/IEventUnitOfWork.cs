using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Data.BaseData;
using Cibus.Infrastructure.IocBox;

namespace Mappen.Data.Events
{
	public class EventUnitOfWork : UnitOfWork
	{
		private IDbContext context = null;
		public override IDbContext Context
		{
			get
			{
				if (context == null)
					context = Box.Get<IEventContext>();

				return context;
			}
		}

		private EventCategoryRepository eventCategories;

		public EventCategoryRepository EventCategories {
			get
			{
				if (eventCategories == null)
					eventCategories = new EventCategoryRepository(Context);

				return eventCategories;
			}
		}

		private EventRepository events;

		public EventRepository Events
		{
			get
			{
				if (events == null)
					events = new EventRepository(Context);

				return events;
			}
		}

		public EventUnitOfWork()
		{ }
	}
}