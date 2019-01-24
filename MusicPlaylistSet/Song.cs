using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Song(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
