using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using xTile.Dimensions;

namespace RSVTravelersMapForce;

/// <summary>Parses Ridgeside Village's <c>RSVWorldMapAreas.json</c> into clickable areas.</summary>
internal static class RsvAreaParser
{
	private static readonly Regex Comment = new(@"//.*?$|/\*.*?\*/", RegexOptions.Multiline | RegexOptions.Singleline);
	private static readonly Regex TrailingComma = new(@",(\s*[}\]])");

	/// <summary>Destinations worth exposing, in the order they should appear.</summary>
	private static readonly string[] Preferred =
	{
		"RidgesideVillage", "Ridge", "RidgeFalls", "RSVCliff", "RSVTheHike",
		"LogCabinHotelLobby", "3Bros", "EzekielHouse", "MaddieHouse", "ShiroHouse",
		"LennyHouse", "FayeHouse", "PurpleMansion", "AguarLab", "BertHouse",
		"AlissaHouse", "PikaHouse", "BlairHouse", "FreddieHouse", "LolaShed",
		"IanHouse", "JericHouse", "PaulaClinic", "KennethHouse", "RSVTheHike",
		"RSVAbandonedHouse", "RSVGreenhouse1", "RSVGreenhouse2", "RSVNinjaHouse",
		"RSVCliff", "AguarLab",
	};

	/// <summary>Parsed areas, fitted to the Ridgeside world map texture.</summary>
	internal static List<RsvArea> Parse(string json)
	{
		string clean = TrailingComma.Replace(Comment.Replace(json, string.Empty), "$1");
		JObject root = JObject.Parse(clean);

		var raw = new List<(string Map, Rectangle Bounds)>();
		int maxX = 1;
		int maxY = 1;

		foreach (JObject entry in (JArray)root["AreaList"]!)
		{
			string map = entry["MapName"]?.Value<string>() ?? "";
			if (!RsvMapSizes.IsLandable(map))
				continue;

			var area = (JObject)entry["Area"]!;
			var rect = new Rectangle(
				area["X"]?.Value<int>() ?? 0,
				area["Y"]?.Value<int>() ?? 0,
				Math.Max(1, area["Width"]?.Value<int>() ?? 1),
				Math.Max(1, area["Height"]?.Value<int>() ?? 1));

			raw.Add((map, rect));
			maxX = Math.Max(maxX, rect.X + rect.Width);
			maxY = Math.Max(maxY, rect.Y + rect.Height);
		}

		// the area coordinates live in a larger space than the 211x144 map texture
		float scaleX = MapWidth / (float)maxX;
		float scaleY = MapHeight / (float)maxY;

		var areas = new List<RsvArea>();
		var seen = new HashSet<string>();

		foreach (string key in Preferred)
		{
			foreach (var (map, bounds) in raw)
			{
				if (!string.Equals(map, "Custom_Ridgeside_" + key, StringComparison.Ordinal))
					continue;
				if (!seen.Add(map))
					continue;

				// warp to the CENTER OF THE TARGET MAP in tiles; the rect only picks the map
				var (tileX, tileY) = RsvMapSizes.Center(map);

				areas.Add(new RsvArea
				{
					MapName = map,
					DisplayName = PrettyName(key),
					Bounds = new Rectangle(
						(int)(bounds.X * scaleX),
						(int)(bounds.Y * scaleY),
						Math.Max(6, (int)(bounds.Width * scaleX)),
						Math.Max(6, (int)(bounds.Height * scaleY))),
					TileX = tileX,
					TileY = tileY,
				});
			}
		}

		return areas;
	}

	/// <summary>Width of Ridgeside's world map texture.</summary>
	internal const int MapWidth = 211;

	/// <summary>Height of Ridgeside's world map texture.</summary>
	internal const int MapHeight = 144;

	private static string PrettyName(string id)
	{
		var parts = Regex.Matches(id, "[A-Z]+(?![a-z])|[A-Z][a-z]*|[0-9]+")
			.Select(m => m.Value)
			.Where(m => m.Length > 0)
			.ToList();

		if (parts.Count == 0)
			return id;

		string first = parts[0];
		if (first == "RSV" || first == "SVE")
			return first + " " + string.Join(" ", parts.Skip(1));

		return string.Join(" ", parts);
	}
}
