namespace RSVTravelersMapForce;

/// <summary>Configuration options for the Ridgeside force.</summary>
public sealed class ModConfig
{
	/// <summary>Whether to show the "Ridgeside" button on the vanilla world map.</summary>
	public bool Enabled { get; set; } = true;

	/// <summary>Gold cost per teleport to a Ridgeside area.</summary>
	public int Price { get; set; } = 1750;

	/// <summary>
	/// Buttons that open the Ridgeside map. Uses SMAPI keybind syntax, so several
	/// alternatives can be listed separated by commas (e.g. "R, DPadLeft").
	/// </summary>
	public string OpenMapKey { get; set; } = "R";

	/// <summary>
	/// Extra buttons that open Traveler's Map. Dipendor's own option is a single
	/// <c>SButton</c>, so this list is served by borrowing that key for the duration
	/// of the button press. Same comma-separated keybind syntax as
	/// <see cref="OpenMapKey"/>.
	/// </summary>
	public string TravelersMapKeys { get; set; } = "N, DPadRight";
}