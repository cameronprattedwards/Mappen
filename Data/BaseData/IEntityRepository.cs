using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Data.BaseData
{
	public interface IEntityRepository<T>
	{
		IEnumerable<T> All { get; }
		IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeParameters);
		int Count();
		T FirstOrDefault();
		void Add(T entity);
		void AddRange(IEnumerable<T> list);
		IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
		/// <summary>
		/// This runs the predicate and retrieves the entities without
		/// tracking in the context. This results in a faster query with
		/// less memory use.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		IEnumerable<T> WhereNoTracking(Expression<Func<T, bool>> predicate);
		void Remove(T entity);
		void Update(T entity);
		IEnumerable<T> Subset(Func<T, object> orderClause, int bottom, int step);
		void Clear();
	}

}
