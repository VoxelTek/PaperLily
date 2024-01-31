// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.TimingBarSegment
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Minigames
{
  [ExportType]
  public class TimingBarSegment : Resource
  {
    [Export(PropertyHint.None, "")]
    public int Offset { get; private set; }

    [Export(PropertyHint.None, "")]
    public bool RandomOffset { get; private set; }

    [Export(PropertyHint.None, "")]
    public int Range { get; private set; }

    [Export(PropertyHint.None, "")]
    public float Speed { get; private set; }
  }
}
