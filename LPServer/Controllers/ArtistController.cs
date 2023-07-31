using LPServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace LPServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        //GET api/<SongsController>
        [HttpGet]
        public IEnumerable<Artist> Get()
        {
            return Artist.GetAllArtists();
        }

        [HttpGet("{id}")]
        public Artist Get(int id)
        {
            Artist a = new Artist();
            return a.GetArtistWithSongsById(id);
        }

        [HttpGet]
        [Route("GetAllArtistWithFav")]
        public IEnumerable<Artist> GetAllArtistWithFav()
        {
            Artist artists = new Artist();
            List<Artist> aList = artists.GetAllArtistWithFav();
            return aList;
        }
    }
}
