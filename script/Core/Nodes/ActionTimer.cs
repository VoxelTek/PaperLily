// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ActionTimer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType]
  public class ActionTimer : Godot.Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public NodePath Node { get; set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public bool Start { get; set; } = true;

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      Game.Room.RegisteredRoomActions[this.Name] = (IAction) this;
    }

    public void Execute()
    {
      if (!this.Enabled || this.Node.IsNullOrEmpty())
        return;
      Godot.Node node = this.GetNode(this.Node);
      if (this.Start)
        this.StartTimer(node);
      else
        this.StopTimer(node);
    }

    private void StartTimer(Godot.Node node)
    {
      switch (node)
      {
        case Timer timer:
          timer.Start();
          break;
        case SimpleTimer simpleTimer:
          simpleTimer.Start();
          break;
      }
    }

    private void StopTimer(Godot.Node node)
    {
      switch (node)
      {
        case Timer timer:
          timer.Stop();
          break;
        case SimpleTimer simpleTimer:
          simpleTimer.Stop();
          break;
      }
    }
  }
}
