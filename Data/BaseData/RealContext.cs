using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using Cibus.Infrastructure.Interfaces;
using Cibus.Infrastructure.IocBox;

namespace Mappen.Data.BaseData
{
	public class RealContext<TContext> : DbContext, IDbContext where TContext : DbContext
	{
		public RealContext()
			: base("Mappen")
		{
			Database.SetInitializer<TContext>(new DropCreateDatabaseIfModelChanges<TContext>());
		}


		public void ChangeStates<T>(T entity) where T : class
		{ }

		public ObjectContext ObjectContext
		{
			get
			{
				return ((IObjectContextAdapter)this).ObjectContext;
			}
		}

		public IDbSet<T> GetDbSet<T>() where T : class
		{
			return this.Set<T>() as IDbSet<T>;
		}

		public void DetectChanges()
		{
			ChangeTracker.DetectChanges();
		}

		public void RemoveEntity<T>(T entity) where T : class
		{
			ObjectContext.DeleteObject(entity);
		}

		/// <summary>
		/// This virtual method allows subclasses to set navigation 
		/// properties' state to 'unchanged'. This is for objects
		/// that are already in the database (so that they don't 
		/// get added twice).
		///
		/// Do nothing by default.
		/// </summary>
		protected virtual void SetNavigationPropertiesUnchanged() { }

		public void SetState(EntityState state, object entity)
		{
			this.Entry(entity).State = state;
		}

		public override int SaveChanges()
		{
			SetNavigationPropertiesUnchanged();
			return base.SaveChanges();
		}
	}
}
