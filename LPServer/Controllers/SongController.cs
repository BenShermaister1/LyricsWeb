using Microsoft.AspNetCore.Mvc;
using LPServer.Models;
using LPServer.Models.DAL;

namespace LPServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        //GET api/<SongsController>
        [HttpGet]
        public IEnumerable<Songs> Get()
        {
            return Songs.GetAllSongs();
        }

        [HttpGet]
        [Route("GetAllSongsWithFav")]
        public IEnumerable<Songs> GetAllSongsWithFav()
        {
            Songs songs = new Songs();
            List<Songs> sList = songs.GetAllSongsWithFav();
            return sList;
        }

        [HttpPost]
        [Route("GetBySearch")]
        public IEnumerable<Songs> GetBySearch(Songs songObj)
        {
            Songs s = new Songs();
            return s.GetBySearch(songObj);
        }

        // GET: api/<SongsController>/5
        [HttpGet("{id}")]
        public Songs Get(int id)
        {
            Songs s = new Songs();
            return s.GetSongBySongId(id);
        }

        [HttpGet]
        [Route("GetSongsByArtistId")]
        public IEnumerable<Songs> GetSongsByArtistId(int id)
        {
            Songs songs = new Songs();
            List<Songs> sList = songs.GetSongsByArtistId(id);
            return sList;
        }
    }
}
