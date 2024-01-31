// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneHome2fHallway
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
  public class Ch1CutsceneHome2fHallway : GameRoom
  {
    [LacieEngine.API.GetNode("Room")]
    private GameRoom nRoom;
    [LacieEngine.API.GetNode("Room/Main/Ruffle")]
    private SimpleAnimatedSprite nRuffle;
    [LacieEngine.API.GetNode("Room/Main/FixHair")]
    private SimpleAnimatedSprite nFixHair;

    public override void _UpdateRoom() => this.nRoom._UpdateRoom();

    public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

    public override Node FindNodeInRoom(string nodeName) => this.nRoom.FindNodeInRoom(nodeName);

    public void Ruffle()
    {
      Node2D node1 = this.GetMainLayer().GetNode<Node2D>((NodePath) "Lacie");
      Node2D node2 = this.GetMainLayer().GetNode<Node2D>((NodePath) "Hiro");
      node1.Visible = false;
      node2.Visible = false;
      this.nRuffle.Visible = true;
      this.nRuffle.Playing = true;
    }

    public void PostRuffle()
    {
      Node2D node = this.GetMainLayer().GetNode<Node2D>((NodePath) "Hiro");
      this.nFixHair.Visible = true;
      node.Visible = true;
      this.nRuffle.Visible = false;
    }

    public void FixHair() => this.nFixHair.Playing = true;
  }
}
