using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupLoader
{
	class MeetupParameters
	{
		public int Page { get; private set; }
		public int Zip { get; private set; }

		public MeetupParameters(int page, int zip)
		{
			Page = page;
			Zip = zip;
		}
	}
}
