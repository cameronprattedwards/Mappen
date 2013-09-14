using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Data.BaseData
{
	public interface IDbContext : IDisposable
	{
		int SaveChanges();
		void DetectChanges();
		void ChangeStates<T>(T entity) where T : class;
		IDbSet<T> GetDbSet<T>() where T : class;
		void RemoveEntity<T>(T entity) where T : class;
		void SetState(EntityState state, object entity);
	}
}
