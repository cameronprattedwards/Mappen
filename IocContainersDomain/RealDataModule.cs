using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Mappen.Domain.Search;

namespace Mappen.Domain.IocContainersDomain
{
    public class RealDataModule : NinjectModule
    {
		public override void Load()
		{
			Bind<ISearchApi>().To<SearchApi>().InSingletonScope();
		}
    }
}
