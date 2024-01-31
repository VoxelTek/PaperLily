// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.CreditsScreen3
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  public class CreditsScreen3 : SimpleScreen
  {
    [LacieEngine.API.GetNode("Container/Section1")]
    private Control nPart1;
    [LacieEngine.API.GetNode("Container/Section2")]
    private Control nPart2;
    [LacieEngine.API.GetNode("Container/Section3")]
    private Control nPart3;
    [LacieEngine.API.GetNode("Container/Section3/SeeYou")]
    private Control nPart4;

    public override void _Ready()
    {
      base._Ready();
      this.SetProcessInput(false);
      this.Timers();
    }

    private async void Timers()
    {
      CreditsScreen3 baseNode = this;
      await baseNode.DelaySeconds(4.5);
      baseNode.ShowCredits2();
      await baseNode.DelaySeconds(5.0);
      baseNode.ShowCredits3();
      await baseNode.DelaySeconds(4.0);
      baseNode.ShowCredits4();
      await baseNode.DelaySeconds(4.2);
      baseNode.ShowCredits5();
    }

    private async void ShowCredits2()
    {
      CreditsScreen3 baseNode = this;
      Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nPart1, 1f));
      await baseNode.DelaySeconds(1.0);
      baseNode.nPart2.Visible = true;
      Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) baseNode.nPart2, 1f));
    }

    private async void ShowCredits3()
    {
      CreditsScreen3 baseNode = this;
      Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nPart2, 1f));
      await baseNode.DelaySeconds(1.0);
      baseNode.nPart3.Visible = true;
      Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) baseNode.nPart3, 1f));
    }

    private void ShowCredits4()
    {
      this.nPart4.Visible = true;
      Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) this.nPart4, 1f));
    }

    private async void ShowCredits5()
    {
      CreditsScreen3 baseNode = this;
      Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nPart3, 2f));
      await baseNode.DelaySeconds(4.0);
      baseNode.GoToNextScreen();
    }

    public override void GoToNextScreen()
    {
      Game.State.Party.Clear();
      Game.State.Party.Add("sai");
      Game.State.Event = "ch2_intro_from_demo";
      Game.State.EventLabel = "START";
      Game.State.LocationStr = "system.locations.ch1.event.ending";
      Game.State.LocationImg = "ch2_maze";
      Game.InputProcessor = Inputs.Processor.Menu;
      SaveScreenMenuContainer screenMenuContainer = GDUtil.MakeNode<SaveScreenMenuContainer>("SaveContainer");
      screenMenuContainer.OnClose = (Action) (() => base.GoToNextScreen());
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) screenMenuContainer);
    }
  }
}
