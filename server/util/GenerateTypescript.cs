using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace server.Util {

public class TypescriptBackendGenerator {
	private readonly Dictionary<Type, string> typeCache = new();
	private readonly StreamWriter file;

	public TypescriptBackendGenerator(string file) {
		this.file = new StreamWriter(file);

		this.file.WriteLine($"// GENERATED FILE {DateTime.Now:dd-MM-yyyy HH\\:mm\\:ss}");
		this.file.WriteLine( "// This file is being generated by GenerateTypescript.cs\n");

		this.file.WriteLine($"// == Standard declarations ======================================");

		this.file.WriteLine("export var baseUrl = ''\n");

		this.file.WriteLine("function timeout(millis = 300): Promise<void> {");
		this.file.WriteLine("	return new Promise<void>(function(resolve) { setTimeout(resolve, millis); });");
		this.file.WriteLine("}\n");

		this.file.WriteLine(@"async function get (url: string): Promise<any> {");
		this.file.WriteLine(@"	return await fetch(baseUrl + url).then(v => v.json());");
		this.file.WriteLine(@"}");

		this.file.WriteLine(@"async function post(url: string, data: any): Promise<any> {");
		this.file.WriteLine(@"	return await fetch(baseUrl + url, {");
		this.file.WriteLine(@"		method: 'POST',");
		this.file.WriteLine(@"		headers: { 'Content-Type': 'application/json' },");
		this.file.WriteLine(@"		body: JSON.stringify(data)");
		this.file.WriteLine(@"	});");
		this.file.WriteLine(@"}");

		this.file.WriteLine(@"async function del (url: string): Promise<any> {");
		this.file.WriteLine(@"	return await fetch(baseUrl + url, { method: 'DELETE' });");
		this.file.WriteLine(@"}");

		this.file.WriteLine();
	}

	public TypescriptBackendGenerator AddEndpoint(Type type) {
		var rootRoute = (type.GetCustomAttributes(true).FirstOrDefault(v => v is RouteAttribute) as RouteAttribute)?.Template??"";
		rootRoute += "/";

		file.WriteLine($"// == Endpoint: {type.Name} '{rootRoute}' ======================================");

		var regex = new Regex(@"\$\{(.+)}", RegexOptions.Compiled);

		List<string> endpointDeclarations = new();
		foreach(var member in type.GetMethods(BindingFlags.Public | BindingFlags.Instance)) {
			var route = member.GetCustomAttribute<RouteAttribute>();
			var get  = member.GetCustomAttribute<HttpGetAttribute>();
			var post = member.GetCustomAttribute<HttpPostAttribute>();
			var del  = member.GetCustomAttribute<HttpDeleteAttribute>();

			if(route == null) continue;

			var resultType = GetOrDeclareType(member.ReturnType);
			var name = member.Name;

			var template = route.Template.Replace("{", "${");
			var arguments = string.Join(", ", regex.Matches(template).Select(m => m.Groups[1].Value + ": string"));

			if(get != null) {
				endpointDeclarations.Add(
					$"export function {Lowercase(name)}({arguments})"+
					$": Promise<{resultType}> " +
					$"{{ return get(`{rootRoute}{template}`); }}"
				);
			}
			else if(post != null) {
				var dataArgument = member.GetParameters().Last();
				var dataArgumentType = GetOrDeclareType(dataArgument.ParameterType);
				var dataArgumentName = dataArgument.Name;

				endpointDeclarations.Add(
					$"export function {Lowercase(name)}({arguments}, {dataArgumentName}: {dataArgumentType})" +
					$": Promise<{resultType}> " +
					$"{{ return post(`{rootRoute}{template}`, {dataArgumentName}); }}"
				);
			}
			else if(del != null) {
				endpointDeclarations.Add(
					$"export function {Lowercase(name)}({arguments})"+
					$": Promise<{resultType}> " +
					$"{{ return del(`{rootRoute}{template}`); }}"
				);
			}
		}

		foreach(var decl in endpointDeclarations)
			file.WriteLine(decl);

		file.Flush();

		return this;
	}

	private string GetOrDeclareType(Type type) {
		if(type == typeof(string))
			return "string";
		else if(type.IsAssignableTo(typeof(double)))
			return "number";
		else if(type == typeof(void))
			return "void";

		if(typeCache.TryGetValue(type, out string value))
			return value;

		if(type.IsArray) {
			return GenerateArray(type);
		}

		if(type.GetInterfaces().Contains(typeof(IEnumerable))) {
			return GetOrDeclareType(type.GetGenericArguments()[0]) + "[]";
		}

		if(type.IsClass) {

			return GenerateClass(type);
		}

		if(type.IsEnum) {
			return GenerateEnum(type);
		}

		return "any"; // Crazy type, don't care
	}

	private string GenerateArray(Type type) {
		var rank = string.Concat(Enumerable.Repeat("[]", type.GetArrayRank()));
		var baseType = GetOrDeclareType(type.GetElementType());
		if(baseType.Any(v => v == ' ' || v == '|'))
			return $"({baseType}){rank}";
		else
			return $"{baseType}{rank}";
	}

	private string GenerateClass(Type type) {
		List<string> lines = new();

		lines.Add($"export");
		lines.Add($"interface {type.Name} {{");
		foreach(var prop in type.GetProperties()) {
			lines.Add($"	{Lowercase(prop.Name)}: {GetOrDeclareType(prop.PropertyType)};");
		}
		lines.Add("}\n");

		foreach(var line in lines) {
			file.WriteLine(line);
		}

		typeCache[type] = type.Name;
		return type.Name;
	}

	private string GenerateEnum(Type type) {
		var values = string.Join('|',
			type.GetEnumNames()
			.Select(name => $"'{name}'")
		);

		file.WriteLine($"export type {type.Name} = {values}\n");
		typeCache[type] = type.Name;

		return type.Name;
	}

	private static string Lowercase(string name) {
		return name = char.ToLowerInvariant(name[0]) + name[1..];
	}
}

}
