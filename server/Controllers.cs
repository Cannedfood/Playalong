﻿using System.Collections.Generic;
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
		public IEnumerable<Song> GetSongs(string band = null, string album = null) {
			return FilterSongs(band: band, album: album);
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

		[Route("bands"), HttpGet]
		public IEnumerable<string> Bands() {
			return _database.Songs.Select(song => song.Band).Distinct();
		}

		[Route("albums"), HttpGet]
		public IEnumerable<string> Albums(string band = null) {
			return FilterSongs(band: band).Select(song => song.Album).Distinct();
		}

		private IEnumerable<Song> FilterSongs(string band = null, string album = null) {
			IEnumerable<Song> songs = _database.Songs;
			if(band  != null) songs = songs.Where(song => song.Band == band);
			if(album != null) songs = songs.Where(song => song.Album == album);
			return songs;
		}

		// Models

		public class Song {
			public string Id              { get; set; }
			
			public string Title           { get; set; }
			public string Album           { get; set; }
			public string Band            { get; set; }

			public double Bpm             { get; set; } = 120;
			public double TimeSubdivision { get; set; } = 4;
			public double TimeCount       { get; set; } = 4;

			public double StartOffset     { get; set; }
			public string Source          { get; set; }

			public List<SongSection> Sections { get; set; }
		};

		public class SongSection {
			public string Name            { get; set; }

			public double Start           { get; set; }
			public double End             { get; set; }

			public double Bpm             { get; set; } = 120;
			public double TimeSubdivision { get; set; } = 4;
			public double TimeCount       { get; set; } = 4;
		};
	}
}
