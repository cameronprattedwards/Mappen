using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;

namespace Mappen.Domain.Search
{
	public class SearchEvent
	{
		[SolrUniqueKey("id")]
		public int Id { get; set; }

		[SolrField("Name")]
		public string Name { get; set; }

		[SolrField("Description")]
		public string Description { get; set; }

		[SolrField("Website")]
		public string Website { get; set; }
	}
}
