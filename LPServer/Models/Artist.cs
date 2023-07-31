using LPServer.Models.DAL;

namespace LPServer.Models
{
    public class Artist
    {
        int id;
        string name;
        List<Songs> songs;
        string img;
        int favCounts; //how many like the artist

        public Artist() { 
        
        }

        public Artist(int id, string name, string img, int favCounts)
        {
            this.Id = id;
            this.Name = name;
            this.Img = img;
            this.FavCounts = favCounts;
        }


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        //public List<Songs> Songs { get => songs; set => songs = value; }
        public string Img { get => img; set => img = value; }
        public int FavCounts { get => favCounts; set => favCounts = value; }

        static public List<Artist> GetAllArtists()
        {
            DBServices dbs = new DBServices();
            return dbs.ReadOnlyUniqueArtist();
        }
        
         public List<Artist> GetAllArtistWithFav()
         {
            DBServices ds = new DBServices();
            return ds.GetAllArtistWithFav();

         }

        public Artist GetArtistWithSongsById(int id)
        {
            DBServices ds = new DBServices();
            return ds.GetArtistWithSongsById(id);
        }
    }
}
