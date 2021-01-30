using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
	[Route("/api/playalong/v1"), Route("/api/")]
	public class PlayalongApiV1 : Controller
	{
		private readonly ILogger<PlayalongApiV1> _logger;

		public PlayalongApiV1(ILogger<PlayalongApiV1> logger)
		{
			_logger = logger;
		}

		// Endpoints

		[Route("songs"), HttpGet]
		public Song[] GetSongs() {
			return new Song[]{ GetSong("heyho"), GetSong("heyho"), GetSong("heyho"), };
		}

		[Route("songs/{id}"), HttpGet]
		public Song GetSong(string id) {
			return new Song {
				Id = id,
				Title = id,
				Band = id,
			};
		}
		[Route("songs/{id}"), HttpPost]
		public void SaveSong(string id, Song song) {

		}
		[Route("songs/{id}"), HttpDelete]
		public void DeleteSong(string id) {

		}

		[Route("songs/{id}/sections"), HttpGet]
		public SongSection[] GetSongSections(string id) {
			return new SongSection[]{
				new SongSection { Name = "Intro", Start = 0, End = 12, },
				new SongSection { Name = "Intro", Start = 0, End = 12, },
				new SongSection { Name = "Intro", Start = 0, End = 12, },
				new SongSection { Name = "Intro", Start = 0, End = 12, },
			};
		}
		[Route("songs/{id}/sections"), HttpPost]
		public void SaveSongSections(string id, SongSection[] songSections) {

		}

		// Models

		public class Song {
			public string Id          { get; set; }
			public string Title       { get; set; }
			public string Band        { get; set; }
			public double Bpm         { get; set; }
			public double StartOffset { get; set; }
			public string Source      { get; set; }
		};

		public class SongSection {
			public string Name  { get; set; }
			public double Start { get; set; }
			public double End   { get; set; }
		};
	}
}
