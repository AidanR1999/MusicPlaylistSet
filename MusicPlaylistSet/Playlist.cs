using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Playlist
    {
        public string Name { get; set; }
        public HashSet<string> Songs { get; set; }

        public Playlist(string name)
        {
            Name = name;
            Songs = new HashSet<string>();
        }
    }
}
