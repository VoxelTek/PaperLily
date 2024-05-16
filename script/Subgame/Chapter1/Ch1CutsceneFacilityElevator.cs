// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1CutsceneFacilityElevator
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1CutsceneFacilityElevator : GameRoomNested
  {
	[LacieEngine.API.GetNode("Room/Background/Flood1")]
	private Node2D nFlood1;
	[LacieEngine.API.GetNode("Room/Background/Flood2")]
	private Node2D nFlood2;
	[LacieEngine.API.GetNode("Room/Background/Flood3")]
	private Node2D nFlood3;
	[LacieEngine.API.GetNode("Room/Background/Flood4")]
	private Node2D nFlood4;
	[LacieEngine.API.GetNode("Room/Background/Flood5")]
	private Node2D nFlood5;
	[LacieEngine.API.GetNode("Room/Background/Flood6")]
	private Node2D nFlood6;
	[LacieEngine.API.GetNode("Lighting/dark")]
	private RoomLighting nLightDark;

	public override void _BeforeFadeIn() => ((Ch1FacilityElevator) this.nRoom).Riding = true;

	public void Flood0()
	{
	  ((Ch1FacilityElevator) this.nRoom).Riding = false;
	  Game.Camera.Shake(0.3f, 10f);
	}

	public void PowerDown() => this.nLightDark.Apply();

	public void Flood1() => this.nFlood1.Visible = true;

	public void Flood2()
	{
	  this.nFlood1.Visible = false;
	  this.nFlood2.Visible = true;
	}

	public void Flood3()
	{
	  this.nFlood2.Visible = false;
	  this.nFlood3.Visible = true;
	}

	public void Flood4()
	{
	  this.nFlood3.Visible = false;
	  this.nFlood4.Visible = true;
	}

	public void Flood5()
	{
	  this.nFlood4.Visible = false;
	  this.nFlood5.Visible = true;
	}

	public void Flood6()
	{
	  this.nFlood5.Visible = false;
	  this.nFlood6.Visible = true;
	}
  }
}
