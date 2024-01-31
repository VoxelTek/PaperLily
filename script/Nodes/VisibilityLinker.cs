// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.VisibilityLinker
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  [ExportType(icon = "visible")]
  public class VisibilityLinker : Node2D
  {
    [Export(PropertyHint.None, "")]
    public bool Invert { get; set; }

    [Export(PropertyHint.None, "")]
    public NodePath[] Nodes { get; set; }

    public override void _Ready()
    {
      int num = (int) this.Connect("visibility_changed", (Object) this, "UpdateVisibility");
      this.UpdateVisibility();
    }

    public void UpdateVisibility()
    {
      for (int index = 0; index < ((IEnumerable<NodePath>) this.Nodes).Count<NodePath>(); ++index)
      {
        if (!this.Nodes[index].IsNullOrEmpty() && this.GetNode(this.Nodes[index]) is CanvasItem node)
        {
          int num = this.Invert ? (!this.Visible ? 1 : 0) : (this.Visible ? 1 : 0);
          node.Visible = num != 0;
        }
      }
    }
  }
}
