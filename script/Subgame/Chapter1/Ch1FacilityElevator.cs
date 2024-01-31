// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityElevator
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityElevator : GameRoom
  {
	[LacieEngine.API.GetNode("Background/WallIndicator/B1F")]
	private Node2D nB1FLight;
	[LacieEngine.API.GetNode("Background/WallIndicator/B2F")]
	private Node2D nB2FLight;
	[LacieEngine.API.GetNode("Background/WallIndicator/B3F")]
	private Node2D nB3FLight;
	[LacieEngine.API.GetNode("Background/WallIndicator/B4F")]
	private Node2D nB4FLight;
	private PVar varElevatorOn = (PVar) "ch1.facility_elevator_on";
	private PVar varElevatorLocation = (PVar) "ch1.facility_elevator_location";
	public bool Riding;
	private const float CAMERA_SHAKE_FPS = 10f;
	private Random random = new Random();
	private Vector2 _cameraOffset = Vector2.Zero;
	private Vector2 _direction = Direction.Down.ToVector();
	private float _elapsed;

	public override void _UpdateRoom()
	{
	  if (!(bool) this.varElevatorOn.Value)
		return;
	  int num = (int) this.varElevatorLocation.Value;
	  this.nB1FLight.Visible = num == 1;
	  this.nB2FLight.Visible = num == 2;
	  this.nB3FLight.Visible = num == 3;
	  this.nB4FLight.Visible = num == 4;
	}

	public override void _RoomProcess(float delta)
	{
	  if (!this.Riding)
		return;
	  this.ProcessShake(delta);
	}

	public void StartElevator() => this.Riding = true;

	public void StopElevator() => this.Riding = false;

	private void ProcessShake(float delta)
	{
	  this._elapsed += delta;
	  if ((double) this._elapsed < 0.10000000149011612)
		return;
	  this._elapsed = 0.0f;
	  Vector2 vector2_1 = this._direction;
	  Vector2 vector2_2 = this._cameraOffset + vector2_1;
	  if ((double) Math.Abs(vector2_2.x) > 1.0 || (double) Math.Abs(vector2_2.y) > 1.0)
	  {
		vector2_1 = this._direction = this.random.NextDirection().ToVector();
		vector2_2 = this._cameraOffset + vector2_1;
	  }
	  if ((double) Math.Abs(vector2_2.x) > 1.0)
		vector2_1.x = 0.0f;
	  if ((double) Math.Abs(vector2_2.y) > 1.0)
		vector2_1.y = 0.0f;
	  this._cameraOffset = vector2_2;
	  if (vector2_1 == Vector2.Zero)
	  {
		this._cameraOffset = Vector2.Zero;
		this._direction = this.random.NextDirection().ToVector();
	  }
	  Game.Camera.Node.Offset = this._cameraOffset.Round();
	}
  }
}
