using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;
using SolrNet.Impl;

namespace Mappen.Domain.Search
{
	public class SearchApi : ISearchApi
	{
		public SolrConnection Connection;
		public ISolrOperations<SearchEvent> Events { get; private set; }

		public SearchApi()
		{
			string url = "https://s-042e4baf.azure.lucidworks.io/solr/events";
			string user = "8qyNfh9JwBtyGqN9";
			string pwd = "x";

			Connection = new SolrNet.Impl.SolrConnection(url)
			{
				HttpWebRequestFactory = new BasicAuthHttpWebRequestFactory(user, pwd)
			};
			Startup.Init<SearchEvent>(Connection);
			Events = ServiceLocator.Current.GetInstance<ISolrOperations<SearchEvent>>();
		}

		public List<SearchEvent> SearchEvents(string query)
		{
			SolrQuery sQuery = new SolrQuery("(Description:" + query + ") OR (Name:" + query + ")");
			ISolrQueryResults<SearchEvent> result = Events.Query(sQuery);
			return result == null ? new List<SearchEvent>() : result.ToList();
		}
	}
}
