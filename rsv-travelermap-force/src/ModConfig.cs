namespace RSVTravelersMapForce;

/// <summary>Configuration options for the Ridgeside force.</summary>
public sealed class ModConfig
{
	/// <summary>Whether to show the "Ridgeside" button on the vanilla world map.</summary>
	public bool Enabled { get; set; } = true;

	/// <summary>Gold cost per teleport to a Ridgeside area.</summary>
	public int Price { get; set; } = 1750;

	/// <summary>Key that opens the Ridgeside map.</summary>
	public string OpenMapKey { get; set; } = "R";
}
