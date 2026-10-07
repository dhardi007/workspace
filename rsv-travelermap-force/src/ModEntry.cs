using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;
using Rectangle = xTile.Dimensions.Rectangle;

namespace RSVTravelersMapForce;

/// <summary>Harmony patches that graft the Ridgeside button onto Traveler's Map.</summary>
[HarmonyPatch]
internal static class MapMenuPatches
{
	private static IEnumerable<MethodBase> TargetMethods()
	{
		Type? type = AccessTools.TypeByName("Dipendor.TravelersMap.MapMenu");
		if (type is null)
		{
			ModEntry.Log("Traveler's Map not found; patches will not apply.", LogLevel.Warn);
			yield break;
		}

		// DeclaredOnly: parchea solo los metodos de MapMenu, no los heredados de IClickableMenu
		// (parchear draw(SpriteBatch) en la base affectaria a todos los menus del juego).
		const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

		foreach (string name in new[] { "draw", "receiveLeftClick" })
		{
			MethodInfo[] found = type
				.GetMethods(flags)
				.Where(m => m.Name == name)
				.ToArray();

			if (found.Length == 0)
			{
				continue;
			}

			if (found.Length > 1)
				ModEntry.Log($"'{name}' has {found.Length} overloads; patching all of them.", LogLevel.Warn);

			foreach (MethodInfo m in found)
			{
				yield return m;
			}
		}
	}

	/// <summary>Draws the Ridgeside button after Traveler's Map draws itself.</summary>
	internal static void Postfix_draw(SpriteBatch b)
		=> ModEntry.Instance?.DrawRsvButton(b);

	private static bool ClickLogged;

	/// <summary>Returns false to swallow clicks that land on the Ridgeside button.</summary>
	internal static bool Prefix_receiveLeftClick(int x, int y)
	{
		if (!ClickLogged)
		{
			ClickLogged = true;
			ModEntry.Log($"[DEBUG] Prefix_receiveLeftClick fired at {x},{y}", LogLevel.Warn);
		}
		return ModEntry.Instance?.ClickRsvButton(x, y) != true;
	}
}

/// <summary>Entry point for the mod.</summary>
public sealed class ModEntry : Mod
{
	/// <summary>The active instance, used by the Harmony patches.</summary>
	internal static ModEntry? Instance { get; private set; }

	/// <summary>Logs a message before the mod instance exists.</summary>
	internal static void Log(string message, LogLevel level)
		=> Instance?.Monitor.Log(message, level);

	private static Microsoft.Xna.Framework.Rectangle MGR(Rectangle r)
		=> new(r.X, r.Y, r.Width, r.Height);

	private ModConfig Config { get; set; } = new();

	private List<RsvArea> Areas { get; set; } = new();

	private Texture2D? ButtonTexture;

	/// <summary>Ridgeside world map texture (211x144).</summary>
	public Texture2D? RsvMapTexture { get; private set; }

	/// <summary>Ridgeside world map texture for winter.</summary>
	public Texture2D? RsvMapTextureWinter { get; private set; }

	/// <summary>The areas parsed from Ridgeside's own data.</summary>
	public IReadOnlyList<RsvArea> GetAreas() => Areas;

	/// <summary>The configured teleport price in gold.</summary>
	public int GetPrice() => Math.Max(0, Config.Price);

	/// <inheritdoc />
	public override void Entry(IModHelper helper)
	{
		Instance = this;
		Config = ReadConfig();

		new Harmony($"RSVTravelersMapForce.{ModFolder.GetHashCode()}").PatchAll();

		helper.Events.Input.ButtonPressed += OnButtonPressed;

		LoadAssets(helper);
		LoadAreas();

		Monitor.Log(
			$"loaded. price={Config.Price}, areas={Areas.Count}, map={RsvMapTexture is not null}, hotkey='{Config.OpenMapKey}'",
			LogLevel.Info);
	}

	/// <summary>The folder this mod's DLL lives in.</summary>
	internal static string ModFolder
		=> Path.GetDirectoryName(typeof(ModEntry).Assembly.Location) ?? "";

	/// <summary>Opens the Ridgeside map with a key. Does not rely on Harmony.</summary>
	private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
	{
		if (!Config.Enabled)
			return;

		// OpenMapKey admite varias teclas separadas por coma ("R, DPadLeft"):
		// basta con que el boton pulsado coincida con alguna de ellas.
		if (!MatchesAnyHotkey(e.Button))
			return;

		if (!Context.IsWorldReady)
			return;

		// solo cuando no hay menu, o cuando el menu abierto es el de Traveler's Map
		string? active = Game1.activeClickableMenu?.GetType().FullName;
		if (Game1.activeClickableMenu is not null && active != "Dipendor.TravelersMap.MapMenu")
			return;

		if (RsvMapTexture is null)
		{
			Monitor.Log("RsvMapTexture is null; no se abre el mapa.", LogLevel.Warn);
			return;
		}

		Game1.activeClickableMenu = new RsvMapMenu(this, Game1.MasterPlayer);
	}

	/// <summary>
	/// Whether the given button matches any configured hotkey. Uses SMAPI keybind
	/// syntax, so alternatives are separated by commas (e.g. "R, DPadLeft").
	/// </summary>
	private bool MatchesAnyHotkey(SButton button)
	{
		string[] parts = (Config.OpenMapKey ?? "")
			.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

		return parts.Any(p => SButton.TryParse(p, out SButton parsed) && parsed == button);
	}

	private static ModConfig ReadConfig()
	{
		string file = Path.Combine(ModFolder, "config.json");
		if (!File.Exists(file))
			return new ModConfig();

		try
		{
			return JsonConvert.DeserializeObject<ModConfig>(File.ReadAllText(file)) ?? new ModConfig();
		}
		catch (Exception ex)
		{
			Log($"bad config.json: {ex.Message}", LogLevel.Warn);
			return new ModConfig();
		}
	}

	private void LoadAssets(IModHelper helper)
	{
		ButtonTexture = TryLoad(helper, "assets/button.png");
		RsvMapTexture = TryLoad(helper, "assets/RSVWorldMap.png");
		RsvMapTextureWinter = TryLoad(helper, "assets/RSVWorldMap_winter.png");
	}

	private static Texture2D? TryLoad(IModHelper helper, string key)
	{
		try
		{
			return helper.ModContent.Load<Texture2D>(key);
		}
		catch
		{
			return null;
		}
	}

	private void LoadAreas()
	{
		string? modDir = FindRsvModDir();
		if (modDir is null)
		{
			Monitor.Log("Ridgeside Village not found.", LogLevel.Warn);
			return;
		}

		string areasFile = Path.Combine(modDir, "assets", "RSVWorldMapAreas.json");
		if (!File.Exists(areasFile))
			return;

		try
		{
			Areas = RsvAreaParser.Parse(File.ReadAllText(areasFile));
		}
		catch (Exception ex)
		{
			Monitor.Log($"could not read Ridgeside areas: {ex.Message}", LogLevel.Warn);
		}
	}

	private static string? FindRsvModDir()
	{
		string mods = Path.GetFullPath(Path.Combine(ModFolder, ".."));
		if (!Directory.Exists(mods))
			return null;

		return Directory
			.EnumerateDirectories(mods, "*", SearchOption.AllDirectories)
			.FirstOrDefault(d => File.Exists(Path.Combine(d, "assets", "RSVWorldMapAreas.json")));
	}

	/// <summary>Draws the Ridgeside button over Traveler's Map.</summary>
	internal void DrawRsvButton(SpriteBatch b)
	{
		if (!Config.Enabled || ButtonTexture is null)
			return;

		if (Game1.activeClickableMenu?.GetType().FullName != "Dipendor.TravelersMap.MapMenu")
			return;

		b.Draw(ButtonTexture, MGR(GetButtonRect()), Color.White);
	}

	/// <summary>Returns true when the click landed on the Ridgeside button.</summary>
	internal bool ClickRsvButton(int x, int y)
	{
		if (!Config.Enabled || ButtonTexture is null)
			return false;

		Rectangle r = GetButtonRect();
		if (x < r.X || x > r.X + r.Width || y < r.Y || y > r.Y + r.Height)
			return false;

		if (RsvMapTexture is null)
		{
			Game1.addHUDMessage(new HUDMessage("Ridgeside Village is not installed."));
			return true;
		}

		Game1.playSound("danDrop");
		Game1.activeClickableMenu = new RsvMapMenu(this, Game1.MasterPlayer);
		return true;
	}

	/// <summary>Gets the on-screen bounds of the Ridgeside button.</summary>
	public Rectangle GetButtonRect()
	{
		int w = ButtonTexture?.Width ?? 210;
		int h = ButtonTexture?.Height ?? 58;
		Rectangle vp = Game1.uiViewport;
		return new Rectangle(vp.Width - w - 24, vp.Height - h - 24, w, h);
	}
}
