using LPServer.Models.DAL;

namespace LPServer.Models
{
    public class User
    {
        int id;
        String username;
        String password;
        String email;
        String regDate;
        Songs[] userSongs;
        public User() { }

        public User(int id, String username, String password, String email, String regDate) 
        {   
            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.Email = email;
            this.RegDate = regDate;
        }

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public String RegDate { get => regDate; set => regDate = value; }



        public int Insert()
        {
            DBServices dbs = new DBServices();
            return dbs.InsertUser(this);
        }

        public User ValidLoginForm(string email, string password)
        {
            DBServices dbs = new DBServices();
            User user = dbs.ValidLoginForm(email, password);
            return user;
        }

        public List<Songs> GetSongsByUser(int id)
        {
            DBServices ds = new DBServices();
            return ds.GetSongsByUser(id);

        }

        public List<User> GetUsers()
        {
            DBServices dbs = new DBServices();
            return dbs.GetUsers();
        }





    }
}
