using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Program
    {
        private static List<string> allSongs;
        private static Customer customer;

        static void Main(string[] args)
        {
            storeSongs();
            customer = new Customer();

            login();
            MainMenu();


            Console.ReadKey();
        }

        public static void storeSongs()
        {
            using (StreamReader sr = new StreamReader("songTitleDataSet.txt"))
            {
                string line;
                allSongs = new List<string>();

                while ((line = sr.ReadLine()) != null)
                {
                    allSongs.Add(line);
                }
            }
        }

        public static void getSongs()
        {
            int i = 1;
            foreach (string song in allSongs)
            {
                Console.WriteLine($"{i}. {song}");
                i++;
            }

            addSong();
        }

        public static void addSong()
        {
            int songNum = 0;
            string[] allSongsArr = allSongs.ToArray();
            string songName = "";

            string songString = Console.ReadLine();

            try
            {
                songNum = Int32.Parse(songString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Please enter a number");
            }

            for (int i = allSongs.Count; i > 1; i--)
            {
                if (songNum == i)
                {
                    songName = allSongsArr[i - 1];
                }
            }

            if (customer.Library.Count != 0)
            {
                Console.Clear();
                Console.WriteLine($"Would you like to add {songName} to one of these playlists:");

                showPlaylists();

                string playlistNum = Console.ReadLine();

            }
            else
            {
                Console.WriteLine("Create a playlist");
            }
        }

        public static void login()
        {
            Console.WriteLine("Are you a premuim member?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            string optionString = Console.ReadLine();

            if (optionString.Equals("1"))
                customer.IsPremium = true;

            Console.Clear();
        }

        public static void MainMenu()
        {
            Console.WriteLine("Enter number to access menu");
            Console.WriteLine("1. Create new playlist");
            Console.WriteLine("2. Access playlists");
            Console.WriteLine("3. View all songs available");

            string optionString = Console.ReadLine();

            Console.Clear();

            switch (optionString)
            {
                case "1":
                    createPlaylist();
                    break;
                case "2":
                    showPlaylists();
                    break;
                case "3":
                    getSongs();
                    break;
                default:
                    MainMenu();
                    break;
            }
        }

        private static void showPlaylists()
        {
            int i = 1;
            foreach (KeyValuePair<string, Playlist> playlist in customer.Library)
            {
                Console.WriteLine($"{i}. {playlist.Key.Name}");
                i++;
            }
        }

        private static void createPlaylist()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            Playlist playlist = new Playlist(name);
            
            customer.Library.Add(playlist, new HashSet<string>());

            Console.Clear();
            MainMenu();
        }
    }
}
