using LPServer.Models.DAL;

namespace LPServer.Models
{
    public class Songs
    {
        int id;
        string song;
        string text;
        Artist artist;
        int favCounts;


        public Songs() { }

        public Songs(int id, string song, string text, Artist artist, int favCounts)
        {
            this.Id = id;
            this.Song = song;
            this.Text = text;
            this.Artist = artist;
            this.favCounts = favCounts;
        }

        public int Id { get => id; set => id = value; }
        public string Song { get => song; set => song = value; }
        public string Text { get => text; set => text = value; }
        public Artist Artist { get => artist; set => artist = value; }
        public int FavCounts { get => favCounts; set => favCounts = value; }

        static public List<Songs> GetAllSongs()
        {
            DBServices dbs = new DBServices();
            return dbs.GetAllSongs();
        }
        public List<Songs> GetBySearch(Songs songObj)
        {
            DBServices ds = new DBServices();
            return ds.GetBySearch(songObj);

        }

        public List<Songs> GetAllSongsWithFav()
        {
            DBServices ds = new DBServices();
            return ds.GetAllSongsWithFav();

        }

        public Songs GetSongBySongId(int id)
        {
            DBServices ds = new DBServices();
            return ds.GetSongBySongId(id);

        }

        public List<Songs> GetSongsByArtistId(int id)
        {
            DBServices ds = new DBServices();
            return ds.GetSongsByArtistId(id);

        }
    }
}
