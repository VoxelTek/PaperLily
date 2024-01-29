using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	[ExportType(icon = "light")]
	public class Lighting : Resource
	{
		[Export(PropertyHint.None, "")]
		public Texture ColorCorrection;

		[Export(PropertyHint.ColorNoAlpha, "")]
		public Color BackgroundColor = Colors.Black;

		[Export(PropertyHint.ColorNoAlpha, "")]
		public Color Modulate { get; set; } = Colors.White;


		[Export(PropertyHint.None, "")]
		public Material Material { get; set; }

		[Export(PropertyHint.None, "")]
		public bool UseNormals { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public PackedScene PlayerLightNode { get; set; }

		[Export(PropertyHint.None, "")]
		public bool PlayerLightState { get; set; }

		[Export(PropertyHint.Range, "0,10")]
		public float PlayerLightEnergy { get; set; } = 1f;


		[Export(PropertyHint.None, "")]
		public LightBlendMode PlayerLightMode { get; set; }

		[Export(PropertyHint.None, "")]
		public LightBlendMode EnvironmentLightMode { get; set; }

		[Export(PropertyHint.Range, "0,7")]
		public int GlowLevel { get; set; }

		public void Apply()
		{
			Game.Room.ApplyLighting(this);
		}

		public void ApplyToWorldEnvironment(WorldEnvironment world)
		{
			world.Environment.AdjustmentColorCorrection = ColorCorrection;
			world.Environment.GlowEnabled = GlowLevel > 0;
			world.Environment.GlowLevels__1 = GlowLevel >= 1;
			world.Environment.GlowLevels__2 = GlowLevel >= 2;
			world.Environment.GlowLevels__3 = GlowLevel >= 3;
			world.Environment.GlowLevels__4 = GlowLevel >= 4;
			world.Environment.GlowLevels__5 = GlowLevel >= 5;
			world.Environment.GlowLevels__6 = GlowLevel >= 6;
			world.Environment.GlowLevels__7 = GlowLevel >= 7;
		}
	}
}
