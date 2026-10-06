using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = xTile.Dimensions.Rectangle;
using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;

namespace RSVTravelersMapForce;

/// <summary>The Ridgeside Village world map screen, opened from the Traveler's Map button.</summary>
internal sealed class RsvMapMenu : IClickableMenu
{
	private readonly ModEntry Mod;

	private readonly Farmer Player;

	private Texture2D Texture
		=> (Game1.currentSeason == "winter" && Mod.RsvMapTextureWinter is not null)
			? Mod.RsvMapTextureWinter
			: Mod.RsvMapTexture!;

	private Rectangle DrawRect;

	private static Microsoft.Xna.Framework.Rectangle MGR(Rectangle r)
		=> new(r.X, r.Y, r.Width, r.Height);

	private float Scale = 1f;

	private RsvArea? Hovered;

	/// <summary>Draws only the 3px outline of a rect, not a filled block.</summary>
	private static void DrawOutline(SpriteBatch b, Rectangle r)
	{
		const int T = 3;
		Color c = Color.White * 0.85f;
		b.Draw(White, new Microsoft.Xna.Framework.Rectangle(r.X, r.Y, r.Width, T), c);
		b.Draw(White, new Microsoft.Xna.Framework.Rectangle(r.X, r.Y + r.Height - T, r.Width, T), c);
		b.Draw(White, new Microsoft.Xna.Framework.Rectangle(r.X, r.Y, T, r.Height), c);
		b.Draw(White, new Microsoft.Xna.Framework.Rectangle(r.X + r.Width - T, r.Y, T, r.Height), c);
	}

	private static Texture2D? WhiteTex;

	private static Texture2D White
	{
		get
		{
			if (WhiteTex is null)
			{
				WhiteTex = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
				WhiteTex.SetData(new[] { Color.White });
			}
			return WhiteTex;
		}
	}

	/// <summary>Creates the menu.</summary>
	public RsvMapMenu(ModEntry mod, Farmer player)
	{
		this.Mod = mod;
		this.Player = player;
		Recalculate(MGR(Game1.uiViewport));
	}

	private void Recalculate(Microsoft.Xna.Framework.Rectangle bounds)
	{
		int w = Texture.Width;
		int h = Texture.Height;
		Scale = Math.Min(bounds.Width / (float)w, bounds.Height / (float)h) * 0.92f;
		int dw = (int)(w * Scale);
		int dh = (int)(h * Scale);
		DrawRect = new Rectangle((bounds.Width - dw) / 2, (bounds.Height - dh) / 2, dw, dh);
	}

	/// <inheritdoc />
	public override void draw(SpriteBatch b)
	{
		b.Draw(Texture, MGR(DrawRect), Color.White);

		if (Hovered is not null)
			DrawOutline(b, MapBounds(Hovered));

		if (Hovered is not null)
		{
			string label = $"{Hovered.DisplayName}   {Mod.GetPrice()}g";
			Vector2 pos = new(Game1.getMouseX() + 18, Game1.getMouseY() + 18);
			b.DrawString(Game1.smallFont, label, pos + new Vector2(1, 1), Color.Black * 0.85f);
			b.DrawString(Game1.smallFont, label, pos, Color.White);
		}

		drawMouse(b);
	}

	private Rectangle MapBounds(RsvArea area)
		=> new(
			DrawRect.X + (int)(area.Bounds.X * Scale),
			DrawRect.Y + (int)(area.Bounds.Y * Scale),
			Math.Max(4, (int)(area.Bounds.Width * Scale)),
			Math.Max(4, (int)(area.Bounds.Height * Scale)));

	/// <inheritdoc />
	public override void receiveLeftClick(int x, int y, bool playSound = true)
	{
		int price = Mod.GetPrice();
		RsvArea? hit = Find(x, y);

		if (hit is null)
			return;

		if (Player.Money < price)
		{
			Game1.addHUDMessage(new HUDMessage($"You need {price}g to travel there."));
			return;
		}

		Player.Money -= price;
		Game1.warpFarmer(hit.MapName, hit.TileX, hit.TileY, 2);
		exitThisMenu();
	}

	/// <inheritdoc />
	public override void update(GameTime time)
	{
		Hovered = Find(Game1.getMouseX(), Game1.getMouseY());
		base.update(time);
	}

	private RsvArea? Find(int x, int y)
	{
		if (x < DrawRect.X || x > DrawRect.X + DrawRect.Width || y < DrawRect.Y || y > DrawRect.Y + DrawRect.Height)
			return null;

		float mx = (x - DrawRect.X) / Scale;
		float my = (y - DrawRect.Y) / Scale;

		foreach (RsvArea a in Mod.GetAreas())
		{
			Rectangle b = a.Bounds;
			int bx = (int)mx;
			int by = (int)my;
			if (bx >= b.X && bx <= b.X + b.Width && by >= b.Y && by <= b.Y + b.Height)
				return a;
		}

		return null;
	}

	/// <inheritdoc />
	public override void receiveRightClick(int x, int y, bool playSound = true)
	{
		exitThisMenu();
	}

	/// <inheritdoc />
	public override void gameWindowSizeChanged(Microsoft.Xna.Framework.Rectangle oldBounds, Microsoft.Xna.Framework.Rectangle newBounds)
		=> Recalculate(newBounds);
}
