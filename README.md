# Castaway

A free, self-hosted media server and player, in the spirit of Plex and Jellyfin.

Run it on your own server (or in Docker), point it at your movie and TV folders, and stream your library from anywhere: at home or remotely.

## Status

Early design stage. The first UI mockup lives in [`design/library-mockup.html`](design/library-mockup.html). Open it in any browser to see the planned library view.

## Planned features

- **Library scanning:** watches your media folders and builds a library of movies and shows
- **Metadata:** posters, descriptions and years pulled from a free movie database (TMDB)
- **Streaming:** direct play when the device supports the file, with on-the-fly conversion through FFmpeg when it doesn't
- **Web app:** a Plex-style interface that works in any browser, on desktop, phone or TV
- **Remote access:** watch outside your home over Tailscale, a Cloudflare Tunnel, or port forwarding with HTTPS
- **Docker:** one container, started with `docker compose up`

## Libraries

Home, Movies, TV Shows, Kids & Family, Documentaries, Stand-Up, Anime

## License

To be decided.
