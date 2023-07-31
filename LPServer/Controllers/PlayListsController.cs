using Microsoft.AspNetCore.Mvc;
using LPServer.Models;
using LPServer.Models.DAL;

namespace LPServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayListsController : ControllerBase
    {


        // POST api/<PlayListsController>
        [HttpPost]
        public int Post([FromBody] PlayList playList)
        {
            return playList.Insert();
        }

        [HttpDelete]
        // DELETE api/<PlayListsController>
        public int Delete([FromBody] PlayList playList)
        {
            return playList.Delete();

        }




    }
}
