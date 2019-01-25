using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSet
{
    class Program
    {
        private static List<Song> allSongs;
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
                allSongs = new List<Song>();
                int id = 1;

                while ((line = sr.ReadLine()) != null)
                {
                    Song song = new Song(id, "");
                    song.Name = line;

                    allSongs.Add(song);
                    id++;
                }
            }
        }

        public static void getSongs()
        {
            foreach (Song song in allSongs)
            {
                Console.WriteLine($"{song.Id}. {song.Name}");
            }

            addSong();
        }

        public static void addSong()
        {
            int songNum = 0;
            Song songToAdd = new Song(0, "");

            string songString = Console.ReadLine();

            try
            {
                songNum = Int32.Parse(songString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Please enter a number");
            }

            foreach (Song song in allSongs)
            {
                if (song.Id == songNum)
                {
                    songToAdd = song;
                }
            }

            if (customer.Library.Count != 0)
            {
                Console.Clear();
                Console.WriteLine($"Would you like to add {songToAdd.Name} to one of these playlists:");

                foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
                {
                    Console.WriteLine($"{playlist.Key}. {playlist.Value.Name}");
                }

                string playlistNumString = Console.ReadLine();
                int playlistNum = Int32.Parse(playlistNumString);

                foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
                {
                    if (playlistNum == playlist.Key)
                    {
                        playlist.Value.Songs.Add(songToAdd);
                        Console.Clear();
                        MainMenu();
                        break;
                    }

                }
            }
            else
            {
                Console.Clear();
                createPlaylist();
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
            Console.WriteLine("0. Back to main menu");
            foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
            {
                Console.WriteLine($"{i}. {playlist.Value.Name}");
                i++;
            }

            selectPlaylist();
        }

        private static void selectPlaylist()
        {
            string playlistNumString = Console.ReadLine();
            int playlistNum = Int32.Parse(playlistNumString);

            if (playlistNum == 0)
            {
                Console.Clear();
                MainMenu();
            }

            foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
            {
                if (playlistNum == playlist.Key)
                {
                    displayPlaylist(playlist.Value);
                }
                if (playlistNum < playlist.Key)
                {
                    Console.WriteLine("Error: playlist does not exist");
                    selectPlaylist();
                }
            }
        }

        private static void displayPlaylist(Playlist playlist)
        {
            Console.Clear();
            Console.WriteLine(playlist.Name.ToUpper());
            Console.WriteLine("0. Back");

            foreach (Song song in playlist.Songs)
            {
                Console.WriteLine($"{playlist.getSongIndex(song.Id)}. {song.Name}");

            }

            playlistGetUserChoice(playlist);
        }

        private static void playlistGetUserChoice(Playlist playlist)
        {
            string userChoice = Console.ReadLine();
            if (userChoice.Equals("0"))
            {
                Console.Clear();
                MainMenu();
            }
            else
            {
                try
                {
                    int songNum = Int32.Parse(userChoice);

                    foreach (Song song in playlist.Songs)
                    {
                        if (songNum == playlist.getSongIndex(song.Id))
                        {
                            Console.WriteLine($"Would you like to remove {song.Name} from {playlist.Name}?");
                            removeSongChoice(playlist, songNum);
                        }
                        if (songNum < song.Id)
                        {
                            Console.WriteLine("Error: Song is not in playlist");
                            selectPlaylist();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a whole number");
                }
            }
        }

        private static void removeSongChoice(Playlist playlist, int songNum)
        {
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    removeSongFromPlaylist(playlist, songNum);
                    break;
                default:
                    displayPlaylist(playlist);
                    break;
            }
        }

        private static void removeSongFromPlaylist(Playlist playlist, int songNum)
        {
            foreach (Song song in playlist.Songs)
            {
                if (songNum == playlist.getSongIndex(song.Id))
                {
                    playlist.Songs.Remove(song);
                    displayPlaylist(playlist);
                }
            }
        }

        private static void createPlaylist()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            Playlist playlist = new Playlist(customer.Library.Count + 1, name);
            
            customer.Library.Add(customer.Library.Count + 1, playlist);

            Console.Clear();
            MainMenu();
        }
    }
}
