using System;
using System.Collections.Generic;

namespace RSVTravelersMapForce;

/// <summary>Tile dimensions of Ridgeside Village's maps, read from its .tmx files.</summary>
internal static class RsvMapSizes
{
	/// <summary>Map name to (width, height) in tiles.</summary>
	internal static readonly IReadOnlyDictionary<string, (int W, int H)> Table
		= new Dictionary<string, (int, int)>
		{
			{ "AguarCaveFixed", (20, 30) },
			{ "AguarCaveFixed_alt", (20, 30) },
			{ "AguarWaterfalls", (1, 1) },
			{ "AlissaShedOpen", (16, 18) },
			{ "BusSVR2", (35, 30) },
			{ "BusSVR3", (35, 30) },
			{ "CliffHousePeek", (2, 2) },
			{ "Custom_Ridgeside_3Bros", (24, 20) },
			{ "Custom_Ridgeside_3Bros2ndFloor", (24, 22) },
			{ "Custom_Ridgeside_AguarBasement", (11, 12) },
			{ "Custom_Ridgeside_AguarCave", (20, 30) },
			{ "Custom_Ridgeside_AguarCaveFixed", (20, 30) },
			{ "Custom_Ridgeside_AguarCaveFixed_alt", (20, 30) },
			{ "Custom_Ridgeside_AguarLab", (30, 18) },
			{ "Custom_Ridgeside_AlissaHouse", (25, 15) },
			{ "Custom_Ridgeside_AlissaShed", (16, 18) },
			{ "Custom_Ridgeside_AmethyneCorp", (28, 27) },
			{ "Custom_Ridgeside_AmethyneJewelry", (32, 23) },
			{ "Custom_Ridgeside_AmethyneMine", (33, 20) },
			{ "Custom_Ridgeside_BertHouse", (21, 16) },
			{ "Custom_Ridgeside_BertHouse2ndFloor", (21, 16) },
			{ "Custom_Ridgeside_BlairHouse", (32, 16) },
			{ "Custom_Ridgeside_EmberNight", (80, 80) },
			{ "Custom_Ridgeside_EzekielHouse", (34, 23) },
			{ "Custom_Ridgeside_EzekielPic", (15, 13) },
			{ "Custom_Ridgeside_FayeHouse", (15, 21) },
			{ "Custom_Ridgeside_FreddieHouse", (17, 18) },
			{ "Custom_Ridgeside_IanHouse", (16, 20) },
			{ "Custom_Ridgeside_Island_S-June", (43, 60) },
			{ "Custom_Ridgeside_JericHouse", (24, 18) },
			{ "Custom_Ridgeside_KennethHouse", (19, 13) },
			{ "Custom_Ridgeside_KiarraPick", (15, 13) },
			{ "Custom_Ridgeside_LennyHouse", (29, 14) },
			{ "Custom_Ridgeside_LogCabinEventHall", (30, 32) },
			{ "Custom_Ridgeside_LogCabinEventHall_Ballroom", (30, 32) },
			{ "Custom_Ridgeside_LogCabinEventHall_Birthday", (30, 32) },
			{ "Custom_Ridgeside_LogCabinEventHall_NightParty", (30, 32) },
			{ "Custom_Ridgeside_LogCabinEventHall_WeddingReception", (30, 32) },
			{ "Custom_Ridgeside_LogCabinEventHall_WeddingReception_Temp", (30, 32) },
			{ "Custom_Ridgeside_LogCabinHotel2ndFloor", (75, 28) },
			{ "Custom_Ridgeside_LogCabinHotel3rdFloor", (26, 18) },
			{ "Custom_Ridgeside_LogCabinHotelLobby", (46, 18) },
			{ "Custom_Ridgeside_LolaShed", (19, 18) },
			{ "Custom_Ridgeside_MaddieHouse", (23, 18) },
			{ "Custom_Ridgeside_MysticFalls1", (29, 20) },
			{ "Custom_Ridgeside_MysticFalls2", (29, 20) },
			{ "Custom_Ridgeside_MysticFalls3", (29, 20) },
			{ "Custom_Ridgeside_OldRSVKids", (15, 13) },
			{ "Custom_Ridgeside_PaulaClinic", (23, 20) },
			{ "Custom_Ridgeside_PikaHouse", (22, 18) },
			{ "Custom_Ridgeside_PrincessVilleDate", (120, 60) },
			{ "Custom_Ridgeside_PurpleMansion", (60, 50) },
			{ "Custom_Ridgeside_PurpleMansion2ndFloor", (48, 29) },
			{ "Custom_Ridgeside_PurpleMansion_Ball", (60, 50) },
			{ "Custom_Ridgeside_RSVAbandonedHouse", (12, 12) },
			{ "Custom_Ridgeside_RSVCableCar", (35, 30) },
			{ "Custom_Ridgeside_RSVCliff", (108, 40) },
			{ "Custom_Ridgeside_RSVCliff_AlissaDate", (108, 40) },
			{ "Custom_Ridgeside_RSVCliff_FayeDate", (108, 40) },
			{ "Custom_Ridgeside_RSVCorineDate", (40, 25) },
			{ "Custom_Ridgeside_RSVEvacuationCenter", (30, 30) },
			{ "Custom_Ridgeside_RSVGathering", (170, 150) },
			{ "Custom_Ridgeside_RSVGreenhouse1", (50, 35) },
			{ "Custom_Ridgeside_RSVGreenhouse2", (20, 24) },
			{ "Custom_Ridgeside_RSVHiddenWarp", (12, 12) },
			{ "Custom_Ridgeside_RSVHiddenWarp2", (12, 12) },
			{ "Custom_Ridgeside_RSVJioShop", (2, 2) },
			{ "Custom_Ridgeside_RSVNinjaHouse", (17, 14) },
			{ "Custom_Ridgeside_RSVRoad", (55, 40) },
			{ "Custom_Ridgeside_RSVSewers", (60, 40) },
			{ "Custom_Ridgeside_RSVSkyGazing", (15, 13) },
			{ "Custom_Ridgeside_RSVSpiritRealm", (100, 100) },
			{ "Custom_Ridgeside_RSVSummitHouse", (23, 16) },
			{ "Custom_Ridgeside_RSVSummitHouseNew", (23, 16) },
			{ "Custom_Ridgeside_RSVSummitShed", (19, 17) },
			{ "Custom_Ridgeside_RSVTheHike", (80, 85) },
			{ "Custom_Ridgeside_RSVTheRide", (93, 40) },
			{ "Custom_Ridgeside_RSVTheRide_static", (29, 17) },
			{ "Custom_Ridgeside_RSVWestCliff", (40, 25) },
			{ "Custom_Ridgeside_Ridge", (110, 50) },
			{ "Custom_Ridgeside_RidgeFalls", (80, 61) },
			{ "Custom_Ridgeside_RidgeForest", (170, 150) },
			{ "Custom_Ridgeside_RidgePond", (30, 22) },
			{ "Custom_Ridgeside_Ridge_KennethDate_OFF", (32, 29) },
			{ "Custom_Ridgeside_Ridge_KennethDate_ON", (32, 29) },
			{ "Custom_Ridgeside_RidgesideVillage", (170, 150) },
			{ "Custom_Ridgeside_RidgesideVillage_CookOff", (170, 150) },
			{ "Custom_Ridgeside_RidgesideVillage_Event", (50, 50) },
			{ "Custom_Ridgeside_RidgesideVillage_Fashion", (50, 50) },
			{ "Custom_Ridgeside_ShiroHouse", (27, 15) },
			{ "Custom_Ridgeside_SummitFarm", (93, 80) },
			{ "Custom_Ridgeside_TortsRealm", (31, 40) },
			{ "Custom_Ridgeside_WeddingReceptionPic", (15, 13) },
			{ "Custom_Ridgeside_ZuzuCableCarView", (15, 13) },
			{ "FayeHouse_Irene", (13, 9) },
			{ "LockedNinjaHouse", (1, 2) },
			{ "LogCabinHotel2ndFloor_June", (8, 8) },
			{ "LogCabinHotelLobbyExpanded", (46, 18) },
			{ "PastryRandom1", (3, 2) },
			{ "PastryRandom2", (3, 2) },
			{ "PastryRandom3", (3, 2) },
			{ "Placeholder", (50, 12) },
			{ "PurpleMansion_NoIrene", (14, 8) },
			{ "PurpleMansion_OpenBath", (3, 4) },
			{ "RSVAbandonedHouse_Sched", (1, 2) },
			{ "RSVBedTile", (1, 2) },
			{ "RSVBetterSquare", (22, 22) },
			{ "RSVBurningSpiritFire", (1, 2) },
			{ "RSVBusStop", (35, 30) },
			{ "RSVBusStopNew", (35, 30) },
			{ "RSVCableCar_SewerEntrance", (5, 4) },
			{ "RSVChristmas", (130, 110) },
			{ "RSVChristmas2", (130, 110) },
			{ "RSVCleanPaulaRoom", (13, 11) },
			{ "RSVCliffNight", (108, 40) },
			{ "RSVCorruptedFire", (1, 2) },
			{ "RSVDaiaBook", (1, 2) },
			{ "RSVEggFestival", (130, 110) },
			{ "RSVEggFestival2", (130, 110) },
			{ "RSVFixedMinecart", (2, 3) },
			{ "RSVFlowerFestival", (100, 50) },
			{ "RSVFlowerFestival2", (100, 50) },
			{ "RSVGreenhouse2_Corrupt", (20, 24) },
			{ "RSVHalloween", (130, 110) },
			{ "RSVHalloween2", (130, 110) },
			{ "RSVJioShop", (2, 2) },
			{ "RSVLine", (50, 12) },
			{ "RSVLuau", (104, 50) },
			{ "RSVLuau2", (104, 50) },
			{ "RSVNinjaHouse_SOBoard", (3, 2) },
			{ "RSVOpenPlot", (3, 2) },
			{ "RSVPondEntrance", (9, 7) },
			{ "RSVSDVFair", (130, 110) },
			{ "RSVSDVFair2", (130, 110) },
			{ "RSVSouthPatchTemp", (28, 30) },
			{ "RSVSpousePatios", (16, 24) },
			{ "RSVSummitEntrance", (6, 3) },
			{ "RSVTheHike_Cart", (9, 5) },
			{ "RSVTheHike_Cart-Empty", (9, 5) },
			{ "RSVTheRideNight", (93, 40) },
			{ "RSVxSVE-Beach-Luau", (48, 26) },
			{ "RSVxSVE-FlowerFestival", (100, 50) },
			{ "RSVxSVE-Town-Christmas", (59, 49) },
			{ "RSVxSVE-Town-EggFestival", (52, 54) },
			{ "RSVxSVE-Town-Fair", (40, 37) },
			{ "RSVxSVE-Town-Fair2", (42, 40) },
			{ "RSVxSVE-Town-Halloween", (25, 24) },
			{ "RSVxSVE-Town-Halloween2", (39, 16) },
			{ "RSVxSVE-Town-Halloween3", (25, 24) },
			{ "RSVxSVE-Town-JojaChristmas.tbin", (59, 49) },
			{ "Richard8HeartsEffect", (1, 2) },
			{ "RidgeFallsOpenPortal", (3, 3) },
			{ "RidgeOpenBridge", (2, 5) },
			{ "RidgesideSpouseRooms", (30, 45) },
			{ "SummitFarm_HouseRedone", (10, 8) },
			{ "SummitFarm_OreArea", (13, 20) },
			{ "UndreyaUnlock", (1, 2) },
			{ "UnlockGH1", (13, 11) },
			{ "UnlockGH2", (7, 10) },
		};

	/// <summary>Per-map nudges, in tiles, for spots where the exact center is blocked.</summary>
	private static readonly Dictionary<string, (int X, int Y)> Nudges = new()
	{
		{ "Custom_Ridgeside_Ridge", (0, 4) },
	};

	/// <summary>Center tile of a map, falling back to the village center.</summary>
	internal static (int X, int Y) Center(string mapName)
	{
		if (!Table.TryGetValue(mapName, out (int W, int H) size))
			return Fallback;

		var (x, y) = (size.W / 2, size.H / 2);
		if (Nudges.TryGetValue(mapName, out (int X, int Y) nudge))
		{
			x += nudge.X;
			y += nudge.Y;
		}
		return (Math.Clamp(x, 1, Math.Max(1, size.W - 2)), Math.Clamp(y, 1, Math.Max(1, size.H - 2)));
	}

	/// <summary>Where to land when the target map is unknown or is a marker point.</summary>
	internal static (int X, int Y) Fallback { get; } = (85, 75);

	/// <summary>Whether a map is a real place worth warping to (skips markers and variants).</summary>
	internal static bool IsLandable(string mapName)
	{
		if (!Table.TryGetValue(mapName, out (int W, int H) size))
			return false;
		if (size.W < 8 || size.H < 8)
			return false;
		if (mapName.Contains("_OFF") || mapName.Contains("Event") || mapName.Contains("Empty"))
			return false;
		return true;
	}
}
