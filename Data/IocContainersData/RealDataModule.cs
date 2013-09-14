using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Mappen.Data.Events;

namespace Mappen.Data.IocContainersData
{
	public class RealDataModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IEventContext>().To<EventContext>();
		}
	}
}
