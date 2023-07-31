using System.Data;
using System.Data.SqlClient;
namespace LPServer.Models.DAL
{
    public class DBServices
    {
        
        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("LPServer");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }



        //--------------------------------------------------------------------------------------------------
        // This method Inserts a User to the User table 
        //--------------------------------------------------------------------------------------------------
        public int InsertUser(User user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionStrings"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@username", user.Username);
            paramDic.Add("@email", user.Email);
            paramDic.Add("@password", user.Password);
            

            cmd = CreateCommandWithStoredProcedure("spInsertUser", con, paramDic);             // create the command

            try
            {
                // int numEffected = cmd.ExecuteNonQuery(); // execute the command
                int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }


        //--------------------------------------------------------------------------------------------------
        // This method return User email and password if exist in the User table 
        //--------------------------------------------------------------------------------------------------

        public User ValidLoginForm(string email, string password)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionStrings"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);
            paramDic.Add("@password", password);


            cmd = CreateCommandWithStoredProcedure("SPGetUserByEmail", con, paramDic);             // create the command

            try
            {
                //get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);// CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                User user = new User();

                while (dr.Read())
                {
                    //read till the end of the data per row

                    user.Id = Convert.ToInt32(dr["UserId"]);
                    user.Username = dr["username"].ToString();
                    user.Email = dr["email"].ToString();
                    user.Password = dr["password"].ToString();
                    user.RegDate = dr["regDate"].ToString();
                }
                return user;
            }
            catch (Exception ex)
            {
                //write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        //--------------------------------------------------------------------------------------------------
        // This method return all users from DB 
        //--------------------------------------------------------------------------------------------------

        public List<User> GetUsers()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SPGetUsers", con, null);             // create the command


            List<User> UserList = new List<User>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    User user = new User();

                    user.Id = Convert.ToInt32(dataReader["UserId"]);
                    user.Username = dataReader["username"].ToString();
                    user.Email = dataReader["email"].ToString();
                    user.RegDate = dataReader["regDate"].ToString();


                    UserList.Add(user);
                }
                return UserList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------------------------------------
        // All the GETS FUNCTIONS 
        //--------------------------------------------------------------------------------------------------

        public List<Artist> ReadOnlyUniqueArtist()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SPGetArtist", con, null);             // create the command


            List<Artist> ArtistList = new List<Artist>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Artist a = new Artist();
                    a.Id = Convert.ToInt32(dataReader["artist_id"]);
                    a.Name = dataReader["artist_name"].ToString();
                    a.Img = dataReader["img"].ToString();

                    ArtistList.Add(a);
                }
                return ArtistList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        public List<Songs> GetAllSongs()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SPGetSongs", con, null);             // create the command


            List<Songs> SongsList = new List<Songs>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Songs s = new Songs();
                    //s.Artist = dataReader["artist"].ToString();
                    s.Artist = new Artist
                    {
                        Id = dataReader["artist_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["artist_id"]),
                        Name = dataReader["Artist_name"] is DBNull ? string.Empty : dataReader["Artist_name"].ToString(),
                        Img = dataReader["Img"] is DBNull ? string.Empty : dataReader["Img"].ToString(),
                        FavCounts = dataReader["favCounts"] is DBNull ? 0 : Convert.ToInt32(dataReader["favCounts"])
                    };
                    s.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                    s.Song = dataReader["song"].ToString();
                    s.Text = dataReader["text"].ToString();
                    

                    SongsList.Add(s);
                }
                return SongsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public List<Songs> GetAllSongsWithFav()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SPGetSongWithFav", con, null);             // create the command


            List<Songs> SongsList = new List<Songs>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Songs s = new Songs();
                    //s.Artist = dataReader["artist"].ToString();
                    s.Artist = new Artist
                    {
                        Id = dataReader["artist_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["artist_id"]),
                        Name = dataReader["artist_name"] is DBNull ? string.Empty : dataReader["Artist_name"].ToString(),
                    };
                    s.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                    s.Song = dataReader["song_name"].ToString();
                    s.Text = dataReader["text"].ToString();
                    s.FavCounts = dataReader["favCounts"] is DBNull ? 0 : Convert.ToInt32(dataReader["favCounts"]);


                    SongsList.Add(s);
                }
                return SongsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public List<Artist> GetAllArtistWithFav()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SPGetArtistWithFav", con, null);             // create the command


            List<Artist> ArtistsList = new List<Artist>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Artist a = new Artist();
                    //s.Artist = dataReader["artist"].ToString();
                    a.Id = dataReader["artist_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["artist_id"]);
                    a.Name = dataReader["artist_name"] is DBNull ? string.Empty : dataReader["Artist_name"].ToString();
                    a.FavCounts = dataReader["favCount"] is DBNull ? 0 : Convert.ToInt32(dataReader["favCount"]);

                    ArtistsList.Add(a);
                }
                return ArtistsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        } 
        public List<Songs> GetBySearch(Songs songObj)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Artist_name", songObj.Artist?.Name); // Use ?. to handle null Artist object
            paramDic.Add("@Song", songObj.Song);
            paramDic.Add("@Text", songObj.Text);
            paramDic.Add("@Img", songObj.Artist?.Img); // Use ?. to handle null Artist object

            cmd = CreateCommandWithStoredProcedure("SPGetAllBySearch", con, paramDic); // create the command

            List<Songs> ArtistSearchList = new List<Songs>();

            try
            {
                //get a reader
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dataReader.Read())
                {
                    Songs s = new Songs();
                    //read till the end of the data per row
                    s.Artist = new Artist
                    {
                        Id = dataReader["artist_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["artist_id"]),
                        Name = dataReader["Artist_name"] is DBNull ? string.Empty : dataReader["Artist_name"].ToString(),
                        Img = dataReader["Img"] is DBNull ? string.Empty : dataReader["Img"].ToString(),
                        FavCounts = dataReader["favCounts"] is DBNull ? 0 : Convert.ToInt32(dataReader["favCounts"])
                    };
                    s.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                    s.Song = dataReader["Song"] is DBNull ? string.Empty : dataReader["Song"].ToString();
                    s.Text = dataReader["Text"] is DBNull ? string.Empty : dataReader["Text"].ToString();
                    ArtistSearchList.Add(s);
                }
                return ArtistSearchList;
            }
            catch (Exception ex)
            {
                //write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }



        public int insertSong_playList(PlayList playList)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", playList.User_id); // Use ?. to handle null Artist object
            paramDic.Add("@song_id", playList.Song_id);

            cmd = CreateCommandWithStoredProcedure("spInsertTo_PlayList", con, paramDic); // create the command

            try
            {
                // int numEffected = cmd.ExecuteNonQuery(); // execute the command
                int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }



        public int Delete1(PlayList playList)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", playList.User_id); // Use ?. to handle null Artist object
            paramDic.Add("@song_id", playList.Song_id);

            cmd = CreateCommandWithStoredProcedure("spDeleteFrom_PlayList", con, paramDic); // create the command

            try
            {
                // int numEffected = cmd.ExecuteNonQuery(); // execute the command
                int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        public List<Songs> GetSongsByUser(int userId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = new SqlCommand("SPGetSongsByUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@user_id", userId);

            List<Songs> songsList = new List<Songs>();

            try
            {
                //get a reader
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Songs s = new Songs();
                    //read till the end of the data per row
                    s.Artist = new Artist
                    {
                        Id = dataReader["ArtistId"] is DBNull ? 0 : Convert.ToInt32(dataReader["ArtistId"]),
                        Name = dataReader["ArtistName"] is DBNull ? string.Empty : dataReader["ArtistName"].ToString(),
                        Img = dataReader["Img"] is DBNull ? string.Empty : dataReader["Img"].ToString(),
                        FavCounts = dataReader["FavCounts"] is DBNull ? 0 : Convert.ToInt32(dataReader["FavCounts"])
                    };
                    s.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                    s.Song = dataReader["Song"] is DBNull ? string.Empty : dataReader["Song"].ToString();
                    s.Text = dataReader["Text"] is DBNull ? string.Empty : dataReader["Text"].ToString();
                    songsList.Add(s);
                }
                return songsList;
            }
            catch (Exception ex)
            {
                //write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }



        //--------------- Get Song By SongId ----------------------


        public Songs GetSongBySongId(int songId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = new SqlCommand("SPGetSongsBySong_id", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@song_id", songId);

            Songs song = null;

            try
            {
                //get a reader
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read()) // Check if there's at least one row returned
                {
                    song = new Songs();

                    // read the data from the first row
                    song.Artist = new Artist
                    {
                        Id = dataReader["ArtistId"] is DBNull ? 0 : Convert.ToInt32(dataReader["ArtistId"]),
                        Name = dataReader["ArtistName"] is DBNull ? string.Empty : dataReader["ArtistName"].ToString(),
                        Img = dataReader["Img"] is DBNull ? string.Empty : dataReader["Img"].ToString(),
                        FavCounts = dataReader["FavCounts"] is DBNull ? 0 : Convert.ToInt32(dataReader["FavCounts"])
                    };
                    song.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                    song.Song = dataReader["Song"] is DBNull ? string.Empty : dataReader["Song"].ToString();
                    song.Text = dataReader["Text"] is DBNull ? string.Empty : dataReader["Text"].ToString();
                }
                return song;
            }
            catch (Exception ex)
            {
                //write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //--------------- Get Artist By Artist Id ----------------------


        public Artist GetArtistWithSongsById(int artistId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = new SqlCommand("SPGetArtistById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@artist_id", artistId);



            try
            {
                //get a reader
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                Artist a = new Artist();
                if (dataReader.Read())
                {
                    a.Id = dataReader["artist_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["artist_id"]);
                    a.Name = dataReader["artist_name"] is DBNull ? string.Empty : dataReader["artist_name"].ToString();
                    a.Img = dataReader["img"] is DBNull ? string.Empty : dataReader["img"].ToString();
                    a.FavCounts = dataReader["favCount"] is DBNull ? 0 : Convert.ToInt32(dataReader["favCount"]);
                }


                //cmd = new SqlCommand("SPGetArtistSongsByAId", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@artist_id", artistId);

                //List<Songs> lSongs = new List<Songs>();

                //while (dataReader.Read()) // Check if there's at least one row returned
                //{
                //    Songs song = new Songs()
                //    song.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                //    song.Song = dataReader["Song"] is DBNull ? string.Empty : dataReader["Song"].ToString();
                //    song.Text = dataReader["Text"] is DBNull ? string.Empty : dataReader["Text"].ToString();

                //    a.Songs.Add(song);
                //}

                return a;
            }
            catch (Exception ex)
            {
                //write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Songs> GetSongsByArtistId(int artistId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("LPServer"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            // create the command
            cmd = new SqlCommand("SPGetArtistSongsByAId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@artist_id", artistId);

            List<Songs> SongsList = new List<Songs>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Songs s = new Songs();
                    //s.Artist = dataReader["artist"].ToString();
                    s.Artist = new Artist
                    {
                        Id = dataReader["ArtistId"] is DBNull ? 0 : Convert.ToInt32(dataReader["ArtistId"]),
                        Name = dataReader["ArtistName"] is DBNull ? string.Empty : dataReader["ArtistName"].ToString(),
                        Img = dataReader["Img"] is DBNull ? string.Empty : dataReader["Img"].ToString()
                    };
                    s.Id = dataReader["song_id"] is DBNull ? 0 : Convert.ToInt32(dataReader["song_id"]);
                    s.Song = dataReader["Song"].ToString();
                    s.Text = dataReader["Text"].ToString();
                    s.FavCounts = dataReader["FavCounts"] is DBNull ? 0 : Convert.ToInt32(dataReader["favCounts"]);


                    SongsList.Add(s);
                }
                return SongsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

    }
}
