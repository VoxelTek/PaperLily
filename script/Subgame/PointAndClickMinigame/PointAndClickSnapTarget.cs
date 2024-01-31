// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.PointAndClickSnapTarget
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Minigames
{
  public class PointAndClickSnapTarget : TextureRect
  {
    [Export(PropertyHint.None, "")]
    public bool FirstTarget { get; set; }

    [Export(PropertyHint.None, "")]
    public Vector2 SnapOffset { get; set; }

    [Export(PropertyHint.None, "")]
    public string Left { get; set; }

    [Export(PropertyHint.None, "")]
    public string Up { get; set; }

    [Export(PropertyHint.None, "")]
    public string Right { get; set; }

    [Export(PropertyHint.None, "")]
    public string Down { get; set; }
  }
}
