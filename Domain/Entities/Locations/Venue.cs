using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Entities.Locations
{
	public enum SubaddressType
	{ 
		Room,
		Apartment,
		Floor,
		Unit,
		Suite
	}

	public class Venue
	{
		public int Id { get; set; }
		public Address Address { get; set; }
		public string Name { get; set; }
		public long Phone { get; set; }
		public string Email { get; set; }
		public SubaddressType SubaddressType { get; set; }
		public int SubaddressNumber { get; set; }

		public Venue Sync(Venue newVenue)
		{
			Address.Sync(newVenue.Address);
			Name = newVenue.Name;
			Phone = newVenue.Phone;
			Email = newVenue.Email;
			SubaddressType = newVenue.SubaddressType;
			SubaddressNumber = newVenue.SubaddressNumber;
			return this;
		}
	}
}
