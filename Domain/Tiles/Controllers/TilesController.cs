using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cibus.Infrastructure.Rest;
using Tiles.Models;
using Tiles.Services;
using Mappen.Domain.Entities.Locations;

namespace Mappen.Tiles.Controllers
{
    public class TilesController : ApiController
    {
		[AcceptVerbs("POST")]
		public RestReturn Get(string id, List<Corner> corners)
		{
			List<Tile> tiles = TileServices.GetTiles(corners);
			return RestReturn.CreateSuccess(tiles);
		}
    }
}
