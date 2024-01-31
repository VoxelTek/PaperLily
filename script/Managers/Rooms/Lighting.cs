// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Lighting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  [ExportType(icon = "light")]
  public class Lighting : Resource
  {
    [Export(PropertyHint.None, "")]
    public Texture ColorCorrection;
    [Export(PropertyHint.ColorNoAlpha, "")]
    public Color BackgroundColor = Godot.Colors.Black;

    [Export(PropertyHint.ColorNoAlpha, "")]
    public Color Modulate { get; set; } = Godot.Colors.White;

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

    public void Apply() => Game.Room.ApplyLighting((Resource) this);

    public void ApplyToWorldEnvironment(WorldEnvironment world)
    {
      world.Environment.AdjustmentColorCorrection = this.ColorCorrection;
      world.Environment.GlowEnabled = this.GlowLevel > 0;
      world.Environment.GlowLevels__1 = this.GlowLevel >= 1;
      world.Environment.GlowLevels__2 = this.GlowLevel >= 2;
      world.Environment.GlowLevels__3 = this.GlowLevel >= 3;
      world.Environment.GlowLevels__4 = this.GlowLevel >= 4;
      world.Environment.GlowLevels__5 = this.GlowLevel >= 5;
      world.Environment.GlowLevels__6 = this.GlowLevel >= 6;
      world.Environment.GlowLevels__7 = this.GlowLevel >= 7;
    }
  }
}
