using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cibus.Infrastructure.Rest;
using Mappen.Domain.Entities.Locations;
using Mappen.Domain.EventsDomain.Services;

namespace Mappen.EventsDomain.Controllers
{
	public class EventsController : ApiController
	{
		[AcceptVerbs("GET")]
		public RestReturn Circle(decimal lat, decimal lng)
		{
			return RestReturn.CreateSuccess(EventsServices.Circle(new Geocode(lat, lng)));
		}

		[AcceptVerbs("GET")]
		public RestReturn Search(string q, decimal lat, decimal lng)
		{
			return RestReturn.CreateSuccess(EventsServices.Search(q, new Geocode(lat, lng)));
		}
	}
}
