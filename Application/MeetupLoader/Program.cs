using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Meetup.Services;
using Mappen.Domain.Meetup.Models;
using Mappen.Data.Events;
using Mappen.Domain.Entities.Events;
using Cibus.Infrastructure.IocBox;
using Mappen.Data.IocContainersData;
using Ninject;

namespace MeetupLoader
{
	class Program
	{

		static void Main(string[] args)
		{
			Box.Module = new StandardKernel(new RealDataModule());
			List<MeetupParameters> myParams = new List<MeetupParameters>() { 
				new MeetupParameters(0, 84010),
				new MeetupParameters(0, 11211),
				new MeetupParameters(1, 11211),
				new MeetupParameters(2, 11211),
				new MeetupParameters(3, 11211),
				new MeetupParameters(4, 11211),
				new MeetupParameters(5, 11211),
				new MeetupParameters(6, 11211),
				new MeetupParameters(7, 11211),
				new MeetupParameters(8, 11211),
				new MeetupParameters(9, 11211),
				new MeetupParameters(10, 11211),
				new MeetupParameters(11, 11211),
				new MeetupParameters(12, 11211),
				new MeetupParameters(13, 11211),
				new MeetupParameters(14, 11211),
				new MeetupParameters(15, 11211),
				new MeetupParameters(16, 11211),
				new MeetupParameters(17, 11211),
				new MeetupParameters(18, 11211),
				new MeetupParameters(19, 11211),
				new MeetupParameters(0, 90071),
				new MeetupParameters(1, 90071),
				new MeetupParameters(2, 90071),
				new MeetupParameters(3, 90071),
				new MeetupParameters(4, 90071),
				new MeetupParameters(5, 90071),
				new MeetupParameters(6, 90071),
				new MeetupParameters(7, 90071),
				new MeetupParameters(8, 90071),
				new MeetupParameters(9, 90071)
			};

			foreach (MeetupParameters myParam in myParams)
			{
				MeetupServices.Load(myParam.Page, myParam.Zip);
				System.Threading.Thread.Sleep(10000);
			}


			Console.Read();
		}
	}
}
