using System;
using System.IO;
using System.Linq;

namespace server.Util {

public static class TypescriptGenerator {
	public static void GenerateTypescriptDefinition(
		this Type type,
		StreamWriter file,
		string indention = "\t"
	)
	{
		file.WriteLine("export");
		file.WriteLine($"interface {type.Name} {{");
		foreach(var prop in type.GetProperties()) {
			file.WriteLine($"{indention}{Lowercase(prop.Name)}: {TypescriptType(prop.PropertyType)};");
		}
		file.WriteLine("}");
		file.WriteLine("");
	}

	private static string Lowercase(string name) {
		return name = char.ToLowerInvariant(name[0]) + name[1..];
	}

	private static string TypescriptType(Type type) {
		if(type == typeof(string)) {
			return "string";
		}
		else if(type.IsEnum) {
			return string.Join('|', type.GetEnumValues().Cast<Object>().Select(v => $"'{v}'"));
		}
		else if(type.IsAssignableTo(typeof(double))) {
			return "number";
		}
		return type.Name;
	}
};

}
