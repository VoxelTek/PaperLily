// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.PushableObject
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  [ExportType(icon = "trolley")]
  public class PushableObject : KinematicBody2D, IActionObject
  {
    private Vector2 _playerRelativePosition;
    private Direction _pushDirection;
    private CollisionShape2D _tempCollisionShape;

    [Export(PropertyHint.None, "")]
    public string PositionVariable { get; set; }

    public PushableObject()
    {
      this.CollisionLayer = 17U;
      this.CollisionMask = 3U;
    }

    public override void _Ready()
    {
      if (!this.PositionVariable.IsNullOrEmpty() && !Game.Variables.GetVariable(this.PositionVariable).IsNullOrEmpty())
        this.Position = GDUtil.StringToVector2(Game.Variables.GetVariable(this.PositionVariable));
      this.SetPhysicsProcess(false);
    }

    public void Activate()
    {
      this._pushDirection = Game.Player.SpriteDirection;
      Game.InputProcessor = Inputs.Processor.PlayerInObject;
      Game.Player.SetPlayerState(CharacterState.InObject);
      Game.Player.CollisionEnabled = false;
      this._playerRelativePosition = Game.Player.Node.GlobalPosition - this.GlobalPosition;
      Game.Player.SpriteState = "push";
      Game.Player.SpriteDirection = this._pushDirection;
      RectangleShape2D rectangleShape2D = new RectangleShape2D();
      rectangleShape2D.Extents = new Vector2(11.5f, 4f) + new Vector2(1f, 1f);
      this._tempCollisionShape = new CollisionShape2D();
      this._tempCollisionShape.Shape = (Shape2D) rectangleShape2D;
      this.AddChild((Node) this._tempCollisionShape);
      this._tempCollisionShape.GlobalPosition = Game.Player.Node.GlobalPosition + new Vector2(0.0f, -4f);
      this.SetProcessInput(true);
      this.SetPhysicsProcess(true);
    }

    public void Deactivate()
    {
      Game.InputProcessor = Inputs.Processor.Player;
      Game.Player.SetPlayerState(CharacterState.Standing);
      this._tempCollisionShape.DeleteIfValid();
      this._tempCollisionShape = (CollisionShape2D) null;
      Game.Player.CollisionEnabled = true;
      this.SetProcessInput(false);
      this.SetPhysicsProcess(false);
      this.PixelSnap();
      if (this.PositionVariable.IsNullOrEmpty())
        return;
      Game.Variables.SetVariable(this.PositionVariable, this.Position.x.ToString() + "," + this.Position.y.ToString());
    }

    public override void _Input(InputEvent @event)
    {
      if (!Inputs.HandleSingle(@event, Inputs.Processor.PlayerInObject, "input_action"))
        return;
      this.Deactivate();
    }

    public override void _PhysicsProcess(float delta)
    {
      Direction directionFromInput = Inputs.GetDirectionFromInput(Direction.None, Inputs.Processor.PlayerInObject);
      if (directionFromInput == Direction.None || this._pushDirection.HasX() && !directionFromInput.HasX() || this._pushDirection.HasY() && !directionFromInput.HasY())
      {
        Game.Player.SpriteState = "push_stand";
        Game.Player.SpriteDirection = this._pushDirection;
      }
      else
      {
        Direction direction = this._pushDirection.HasX() ? directionFromInput.FlattenX() : directionFromInput.FlattenY();
        Game.Player.SpriteState = "push";
        Game.Player.SpriteDirection = this._pushDirection;
        Vector2 vector2 = this.MoveAndSlide(direction.ToVector() * 30f);
        this.PixelSnap((double) vector2.x == 0.0, (double) vector2.y == 0.0);
        Game.Player.Node.Position = this.GlobalPosition + this._playerRelativePosition;
      }
    }
  }
}
