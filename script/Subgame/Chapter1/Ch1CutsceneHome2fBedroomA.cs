// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneHome2fBedroomA
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneHome2fBedroomA : GameRoom
  {
    [LacieEngine.API.GetNode("Room")]
    private GameRoom nRoom;
    [LacieEngine.API.GetNode("Room/Background/furniture_bed/LacieLiedown")]
    private Sprite nLacieLiedown;
    [LacieEngine.API.GetNode("Room/Main/furniture_table_lacie/MiscLetter")]
    private Node2D nGoldenLetter;
    [LacieEngine.API.GetNode("Room/Main/LacieLetter")]
    private Node2D nLacieLetter;

    public override void _UpdateRoom() => this.nRoom._UpdateRoom();

    public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

    public void LacieLieDown()
    {
      Game.Room.RegisteredNPCs["lacie"].Visible = false;
      this.nLacieLiedown.Visible = true;
    }

    public void LacieGetUp()
    {
      this.nLacieLiedown.Visible = false;
      Game.Room.RegisteredNPCs["lacie"].Visible = true;
    }

    public void PickUpLetter()
    {
      this.nGoldenLetter.Visible = false;
      this.nLacieLetter.Visible = true;
      Game.Room.RegisteredNPCs["lacie"].Visible = false;
    }

    public void PutAwayLetter()
    {
      this.nLacieLetter.Visible = false;
      Game.Room.RegisteredNPCs["lacie"].Visible = true;
    }

    public void LacieBedCloseEyes() => this.nLacieLiedown.Frame = 0;

    public void LacieBedOpenEyes() => this.nLacieLiedown.Frame = 1;
  }
}
