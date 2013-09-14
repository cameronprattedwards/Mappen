using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;

namespace Mappen.Data.BaseData
{
	public abstract class UnitOfWork : IUnitOfWork
	{
		public abstract IDbContext Context { get; }

		/// <summary>
		/// Checks to see if the connection is valid
		/// </summary>
		/// <returns></returns>
		public bool ConnectionIsValid()
		{
			try
			{
				DbContext context = (DbContext)Context;
				context.Database.Connection.Open();
				context.Database.Connection.Close();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		public int Save()
		{
			bool saveFailed;
			saveFailed = false;
			try
			{
				return Context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("EF Validation Errors: ");
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						sb.AppendLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
					}
				}
				throw new InvalidOperationException(sb.ToString());
			}
			catch (DbUpdateConcurrencyException ex)
			{
				// Get failed entry...
				var entry = ex.Entries.Single();
				var values = entry.GetDatabaseValues();
				if (values == null)
				{
					return 0;
				}
				// Overwrite original values with values from the database, 
				// but don't touch current values where changes are held
				entry.OriginalValues.SetValues(values);

				saveFailed = true;
				Console.WriteLine("Save Failed");
			}
			catch (OptimisticConcurrencyException e)
			{
				var entry = e.StateEntries.Single();
				if (entry != null)
				{
					((ObjectContext)Context).Refresh(RefreshMode.StoreWins, entry);
				}
				saveFailed = true;
				Console.WriteLine("Save Failed");
			}
			//} while (saveFailed);

			return 1;
		}

		public void DetectChanges()
		{
			Context.DetectChanges();
		}

		public void Dispose()
		{
			Context.Dispose();
		}

		public void SetState(EntityState state, object entity)
		{
			Context.SetState(state, entity);
		}
	}
}
