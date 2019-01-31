using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HashSet<Song> Songs { get; set; }

        public Playlist(int id, string name)
        {
            Id = 0;
            Name = name;
            Songs = new HashSet<Song>();
        }

        public int setSongIndex(int songId)
        {
            int index = 1;
            foreach (Song song in Songs)
            {
                if (song.Id == songId)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public int getSongIndex(Song song)
        {
            int index = 1;

            foreach (Song songLoop in Songs)
            {
                if (song.Id == songLoop.Id)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public void removeSongFromPlaylist(int songNum)
        {
            foreach (Song song in Songs)
            {
                if (songNum == setSongIndex(song.Id))
                {
                    Songs.Remove(song);
                }
            }
        }
    }
}
