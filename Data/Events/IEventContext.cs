using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Data.BaseData;
using Mappen.Domain.Entities.Events;
using System.Data.Entity;

namespace Mappen.Data.Events
{
	public interface IEventContext : IDbContext
	{
		DbSet<Event> Events { get; }
	}
}