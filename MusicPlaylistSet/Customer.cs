using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Customer
    {
        public bool IsPremium { get; set; }
        public Dictionary<int, Playlist> Library { get; set; }

        public Customer()
        {
            IsPremium = false;
            Library = null;
        }
    }
}
