using SolrNet;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Search
{
	public interface ISearchApi
	{
		List<SearchEvent> SearchEvents(string query);
	}
}
