// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ActionFade2D
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType]
  public class ActionFade2D : Godot.Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public NodePath Node { get; set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public bool FadeIn { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public float Duration { get; set; } = 0.5f;

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
      CanvasItem node = this.GetNode<CanvasItem>(this.Node);
      if (node == null)
        return;
      if (this.FadeIn)
        Game.Animations.Play((LacieAnimation) new FadeInAnimation(node, this.Duration));
      else
        Game.Animations.Play((LacieAnimation) new FadeOutAnimation(node, this.Duration));
    }
  }
}
