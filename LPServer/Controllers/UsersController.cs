using Microsoft.AspNetCore.Mvc;
using LPServer.Models;

namespace LPServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public List<User> Get()
        {
            User user = new User();
            return user.GetUsers();
        }

        // GET: api/<PlayListsController>/5
        [HttpGet("{id}")]
        public IEnumerable<Songs> Get(int id)
        {
            User u = new User();
            return u.GetSongsByUser(id);
        }


        [HttpGet]
        [Route("ValidLogin")]    //this is using a Query string ?'email=${email} & password=${password}'
        public IActionResult ValidLogin(string email, string password)
        {
            User user = new User();
            var result = user.ValidLoginForm(email, password);
            if (result.Email != null)
            {
                return Ok(result);
            }

            return NotFound(); // Return 404 if the user is not found or login is invalid
        }


        // POST api/<UsersController>
        [HttpPost]
        public int Post([FromBody] User u)
        {
            return u.Insert();
        }
    }
}
