using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Data.BaseData
{
	public class FakeContext : IDbContext
	{
		public Dictionary<Type, object> DbSetMap { get; set; }

		public FakeContext()
		{
			DbSetMap = new Dictionary<Type, object>();
		}

		protected void AddDbSet<T>(IDbSet<T> dbSet) where T : class
		{
			DbSetMap.Add(typeof(T), dbSet);
		}

		public int SaveChanges()
		{
			return 1;
		}

		public void DetectChanges()
		{
			return;
		}

		public void Dispose()
		{
		}

		public IDbSet<T> GetDbSet<T>() where T : class
		{
			return DbSetMap[typeof(T)] as IDbSet<T>;
		}

		public void RemoveEntity<T>(T entity) where T : class
		{
			IDbSet<T> set = GetDbSet<T>();
			set.Remove(entity);
		}

		// We do no state changing logic for external databases
		public void ChangeStates<T>(T entity) where T : class
		{
		}

		public void SetState(EntityState state, object entity)
		{
			return; // Don't do anything in the fake context
		}
	}
}
