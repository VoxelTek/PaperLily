// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SelfDestructTimer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [ExportType(icon = "stopwatch")]
  public class SelfDestructTimer : Node
  {
    [Export(PropertyHint.None, "")]
    public float WaitTime { get; set; }

    public SelfDestructTimer()
    {
    }

    public SelfDestructTimer(float time) => this.WaitTime = time;

    public override void _Process(float delta)
    {
      this.WaitTime -= delta;
      if ((double) this.WaitTime > 0.0)
        return;
      this.GetParent().Delete();
    }
  }
}
