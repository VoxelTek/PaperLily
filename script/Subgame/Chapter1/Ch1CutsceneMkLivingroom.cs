// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneMkLivingroom
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
  public class Ch1CutsceneMkLivingroom : GameRoom
  {
    [LacieEngine.API.GetNode("Room")]
    private GameRoom nRoom;
    [LacieEngine.API.GetNode("Room/Main/LacieSit")]
    private Node2D nLacieSit;
    [LacieEngine.API.GetNode("Room/Main/SaiSit")]
    private Node2D nSaiSit;
    [LacieEngine.API.GetNode("Room/Main/MkAnxious")]
    private SimpleAnimatedSprite nMkAnxious;
    [LacieEngine.API.GetNode("Room/Main/MkConfused")]
    private Node2D nMkConfused;
    [LacieEngine.API.GetNode("Room/Main/MkFireoff")]
    private SimpleAnimatedSprite nMkFireoff;
    [LacieEngine.API.GetNode("Room/Main/SaiLessChill")]
    private Node2D nSaiLessChill;
    [LacieEngine.API.GetNode("Animation")]
    private AnimationPlayer nAnimation;

    public override void _UpdateRoom() => this.nRoom._UpdateRoom();

    public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

    public override Node FindNodeInRoom(string nodeName) => this.nRoom.FindNodeInRoom(nodeName);

    public void SaiSitDown()
    {
      Game.Room.RegisteredNPCs["sai"].Visible = false;
      this.nSaiSit.Visible = true;
    }

    public void LacieSitDown()
    {
      Game.Room.RegisteredNPCs["lacie"].Visible = false;
      this.nLacieSit.Visible = true;
    }

    public void BothStandUp()
    {
      this.nLacieSit.Visible = false;
      this.nSaiSit.Visible = false;
      Game.Room.RegisteredNPCs["lacie"].Visible = true;
      Game.Room.RegisteredNPCs["sai"].Visible = true;
    }

    public void CagesFall() => this.nAnimation.PlayFirstAnimation();

    public void MkConfused()
    {
      Game.Room.RegisteredNPCs["mk"].Visible = false;
      this.nMkConfused.Visible = true;
    }

    public void HideMkConfused()
    {
      this.nMkConfused.Visible = false;
      Game.Room.RegisteredNPCs["mk"].Visible = true;
    }

    public void MkFireoff()
    {
      Game.Room.RegisteredNPCs["mk"].Visible = false;
      this.nMkFireoff.Visible = true;
      this.nMkFireoff.Playing = true;
    }

    public void HideMkFireoff()
    {
      this.nMkFireoff.Visible = false;
      Game.Room.RegisteredNPCs["mk"].Visible = true;
    }

    public void MkAnxious()
    {
      Game.Room.RegisteredNPCs["mk"].Visible = false;
      this.nMkAnxious.Visible = true;
      this.nMkAnxious.Playing = true;
    }

    public void MkStopAnim()
    {
      this.nMkAnxious.Playing = false;
      this.nMkAnxious.Frame = 1;
    }

    public void ShowLessChillSai()
    {
      Game.Room.RegisteredNPCs["sai"].Visible = false;
      this.nSaiLessChill.Visible = true;
    }
  }
}
