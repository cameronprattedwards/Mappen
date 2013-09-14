using Mappen.Data.BaseData;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using Cibus.Infrastructure.Utils;
using System.ComponentModel;
using System.Reflection;

namespace Mappen.Data.BaseData
{
	public class Repository<T> : IEntityRepository<T> where T : class
	{
		protected IDbContext Context { get; set; }

		public IDbSet<T> DbSet
		{
			get
			{
				return Context.GetDbSet<T>();
			}
		}

		public Repository(IDbContext context)
		{
			Context = context;
		}

		public T FirstOrDefault()
		{
			return DbSet.FirstOrDefault();
		}

		public IEnumerable<T> Including(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = DbSet;
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}

			return query;
		}

		public IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = DbSet;
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
			return query.ToList();
		}

		/// <summary>
		/// overridden in Repository
		/// </summary>
		/// <param name="entity"></param>
		public virtual void Add(T entity)
		{
			Update(entity);
		}

		public void AddRange(IEnumerable<T> list)
		{
			foreach (T t in list)
			{
				Add(t);
			}
		}

		public virtual void Clear()
		{
			List<object> toRemove = new List<object>();
			foreach (var entity in DbSet)
			{
				toRemove.Add(entity);
			}
			foreach (var obj in toRemove)
			{
				DbSet.Remove((T)obj);
			}
		}

		public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
		{
			if (predicate == null)
			{
				return new List<T>();
			}

			return DbSet.Where(predicate);
		}

		public IEnumerable<T> WhereIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
		{
			if (predicate == null)
			{
				return new List<T>();
			}

			var query = DbSet.Where(predicate);
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
			return query.ToList();
		}

		public IEnumerable<T> WhereNoTracking(Expression<Func<T, bool>> predicate)
		{
			if (predicate == null)
			{
				return new List<T>();
			}

			return DbSet.Where(predicate).AsNoTracking();
		}

		public IEnumerable<T> All
		{
			get
			{
				return DbSet;
			}
		}

		public void Remove(T entity)
		{
			DbSet.Remove(entity);
		}

		public void Update(T entity)
		{
			DbSet.Add(entity);
			Context.ChangeStates(entity);
		}

		public int Count()
		{
			return DbSet.Count();
		}

		public IEnumerable<T> Subset(Func<T, object> orderClause, int bottom, int step)
		{
			return DbSet.OrderBy(orderClause).Skip(bottom).Take(step).ToList();
		}

		public void BulkClear<T>(string tableName = null)
		{
			if (string.IsNullOrEmpty(tableName))
			{
				tableName = GetTableName<T>();
			}

			if (Context is FakeContext)
			{
				throw new InvalidOperationException("Cannot do bulk clears on fake context");
			}
			DbContext context = (DbContext)Context;
			string sql = "delete from dbo." + tableName;
			context.Database.ExecuteSqlCommand(sql);
		}

		private string GetTableName<T>()
		{
			return typeof(T).Name + "s";
		}

		public void BulkInsert<T>(string tableName, IList<T> data, bool keepIdentity = false) where T : class
		{
			if (Context is FakeContext)
			{
				throw new InvalidOperationException("Cannot do bulk inserts on fake context");
			}
			MethodInfo insertRowMethod = typeof(Repository<T>).GetMethod("GetRecordValues");
			MethodInfo mapColumns = typeof(Repository<T>).GetMethod("MapColumns");
			DbContext context = (DbContext)Context;

			if (string.IsNullOrEmpty(tableName))
			{
				tableName = GetTableName<T>();
			}

			// Make the bulk copy connection to the database, and add the data into it
			string connection = context.Database.Connection.ConnectionString;
			if (!connection.ToLower().Contains("password"))
			{
				connection += ";Password=Park3r12";
			}
			connection += "; Charset=utf8";
			using (var bulkCopy =
				new SqlBulkCopy(connection, keepIdentity
				? SqlBulkCopyOptions.KeepIdentity
				: SqlBulkCopyOptions.Default))
			{
				bulkCopy.BatchSize = data.Count;
				bulkCopy.DestinationTableName = "dbo." + tableName;
				bulkCopy.BulkCopyTimeout = UtilDefs.DbTimeout;

				var table = new DataTable();

				// Map all the columns for the table
				int columnCount = MapColumns<T>(bulkCopy, string.Empty, table, mapColumns);

				int start = 0;
				int skip = 1000;
				int end = start + skip;
				end = end > data.Count - 1 ? data.Count - 1 : end;
				while (start < data.Count)
				{
					table.Rows.Clear();
					for (int i = start; i < end; i++)
					{
						T obj = data[i];
						// Fill a list of values for this record
						var values = new List<object>();
						GetRecordValues<T>(obj, values, insertRowMethod);

						// Add the record to the table
						table.Rows.Add(values.ToArray());
					}

					// Write the whole table to the server in one pop
					bulkCopy.WriteToServer(table);
					start = end;
					end += skip;
					end = end > data.Count ? data.Count : end;
				}
			}
		}

		private void SetId(object obj, int p)
		{
			throw new NotImplementedException();
		}

		public void GetRecordValues<T>(object obj, List<object> values, MethodInfo getRecordValues) where T : class
		{
			List<PropertyInfo> properties = typeof(T).GetProperties().Where(p => ReflectionUtils.IsMapped(p)).ToList();
			foreach (PropertyInfo prop in properties)
			{
				object val = prop.GetValue(obj);
				if (ReflectionUtils.IsPrimitiveOrDateTime(prop))
				{
					values.Add(val);
				}
				else
				{
					// Make the generic method for val's type and call it
					MethodInfo genMethod = getRecordValues.MakeGenericMethod(val.GetType());
					genMethod.Invoke(this, new object[] { val, values, getRecordValues });
				}
			}
		}

		/// <summary>
		/// Map all the columns for the bulk copy table
		/// This is recursive, in order to handle nested objects.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bulkCopy"></param>
		/// <param name="prefix"></param>
		/// <param name="table"></param>
		/// <param name="mapColumns"></param>
		/// <returns></returns>
		public int MapColumns<T>(SqlBulkCopy bulkCopy, string prefix, DataTable table, MethodInfo mapColumns) where T : class
		{
			if (string.IsNullOrEmpty(prefix))
			{
				prefix = string.Empty;
			}
			var props = typeof(T).GetProperties().Cast<PropertyInfo>().Where(p => ReflectionUtils.IsMapped(p));

			int columnCount = 0;
			foreach (var prop in props)
			{
				if (ReflectionUtils.IsPrimitiveOrDateTime(prop))
				{
					string name = prefix + prop.Name;
					bulkCopy.ColumnMappings.Add(name, name);
					table.Columns.Add(name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
					columnCount++;
				}
				else
				{
					Type type = prop.PropertyType.IsEnum ? typeof(Int32) : prop.PropertyType;
					MethodInfo method = mapColumns.MakeGenericMethod(type);
					int count = (int)method.Invoke(this, new object[] { bulkCopy, prefix + prop.Name + "_", table, mapColumns });
					columnCount += count;
				}
			}
			return columnCount;
		}
	}
}
