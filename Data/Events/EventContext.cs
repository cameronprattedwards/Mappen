using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Data.BaseData;
using Mappen.Domain.Entities.Events;
using Mappen.Domain.Entities.Locations;
using System.Data.Entity;

namespace Mappen.Data.Events
{
	public class EventContext : RealContext<EventContext>, IEventContext
	{
		public DbSet<EventCategory> EventCategories { get; set; }
		public DbSet<Event> Events { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Geocode>().Property(g => g.Lat).HasPrecision(18, 10);
			modelBuilder.Entity<Geocode>().Property(g => g.Lng).HasPrecision(18, 10);
			base.OnModelCreating(modelBuilder);
		}
	}
}