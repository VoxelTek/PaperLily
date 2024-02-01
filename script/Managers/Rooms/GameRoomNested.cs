// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.GameRoomNested
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class GameRoomNested : GameRoom
  {
	[Export(PropertyHint.None, "")]
	public NodePath Subroom = new NodePath();
	protected GameRoom nRoom;

	public override void _Initialize()
	{
	  this.nRoom = this.GetNode<GameRoom>(this.Subroom);
	  Injector.Resolve((object) this.nRoom);
	  this.nRoom.InjectNodes();
	  this._InitializeNested();
	}

	public virtual void _InitializeNested()
	{
	}

	public override void _AfterFadeIn() => this.nRoom._AfterFadeIn();

	public override void _BeforeFadeOut() => this.nRoom._BeforeFadeOut();

	public override void _AfterFadeOut() => this.nRoom._AfterFadeOut();

	public override void _UpdateRoom() => this.nRoom._UpdateRoom();

	public override void _RoomProcess(float delta) => this.nRoom._RoomProcess(delta);

	public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

	public override Node FindNodeInRoom(string nodeName) => this.nRoom.FindNodeInRoom(nodeName);
  }
}
