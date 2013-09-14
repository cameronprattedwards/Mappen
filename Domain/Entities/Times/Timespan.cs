using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Mappen.Domain.Entities.Times
{
	public class Timespan
	{
		public int Id { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public Timespan() { }

		public Timespan(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}

		public bool Contains(DateTime time)
		{
			return Start.Ticks <= time.Ticks && time.Ticks <= End.Ticks;
		}

		public Timespan Sync(Timespan newSpan)
		{
			Start = newSpan.Start;
			End = newSpan.End;
			return this;
		}
	}
}
