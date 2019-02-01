using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Playlist
    {
        //Public Properties.
        public int Id { get; set; }
        public string Name { get; set; }
        public HashSet<Song> Songs { get; set; }
        
        /// <summary>
        /// Blank Constructor.
        /// </summary>
        public Playlist()
        {
            //Sets Properties equal to their null values.
            Id = 0;
            Name = null;
            Songs = new HashSet<Song>();
        }

        /// <summary>
        ///Overloaded Constructor accepting 1 argument.
        /// </summary>
        /// <param name="name"></param>
        public Playlist(string name)
        {
            //Sets Id and Songs to their null values and sets Propertie Name equal to input parameter string name.
            Id = 0;
            Name = name;
            Songs = new HashSet<Song>();
        }

        /// <summary>
        /// Overloaded Constructor accepting 2 arguments.
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        public Playlist(int id, string name)
        {
            //Sets Songs to its null value and sets Properties Id and Name equal to input parameters int id and string name, Respectively.
            Id = id;
            Name = name;
            Songs = new HashSet<Song>();
        }

        /// <summary>
        /// Overloaded Constuctor accepting 3 arguments.
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        /// <param name="songs">HashSet<Song></param>
        public Playlist(int id, string name, HashSet<Song> songs)
        {
            //Sets properties Id, Name and Songs equal to input parameters int id, string name, HashSet<Song> songs, respectively.
            Id = id;
            Name = name;
            Songs = songs;
        }

        /// <summary>
        /// Sets song index and returns that index or -1 if it fails.
        /// </summary>
        /// <param name="songId">int</param>
        /// <returns>int index (or -1 if fails)</returns>
        public int setSongIndex(int songId)
        {
            //Local variables.
            int index = 1;

            //Loops Propertie Songs.
            foreach (Song song in Songs)
            {
                //Checks each songs id to input parameter int songId.
                if (song.Id == songId)
                {
                    //Returns index if song.id is equal to input parameter int songId.
                    return index;
                }

                //Increments local variable int index.
                index++;
            }

            //Returns -1 if fails.
            return -1;
        }

        /// <summary>
        /// Finds song index and returns that index or returns -1 if fails.
        /// </summary>
        /// <param name="song">Song Class Variable</param>
        /// <returns>int index (or -1 if fails)</returns>
        public int getSongIndex(Song song)
        {
            //Local variables.
            int index = 1;

            //Loops Propertie Songs.
            foreach (Song songLoop in Songs)
            {
                //Checks id of input parameter song against id of foreach local variable songLoop.
                if (song.Id == songLoop.Id)
                {
                    //Returns index if condition is true.
                    return index;
                }

                //Increments local variable index.
                index++;
            }

            //Returns -1 if fails.
            return -1;
        }

        /// <summary>
        /// Removes a song from a Playlist.
        /// </summary>
        /// <param name="songNum">int</param>
        public void removeSongFromPlaylist(int songNum)
        {
            //Loops Propertie Songs.
            foreach (Song song in Songs)
            {
                //Checks if input parameter int songNum is equal to return value of method setSongIndex with parameter
                //id of foreach local variable song.
                if (songNum == setSongIndex(song.Id))
                {
                    //Removes foreach local variable song from Songs propertie if condition is true.
                    Songs.Remove(song);

                    //Stops reiteration.
                    break;
                }
            }
        }
    }
}
