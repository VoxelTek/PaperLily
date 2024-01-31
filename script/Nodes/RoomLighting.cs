// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.RoomLighting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  [ExportType(icon = "room")]
  public class RoomLighting : Node, IAction, IInspectorCustomizer
  {
    [Export(PropertyHint.None, "")]
    public bool Default { get; set; }

    [Export(PropertyHint.None, "")]
    public Lighting Lighting { get; set; }

    public bool Current { get; private set; }

    public override void _Ready()
    {
      if (Engine.EditorHint)
      {
        this.SetChildrenVisible(false);
      }
      else
      {
        if (!this.Default)
          return;
        Game.CurrentRoom.Lighting = this.Lighting;
        this.SetChildrenVisible(true);
        this.Current = true;
      }
    }

    public void Execute() => this.Apply();

    public void Apply()
    {
      foreach (Node sibling in this.GetSiblings())
      {
        if (sibling is RoomLighting roomLighting)
          roomLighting.Unset();
      }
      this.SetChildrenVisible(true);
      Game.Room.ApplyLighting((Resource) this.Lighting);
      this.Current = true;
    }

    public void Unset()
    {
      if (!this.Current)
        return;
      this.SetChildrenVisible(false);
      Game.Room.ResetLighting();
      this.Current = false;
    }

    private void SetChildrenVisible(bool visible)
    {
      foreach (Node child in this.GetChildren())
      {
        if (child is CanvasItem canvasItem)
          canvasItem.Visible = visible;
      }
    }
  }
}
