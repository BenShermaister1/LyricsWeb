namespace LPServer.Models.DAL
{
    public class PlayList
    {
        int user_id;
        int song_id;
 

        public PlayList(){}

        public PlayList(int user_id, int song_id)
        {
            this.user_id = user_id;
            this.song_id = song_id;
        }

        public int User_id { get => user_id; set => user_id = value; }
        public int Song_id { get => song_id; set => song_id = value; }


        public int Insert()
        {
            DBServices ds = new DBServices();
            return ds.insertSong_playList(this);
        }

        public int Delete()
        {
            DBServices ds = new DBServices();
            return ds.Delete1(this);

        }

        







    }




}
