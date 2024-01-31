// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneFacilityB2fHallwayEast
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneFacilityB2fHallwayEast : GameRoomNested
  {
    [LacieEngine.API.GetNode("Room/Background/Cutscene1")]
    private SimpleAnimatedSprite nCutscene1;
    [LacieEngine.API.GetNode("Room/Background/Cutscene2")]
    private SimpleAnimatedSprite nCutscene2;
    [LacieEngine.API.GetNode("Room/Background/Cutscene3")]
    private SimpleAnimatedSprite nCutscene3;
    [LacieEngine.API.GetNode("Room/Background/Cutscene4")]
    private SimpleAnimatedSprite nCutscene4;
    [LacieEngine.API.GetNode("Room/Background/SaiSmile")]
    private Node2D nSaiSmile;
    private static readonly Color SAI_TINT = new Color("#0a0a0a");

    public void CharactersInit()
    {
      Game.Room.RegisteredNPCs["sai"].Modulate = Ch1CutsceneFacilityB2fHallwayEast.SAI_TINT;
      Game.Room.RegisteredNPCs["lacie"].Visible = false;
    }

    public void LacieStopRubbingButt()
    {
      this.nCutscene1.Playing = false;
      this.nCutscene1.Frame = 2;
    }

    public void LacieLookLeft()
    {
      this.nCutscene2.Visible = false;
      this.nCutscene1.Visible = true;
      this.nCutscene1.Frame = 3;
    }

    public void LacieLookLeftShade() => this.nCutscene1.Frame = 4;

    public void SaiIlluminate()
    {
      Game.Animations.Play((LacieAnimation) new ModulateAnimation((CanvasItem) Game.Room.RegisteredNPCs["sai"], 0.5f, Godot.Colors.Black, Godot.Colors.White));
    }

    public void SaiSmile()
    {
      Game.Room.RegisteredNPCs["sai"].Visible = false;
      this.nSaiSmile.Visible = true;
    }

    public void SaiDontSmile()
    {
      Game.Room.RegisteredNPCs["sai"].Visible = true;
      this.nSaiSmile.Visible = false;
    }

    public void LacieThrowSalt()
    {
      this.nCutscene1.Visible = false;
      this.nCutscene2.Visible = true;
      this.nCutscene2.Playing = true;
    }

    public void LacieRealizeItWasDumb()
    {
      this.nCutscene2.Playing = false;
      this.nCutscene2.Frame = 14;
      Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) this.nCutscene2, 0.5f));
      this.nCutscene3.Visible = true;
    }

    public void SaiReachOut()
    {
      Game.Room.RegisteredNPCs["sai"].Visible = false;
      this.nCutscene2.Visible = false;
      this.nCutscene3.Visible = true;
      this.nCutscene3.Playing = true;
    }

    public void LacieStandUpByHerself()
    {
      this.nCutscene3.Visible = false;
      this.nCutscene4.Visible = true;
      this.nCutscene4.Playing = true;
    }

    public void LacieAndSaiReset()
    {
      Node2D registeredNpC1 = Game.Room.RegisteredNPCs["sai"];
      Node2D registeredNpC2 = Game.Room.RegisteredNPCs["lacie"];
      this.nCutscene4.Visible = false;
      registeredNpC1.Visible = true;
      registeredNpC2.Visible = true;
    }
  }
}
