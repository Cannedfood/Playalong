using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
	[Route("/api/playalong/v1"), Route("/api/")]
	public class PlayalongApiV1 : Controller
	{
		private readonly ILogger<PlayalongApiV1> _logger;
		private readonly Database _database;

		public PlayalongApiV1(ILogger<PlayalongApiV1> logger, Database database)
		{
			_logger = logger;
			_database = database;
		}

		// Endpoints

		[Route("songs"), HttpGet]
		public IEnumerable<Song> GetSongs() {
			return _database.Songs;
		}

		[Route("songs/{id}"), HttpGet]
		public Song GetSong(string id) {
			return _database.Songs.FirstOrDefault(s => s.Id == id);
		}
		[Route("songs/{id}"), HttpPost]
		public void SaveSong(string id, [FromBody] Song song) {
			song.Id = id;

			for(int i = 0; i < _database.Songs.Count; i++) {
				if(_database.Songs[i].Id == id) {
					_database.Songs[i] = song;
					_ = _database.Save();
					return;
				}
			}
			_database.Songs.Add(song);
			_ = _database.Save();
		}
		[Route("songs/{id}"), HttpDelete]
		public void DeleteSong(string id) {
			for(int i = 0; i < _database.Songs.Count; i++) {
				if(_database.Songs[i].Id == id) {
					_database.Songs.RemoveAt(i);
					_ = _database.Save();
					return;
				}
			}
		}

		// Models

		public class Song {
			public string Id          { get; set; }
			public string Title       { get; set; }
			public string Band        { get; set; }
			public double Bpm         { get; set; }
			public double StartOffset { get; set; }
			public string Source      { get; set; }

			public List<SongSection> Sections { get; set; }
		};

		public class SongSection {
			public string Name  { get; set; }
			public double Start { get; set; }
			public double End   { get; set; }
		};
	}
}
