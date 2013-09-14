using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibus.Infrastructure.Rest;
using Mappen.Domain.Meetup.Models;
using RestSharp;

namespace Mappen.Domain.Meetup.Services
{
	public class MeetupApi
	{
		private static string key = "1e11103e5e5a284f11255a378011b";

		private static T Execute<T>(RestRequest request) where T : new()
		{
			var client = new RestClient();
			client.BaseUrl = "http://api.meetup.com/";
			var response = client.Execute<T>(request);
			return response.Data;
		}

		private static List<MeetupEvent> RequestEvents(RestRequest request)
		{
			return Execute<MeetupEventResponse>(request).Results.Where(e => e.Venue != null).ToList();
		}

		private static List<MeetupGroup> RequestGroups(RestRequest request)
		{
			return Execute<MeetupGroupResponse>(request).Results;
		}

		public static List<MeetupEvent> GetEvents(int page, int zip)
		{
			RestRequest request = new RestRequest("2/open_events.json/?offset="+page.ToString()+"&text_format=plain&zip="+zip.ToString()+"&order=distance&key=" + key);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept-Charset","utf-8");
			List<MeetupEvent> events = RequestEvents(request);
			IEnumerable<string> ids = from e in events select e.Group.Id;
			string joined = string.Join(",", ids.ToList());
			RestRequest groupsRequest = new RestRequest("2/groups.json?group_id=" + joined + "&key=" + key);
			groupsRequest.RequestFormat = DataFormat.Json;
			groupsRequest.AddHeader("Accept-Charset", "utf-8");
			List<MeetupGroup> requestedGroups = RequestGroups(groupsRequest);
			events = events.Join<MeetupEvent, MeetupGroup, string, MeetupEvent>(requestedGroups, e => e.Group.Id, g => g.Id, (e, g) => { e.Group = g; return e; }).ToList();
			return events;
		}

	}
}
