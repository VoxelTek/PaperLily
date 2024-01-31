// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1GardenExitRitual
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1GardenExitRitual : GameRoom
  {
    [LacieEngine.API.GetNode("Main/VaseNoFlower")]
    private Node2D nVaseNoFlower;
    [LacieEngine.API.GetNode("Main/VaseFlower")]
    private Node2D nVaseFlower;
    [LacieEngine.API.GetNode("Main/VaseNoFlower/Paperlily")]
    private Node2D nPaperLily;
    [LacieEngine.API.GetNode("Main/DoorGroup/Door")]
    private Sprite nDoor;
    [LacieEngine.API.GetNode("Main/DoorGroup/White")]
    private Node2D nDoorWhite;
    private PVar varTookFlower = (PVar) "ch1.garden_took_flower";
    private PVar varPlacedFlower = (PVar) "ch1.garden_placed_flower";

    public override void _UpdateRoom()
    {
      bool flag = (bool) this.varPlacedFlower.Value;
      this.nVaseNoFlower.Visible = (bool) this.varTookFlower.Value;
      this.nVaseFlower.Visible = !(bool) this.varTookFlower.Value;
      this.nDoor.Frame = flag ? 2 : 0;
      this.nDoorWhite.Visible = flag;
      this.nPaperLily.Visible = flag;
    }

    public void DespawnLacie()
    {
      Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) Game.Room.RegisteredNPCs["lacie"], 1f));
    }

    public void CameraPanUp()
    {
      Game.Animations.Play((LacieAnimation) new Ch1GardenExitRitual.CameraPanUpAnimation((Node2D) Game.Camera.Node, Game.Camera.Node.Position, Game.Camera.Node.Position + Vector2.Up * 1000f, 2f));
    }

    public class CameraPanUpAnimation : LacieAnimation
    {
      private Node2D _node;
      private Vector2 _from;
      private Vector2 _to;
      private Vector2 _track;

      public CameraPanUpAnimation(Node2D node, Vector2 from, Vector2 to, float duration)
      {
        this.Node = (Node) (this._node = node);
        this.Duration = duration;
        this._from = from;
        this._to = to;
      }

      public override void InitialCalculations() => this._track = this._to - this._from;

      public override void UpdateToFirstFrame() => this._node.Position = this._from;

      public override void UpdateToCurrentFrame()
      {
        float num = this.Elapsed / this.Duration;
        this._node.Position = this._from + this._track * (num * num * num);
      }

      public override void UpdateToFinalFrame() => this._node.Position = this._to;
    }
  }
}
