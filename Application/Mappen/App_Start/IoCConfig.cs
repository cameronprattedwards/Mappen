using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Cibus.Infrastructure.IocContainers;
using Ninject;
using Cibus.Infrastructure.IocBox;

namespace MappenSite
{
	public static class IocConfig
	{
		public static void RegisterIoc()
		{
			Box.Module = new StandardKernel(
				new InfrastructureRealDataModule(),
				new Mappen.Data.IocContainersData.RealDataModule(),
				new Mappen.Domain.IocContainersDomain.RealDataModule()
			);
		}
	}
}
