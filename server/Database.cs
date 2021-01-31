using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using static server.Controllers.PlayalongApiV1;

namespace server {

public class Database {
	public List<Song> Songs = new();

	public Database() {
		Load().Wait();
	}
	~Database() {
		Save().Wait();
	}

	public async Task Load() {
		try {
			using var stream = System.IO.File.OpenRead("./songs.json");
			Songs = await JsonSerializer.DeserializeAsync<List<Song>>(stream, options: JsonOptions);
		}
		catch(Exception e) {
			Console.WriteLine(e.StackTrace);
		}
	}
	public async Task Save() {
		using var stream = System.IO.File.Create("./songs.json");
		await JsonSerializer.SerializeAsync(stream, Songs, options: JsonOptions);
	}

	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNameCaseInsensitive = true,
	};
};

}
