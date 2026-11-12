namespace Castaway.Models;

/// <summary>
/// Every section of Castaway. Home has its own preview page (sample titles); everything
/// else uses the generic ComingSoonView built from this data.
/// Add a module here and it appears in the sidebar automatically.
/// </summary>
public static class ModuleCatalog
{
    public static readonly ModuleInfo Home = new()
    {
        Key = "home", Title = "Home", Glyph = "", IsComingSoon = false,
        Tagline = "What's new and what you were watching.",
    };

    public static readonly IReadOnlyList<ModuleInfo> Sidebar =
    [
        Home,
        new()
        {
            Key = "movies", Title = "Movies", Glyph = "", Section = "LIBRARIES", IsLibrary = true,
            Tagline = "Every movie on your Castaway server.",
            Description = "Castaway scans your movie folders on the server, matches each file with its poster, year, cast and description, and shows them here as a library you can browse, sort and play from anywhere.",
            PrimaryAction = "Add movie folder",
            Kpis = ["Movies", "Unwatched", "Added this week", "Library size"],
            Features =
            [
                "Posters, descriptions and cast matched automatically (TMDB)",
                "Sort by title, year, date added or rating",
                "Filter by genre, decade, resolution and watched",
                "Collections (all the films in a series together)",
                "Play, resume and mark as watched",
                "Trailers and extras",
            ],
        },
        new()
        {
            Key = "tv", Title = "TV Shows", Glyph = "", IsLibrary = true,
            Tagline = "Your shows, organized by season and episode.",
            Description = "Point Castaway at your TV folders and it sorts every episode into shows and seasons, with artwork and episode descriptions. Castaway remembers where you are in every show and lines up the next episode.",
            PrimaryAction = "Add TV folder",
            Kpis = ["Shows", "Episodes", "Unwatched episodes", "In progress"],
            Features =
            [
                "Shows, seasons and episodes with artwork",
                "Next episode lined up automatically",
                "Skip intro and skip credits",
                "Mark a season or a whole show as watched",
                "Specials and multi-part episodes",
                "Continue Watching on every device",
            ],
        },
        new()
        {
            Key = "kids", Title = "Kids & Family", Glyph = "", IsLibrary = true,
            Tagline = "Movies and shows for the whole family.",
            Description = "A library just for family-friendly movies and shows. Combined with kid profiles, the little ones only see what's in here.",
            PrimaryAction = "Add folder",
            Kpis = ["Titles", "Movies", "Shows", "Unwatched"],
            Features =
            [
                "Separate library for family titles",
                "Kid profiles that only see this library",
                "Content rating limits per profile",
                "Big, simple artwork view",
            ],
        },
        new()
        {
            Key = "documentaries", Title = "Documentaries", Glyph = "", IsLibrary = true,
            Tagline = "True stories, nature and history.",
            Description = "Keep documentary films and series in their own library, with descriptions and artwork matched just like movies and shows.",
            PrimaryAction = "Add folder",
            Kpis = ["Titles", "Films", "Series", "Unwatched"],
            Features =
            [
                "Documentary films and series together",
                "Topics like nature, history, science and true crime",
                "Artwork and descriptions matched automatically",
                "Continue Watching on every device",
            ],
        },
        new()
        {
            Key = "standup", Title = "Stand-Up", Glyph = "", IsLibrary = true,
            Tagline = "Comedy specials, all in one place.",
            Description = "A library for stand-up comedy specials, grouped by comedian so you can find every special from your favorites.",
            PrimaryAction = "Add folder",
            Kpis = ["Specials", "Comedians", "Unwatched", "Added this month"],
            Features =
            [
                "Browse by comedian",
                "Specials matched with artwork and year",
                "Shuffle play",
                "Mark as watched",
            ],
        },
        new()
        {
            Key = "anime", Title = "Anime", Glyph = "", IsLibrary = true,
            Tagline = "Series and films, subbed or dubbed.",
            Description = "Anime gets its own library with absolute episode numbering, season matching built for anime, and an easy switch between subtitles and dubbed audio.",
            PrimaryAction = "Add folder",
            Kpis = ["Series", "Episodes", "Films", "Unwatched"],
            Features =
            [
                "Matching built for anime episode numbering",
                "Remember sub or dub per series",
                "Japanese audio with your subtitle language",
                "Skip openings and endings",
                "Series and films together",
            ],
        },
        new()
        {
            Key = "server", Title = "Castaway Server", Glyph = "", Section = "SERVER",
            Tagline = "The server that holds and streams your media.",
            Description = "Castaway Server runs on your own PC, NAS or home server, by itself or in Docker. It scans your media folders, keeps the library, and streams to this app and your other devices. Everything stays on hardware you own and it's completely free.",
            PrimaryAction = "Connect to server",
            Kpis = ["Servers", "Libraries", "Storage used", "Status"],
            Columns = ["Server", "Address", "Version", "Libraries", "Status"],
            Features =
            [
                "Run on Windows, Linux or in a Docker container",
                "Find servers on your home network automatically",
                "Scan folders and watch them for new files",
                "Converts video on the fly when a device can't play it (FFmpeg)",
                "User accounts, with profiles for each person",
                "No subscriptions, no paid tiers",
            ],
        },
        new()
        {
            Key = "remote", Title = "Remote Access", Glyph = "",
            Tagline = "Watch your library away from home.",
            Description = "Stream from your Castaway server when you're out of the house, without paying for a relay. Castaway helps you set up a secure connection, and lowers the quality automatically on slow connections.",
            PrimaryAction = "Set up remote access",
            Kpis = ["Remote access", "Upload speed", "Remote streams", "Quality limit"],
            Columns = ["Method", "Address", "Secure", "Status"],
            Features =
            [
                "Tailscale, Cloudflare Tunnel or port forwarding",
                "HTTPS for every remote connection",
                "Quality limit for remote streams",
                "Share your library with family and friends",
                "Connection test from outside your network",
            ],
        },
        new()
        {
            Key = "activity", Title = "Activity", Glyph = "",
            Tagline = "Who's watching what, right now.",
            Description = "See every stream playing from your server: who's watching, on which device, and whether the video is playing directly or being converted. Plus a history of everything watched.",
            PrimaryAction = "View history",
            Kpis = ["Now playing", "Converting", "Watched today", "Bandwidth"],
            Columns = ["Title", "User", "Device", "Quality", "Progress"],
            Features =
            [
                "Live list of streams playing now",
                "Direct play or converting, at a glance",
                "Watch history per user",
                "Stop a stream from here",
            ],
        },
    ];

    public static ModuleInfo? Find(string key) => Sidebar.FirstOrDefault(m => m.Key == key);
}
