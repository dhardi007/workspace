using xTile.Dimensions;

namespace RSVTravelersMapForce;

/// <summary>A clickable teleport destination in Ridgeside Village.</summary>
public sealed class RsvArea
{
	/// <summary>The SMAPI map name this area warps to.</summary>
	public string MapName { get; set; } = "";

	/// <summary>Display name shown in the tooltip.</summary>
	public string DisplayName { get; set; } = "";

	/// <summary>Pixel bounds of the area on the Ridgeside world map.</summary>
	public Rectangle Bounds { get; set; }

	/// <summary>Tile X to warp to.</summary>
	public int TileX { get; set; }

	/// <summary>Tile Y to warp to.</summary>
	public int TileY { get; set; }
}
