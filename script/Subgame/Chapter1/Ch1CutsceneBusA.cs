// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneBusA
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneBusA : GameRoom
  {
    [LacieEngine.API.GetNode("Background/seats_a5/lacie")]
    private Sprite nLacieSitting;
    [LacieEngine.API.GetNode("Background/seats_a5/lacie/Animation")]
    private AnimationPlayer nLacieScoochAnimation;
    [LacieEngine.API.GetNode("Background/curtains_a/Animation")]
    private AnimationPlayer nTicketAnimation;
    [LacieEngine.API.GetNode("BusMoveAnimation")]
    private AnimationPlayer nBusMoveAnimation;
    private NPCWalker nLacie;

    public override void _BeforeFadeIn()
    {
      this.nLacieSitting.Visible = false;
      this.nLacie = new NPCWalker("lacie", "up");
      this.nLacie.Position = this.GetPoint("enter");
      this.GetMainLayer().AddChild((Node) this.nLacie);
    }

    public void LacieScoochIn()
    {
      this.nLacie.Visible = false;
      this.nLacieSitting.Visible = true;
      this.nLacieScoochAnimation.PlayFirstAnimation();
    }

    public void PlayTicketAnimation() => this.nTicketAnimation.PlayFirstAnimation();

    public void PlayBusAnimation()
    {
      this.nBusMoveAnimation.GetAnimation("animation").TrackSetPath(0, (NodePath) ((string) Game.Camera.Node.GetPath() + ":offset"));
      this.nBusMoveAnimation.PlayFirstAnimation();
    }

    public void LacieCloseEyes() => this.nLacieSitting.Frame = 5;
  }
}
