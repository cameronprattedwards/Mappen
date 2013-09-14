using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Data.BaseData
{
	public interface IUnitOfWork : IDisposable
	{
		IDbContext Context { get; }
		////T Repo<T>() where T : class;
		int Save();
		void SetState(EntityState state, object entity);
	}
}
