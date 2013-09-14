using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappen.Domain.Entities.Locations
{
	public class Address
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string Street3 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public long Zip { get; set; }
		public Geocode Geocode { get; set; }

		public Address Sync(Address newAddress)
		{
			Name = newAddress.Name;
			Street1 = newAddress.Street1;
			Street2 = newAddress.Street2;
			Street3 = newAddress.Street3;
			City = newAddress.City;
			State = newAddress.State;
			Zip = newAddress.Zip;
			Geocode = newAddress.Geocode;
			return this;
		}
	}
}
