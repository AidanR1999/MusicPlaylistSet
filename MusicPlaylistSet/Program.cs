using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
            //get all the stored songs and create a new customer
            storeSongs();
            customer = new Customer();

            //run the login and main menu methods
            login();
            MainMenu();


            Console.ReadKey();
        }

        /// <summary>
        /// gets all the song names from a text file and assigns them a song id
        /// </summary>
        public static void storeSongs()
        {
            //read the song names from the text file in the debug folder
            using (StreamReader sr = new StreamReader("songTitleDataSet.txt"))
            {
                //add first song to the dictionary of all songs
                string line;
                allSongs = new List<Song>();
                int id = 1;

                //loop the dictionary, added each song and increment the songs id by 1
                while ((line = sr.ReadLine()) != null)
                {
                    Song song = new Song(id, "");
                    song.Name = line;

                    allSongs.Add(song);
                    id++;
                }
            }
        }

        /// <summary>
        /// get all the songs stored in the dictionary along with its id
        /// </summary>
        public static void getSongs()
        {
            Console.WriteLine("0. Back");
            foreach (Song song in allSongs)
            {
                Console.WriteLine($"{song.Id}. {song.Name}");
            }

            getSongToAdd();
        }

        /// <summary>
        /// gets user input to determine which song they wish to select
        /// </summary>
        public static void getSongToAdd()
        {
            int songNum = 0;
            string songString = Console.ReadLine();

            try
            {
                songNum = Int32.Parse(songString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Please enter a number");
            }

            //returns user to the main menu
            if (songNum == 0) 
            {
                Console.Clear();
                MainMenu();
            }
            else if (songNum <= allSongs.Count)
            {
                foreach (Song song in allSongs)
                {
                    if (song.Id == songNum)
                    {
                        confirmSongToAdd(song);

                    }
                }
            }
            else
            {
                Console.WriteLine("Song does not exist in current library");
                getSongToAdd();
            }
        }

        /// <summary>
        /// gets confirmation from the user that the selected song is to be added to the selected playlist
        /// </summary>
        /// <param name="songToAdd"></param>
        public static void confirmSongToAdd(Song songToAdd)
        {
            if (customer.Library.Count != 0)
            {
                Console.Clear();
                Console.WriteLine($"Would you like to add {songToAdd.Name} to one of these playlists:");

                //loops through the users stored playlists and displays them for selection to add song too
                foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
                {
                    Console.WriteLine($"{playlist.Key}. {playlist.Value.Name}");
                }

                addSongToPlaylist(songToAdd);
            }
            else
            { 
                Console.Clear();
                createPlaylist();
            }
        }

        /// <summary>
        /// will add users selected song to the selected playlist
        /// </summary>
        /// <param name="songToAdd"></param>
        public static void addSongToPlaylist(Song songToAdd)
        {
            string playlistNumString = Console.ReadLine();
            int playlistNum = Int32.Parse(playlistNumString);

            foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
            {
                //checks the users selection matches a playlist in the users library
                if (playlistNum == playlist.Key)
                {
                    //check the number of songs in the playlist and the status of premium
                    if (playlist.Value.Songs.Count >= 100 && !customer.IsPremium)
                    {
                        //if user is not a premium member, notify user they have reached playlist capacity
                        Console.WriteLine("Playlist reached capacity. Please upgrade account.");
                        Console.WriteLine("Press any key to return to the Main Menu.");
                        Console.ReadKey();
                        Console.Clear();
                        MainMenu();
                    }

                    //adds song to the playlist(Hashset)
                    playlist.Value.Songs.Add(songToAdd);
                    Console.Clear();
                    MainMenu();
                    break;
                }
            }
        }

        /// <summary>
        /// runs a basic log in system for user interaction
        /// </summary>
        public static void login()
        {
            Console.WriteLine("Are you a premium member?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            string optionString = Console.ReadLine();

            //sets isPremium bool to true if user selects the related option at the log in menu
            if (optionString.Equals("1"))
                customer.IsPremium = true;

            Console.Clear();
        }

        /// <summary>
        /// runs a main menu system and runs corresponding method with user input
        /// </summary>
        public static void MainMenu()
        {
            Console.WriteLine("Enter number to access menu");
            Console.WriteLine("1. Create new playlist");
            Console.WriteLine("2. Access playlists");
            Console.WriteLine("3. View all songs available");

            string optionString = Console.ReadLine();

            Console.Clear();

            //switch statement containing user options
            switch (optionString)
            {
                case "1":
                    createPlaylist();
                    break;
                case "2":
                    showPlaylists();
                    selectPlaylist();
                    break;
                case "3":
                    getSongs();
                    break;
                default:
                    MainMenu();
                    break;
            }
        }

        /// <summary>
        /// displays a users collection of playlists with access to the premium features
        /// this will only show if the user selects that they are a premium user at the main menu
        /// </summary>
        private static void showPlaylists()
        {
            int i = 1;
            Console.WriteLine("0. Back to main menu");
            foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
            {
                Console.WriteLine($"{i}. {playlist.Value.Name}");
                i++;
            }
            if (customer.IsPremium)
            {
                Console.WriteLine($"{i}. Access Premium Features");
            }
        }

        /// <summary>
        /// shows the users selection of playlists without the premium features
        /// </summary>
        private static void showPlaylistsNoExtras()
        {
            int i = 1;
            Console.WriteLine("0. Back to main menu");
            foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
            {
                Console.WriteLine($"{i}. {playlist.Value.Name}");
                i++;
            }
        }

        /// <summary>
        /// gets user input to select a playlist from the users collection
        /// </summary>
        private static void selectPlaylist()
        {
            string playlistNumString = Console.ReadLine();
            int playlistNum = Int32.Parse(playlistNumString);

            if (playlistNum == 0)
            {
                Console.Clear();
                MainMenu();
            }
            //gives premium users access to more powerful playlist tools
            if (playlistNum == customer.Library.Count + 1 && customer.IsPremium)
            {
                Console.Clear();
                displayPlaylistExtraOptions();

                switch (getUserChoiceExtraPlaylistOption())
                {
                    //exit to menu
                    case 0:
                        Console.Clear();
                        showPlaylists();
                        selectPlaylist();
                        break;
                    //unionise
                    case 1:
                        Console.Clear();
                        showPlaylistsNoExtras();
                        Console.WriteLine("Select 2 playlists to unionise");
                        createPlaylist(unionisePlaylists(getPlaylist(), getPlaylist()));
                        break;
                    //differentiate
                    case 2:
                        Console.Clear();
                        showPlaylistsNoExtras();
                        Console.WriteLine("Select 2 playlists to intersect");
                        //createPlaylist(playlistCompliment(getPlaylist(), getPlaylist()));
                        break;
                    //intersect
                    case 3:
                        Console.Clear();
                        showPlaylistsNoExtras();
                        Console.WriteLine("Select 2 playlists to intersect");
                        createPlaylist(intersectPlaylists(getPlaylist(), getPlaylist()));
                        break;
                    //subset of
                    case 4:
                        Console.Clear();
                        showPlaylistsNoExtras();
                        Console.WriteLine("Select subset playlist first, then superset");
                        Console.WriteLine(isSubSet(getPlaylist(), getPlaylist()));
                        break;
                    //superset of
                    case 5:
                        Console.Clear();
                        showPlaylistsNoExtras();
                        Console.WriteLine("Select superset playlist first, then subset");
                        Console.WriteLine(isSuperSet(getPlaylist(), getPlaylist()));
                        break;
                    default:
                        Console.WriteLine("Error: Enter one of the numbers available");
                        break;
                }
            }

            //displays the songs the playlist contains
            foreach (KeyValuePair<int, Playlist> playlist in customer.Library)
            {
                if (playlistNum == playlist.Key)
                {
                    displaySongsInPlaylist(playlist.Value);
                    
                }
                if (playlistNum < playlist.Key)
                {
                    Console.WriteLine("Error: playlist does not exist");
                    selectPlaylist();
                }
            }
        }

        /// <summary>
        /// returns the hashset playlist the user has selected from the menu
        /// </summary>
        /// <returns>selected playlist</returns>
        private static Playlist getPlaylist()
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
                    return playlist.Value;

                }
                //will display an error message if the user has entered a selection that does not exist
                if (playlistNum < playlist.Key)
                {
                    Console.WriteLine("Error: playlist does not exist");
                    selectPlaylist();
                }
            }

            return null;
        }

        /// <summary>
        /// prompts the user to select one of the premium playlist options
        /// </summary>
        /// <returns>user input</returns>
        private static int getUserChoiceExtraPlaylistOption()
        {
            string userChoiceStr = Console.ReadLine();

            try
            {
                int userChoice = Int32.Parse(userChoiceStr);

                return userChoice;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Enter a whole number");
                getUserChoiceExtraPlaylistOption();
            }

            return 0;
        }

        /// <summary>
        /// displays the premium playlist options
        /// </summary>
        private static void displayPlaylistExtraOptions()
        {
            Console.WriteLine("0. Back");
            Console.WriteLine("1. Unionise Playlists ");
            Console.WriteLine("2. Differentiate Playlists ");
            Console.WriteLine("3. Intersect Playlists ");
            Console.WriteLine("4. Check if subset of");
            Console.WriteLine("5. Check if superset of");

        }

        /// <summary>
        /// displays all the songs that are stored in the users selected playlist
        /// </summary>
        /// <param name="playlist"></param>
        private static void displaySongsInPlaylist(Playlist playlist)
        {
            Console.Clear();
            Console.WriteLine(playlist.Name.ToUpper());
            Console.WriteLine("0. Back");

            foreach (Song song in playlist.Songs)
            {
                Console.WriteLine($"{playlist.setSongIndex(song.Id)}. {song.Name}");

            }

            playlistGetSongChoice(playlist);
        }

        /// <summary>
        /// allows the user to remove the corresponding song from the playlist
        /// </summary>
        /// <param name="playlist"></param>
        private static void playlistGetSongChoice(Playlist playlist)
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
                        if (songNum == playlist.getSongIndex(song))
                        {
                            Console.WriteLine($"Would you like to remove {song.Name} from {playlist.Name}?");

                            removeSongChoice(playlist, songNum);
                            Console.Clear();
                            showPlaylists();
                            selectPlaylist();
                            break;
                        }
                        else if (songNum > song.Id)
                        {
                            Console.WriteLine("Error: Song is not in playlist");
                            selectPlaylist();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a whole number");
                    playlistGetSongChoice(playlist);
                }
            }
        }

        /// <summary>
        /// a confirmation page for song deletion in case of accidental menu selection
        /// </summary>
        /// <param name="playlist"></param>
        /// <param name="songNum"></param>
        private static void removeSongChoice(Playlist playlist, int songNum)
        {
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    playlist.removeSongFromPlaylist(songNum);
                    break;
                default:
                    displaySongsInPlaylist(playlist);
                    break;
            }
        }

        /// <summary>
        /// method creates an empty playlist in the users collection of playlists
        /// will later allow songs to be added
        /// </summary>
        private static void createPlaylist()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            Playlist playlist = new Playlist(customer.Library.Count + 1, name);

            //adds the playlist to the users library of playlists
            customer.Library.Add(customer.Library.Count + 1, playlist);

            Console.Clear();
            MainMenu();
        }

        /// <summary>
        /// creates a playlist already populated with songs
        /// </summary>
        /// <param name="playlist"></param>
        private static void createPlaylist(Playlist playlist)
        {
            Console.WriteLine("Name: ");
            playlist.Name = Console.ReadLine();

            customer.Library.Add(customer.Library.Count + 1, playlist);
            

            Console.Clear();
            MainMenu();
        }

        /// <summary>
        /// Finds all songs two inputted Playlists have in common and returns new Playlist with only matching songs.
        /// </summary>
        /// <param name="playlist1">Playlist</param>
        /// <param name="playlist2">Playlist</param>
        /// <returns>Playlist intersecting</returns>
        public static Playlist intersectPlaylists(Playlist playlist1, Playlist playlist2)
        {
            //Local Variables.
            int i = 1;
            Playlist intersecting = new Playlist();
            HashSet<string> a = new HashSet<string>();
            HashSet<string> b = new HashSet<string>();

            //Adding each Songs name from parameter variable playlist1 into Hashset of strings.
            foreach (Song s in playlist1.Songs)
            {
                a.Add(s.Name);
            }

            //Adding each Songs name from parameter variable playlist2 into Hashset of strings.
            foreach (Song s in playlist2.Songs)
            {
                b.Add(s.Name);
            }

            //Refactors local variable a to contain only song names (Strings) that are present in both local Variables a and b.
            a.IntersectWith(b);

            //Loops through every song name (String) in Hashset a.
            foreach (string s in a)
            {
                //Adds Songs to the HashSet<Songs> in local Variable intersecting using i as Id, and s as Name.
                intersecting.Songs.Add(new Song(i, s));

                //Increases value of i for next iteration.
                i++;
            }
            //Returns local variable intersecting of type Playlist.
            return intersecting;
        }

        /// <summary>
        /// Merges two playlists while deleting duplicates
        /// </summary>
        /// <param name="playlist1"></param>
        /// <param name="playlist2"></param>
        /// <returns> Playlist unionReturn </returns>
        public static Playlist unionisePlaylists(Playlist playlist1, Playlist playlist2)
        {
            int i = 0;
            // Create a hashset to store the unionised playlist & set it to hold the first playlist
            HashSet<Song> union = new HashSet<Song>(playlist1.Songs);

            // Merges the union playlist with playlist2 while deleting duplicates
            union.UnionWith(playlist2.Songs);

            // Create a new hashset of strings to hold the song names from the union playlist
            HashSet<string> a = new HashSet<String>();
            foreach (Song s in union)
            {
                a.Add(s.Name);
            }

            // Create an instance of the playlist class
            Playlist unionReturn = new Playlist();

            // Loop that adds the songs to the playlist 
            foreach (string s in a)
            {
                unionReturn.Songs.Add(new Song(i, s));
                i++;
            }
            // return the merged playlist
            return unionReturn;
        }

        public static bool isSuperSet(Playlist playlist1, Playlist playlist2)
        {
            HashSet<string> a = new HashSet<string>();
            foreach (Song s in playlist1.Songs)
            {
                a.Add(s.Name);
            }

            HashSet<string> b = new HashSet<string>();
            foreach (Song s in playlist2.Songs)
            {
                b.Add(s.Name);
            }

            return a.IsSupersetOf(b);
        }

        public static bool isSubSet(Playlist playlist1, Playlist playlist2)
        {
            HashSet<string> a = new HashSet<string>();
            foreach (Song s in playlist1.Songs)
            {
                a.Add(s.Name);
            }

            HashSet<string> b = new HashSet<string>();
            foreach (Song s in playlist2.Songs)
            {
                b.Add(s.Name);
            }


            return a.IsSubsetOf(b);
        }

        private static void playlistCompliment(Playlist Playlist1, Playlist Playlist2)
        {
            HashSet<string> Playlist1Songs = new HashSet<string>();
            HashSet<string> Playlist2Songs = new HashSet<string>();

            foreach (Song s in Playlist1.Songs)
            {
                Playlist1Songs.Add(s.Name);
            }

            foreach (Song s in Playlist2.Songs)
            {
                Playlist2Songs.Add(s.Name);
            }

            HashSet<string> UniquetoPL = new HashSet<string>();


            //Finds Songs in setA which compliment SetB 
            UniquetoPL = new HashSet<string>(Playlist1Songs.Except(Playlist2Songs));

            Console.WriteLine("These songs are unique to the first playlist: ");

            Console.Write("{");
            foreach (string s in UniquetoPL)
            {
                Console.Write(" {0},", s);
            }
            Console.WriteLine(" }");

            //Finds Songs in setB which compliment SetA 
            UniquetoPL = new HashSet<string>(Playlist2Songs.Except(Playlist1Songs));

            Console.WriteLine("These songs are unique to the second playlist: ");
            Console.Write("{");
            foreach (string s in UniquetoPL)
            {
                Console.Write(" {0},", s);
            }
            Console.WriteLine(" }");
        }
    }
}
