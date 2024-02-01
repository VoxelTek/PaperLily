// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.ChairV2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  [ExportType(icon = "chair")]
  public class ChairV2 : Node2D, IEditorArea, IToggleable
  {
    private static readonly string[] HandledInputs = new string[5]
    {
      "input_action",
      "input_left",
      "input_up",
      "input_right",
      "input_down"
    };
    [Export(PropertyHint.None, "")]
    public Vector2 PlayerOffsetLeft = new Vector2(0.0f, 0.0f);
    [Export(PropertyHint.None, "")]
    public Vector2 PlayerOffsetUp = new Vector2(0.0f, 0.0f);
    [Export(PropertyHint.None, "")]
    public Vector2 PlayerOffsetRight = new Vector2(0.0f, 0.0f);
    [Export(PropertyHint.None, "")]
    public Vector2 PlayerOffsetDown = new Vector2(0.0f, 0.0f);

    [Export(PropertyHint.None, "")]
    public Vector2 Area { get; set; } = new Vector2(32f, 32f);

    [Export(PropertyHint.Flags, "Left,Up,Right,Down")]
    public byte Directions { get; set; }

    [Export(PropertyHint.None, "")]
    public bool WideX { get; set; }

    [Export(PropertyHint.None, "")]
    public bool WideY { get; set; }

    [Export(PropertyHint.None, "")]
    public int WideMarginX { get; set; } = 8;

    [Export(PropertyHint.None, "")]
    public int WideMarginY { get; set; } = 8;

    [Export(PropertyHint.None, "")]
    public NodePath RelatedNode { get; private set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public bool ShowPlayerBehindObject { get; set; }

    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    public override void _EnterTree()
    {
      if (Engine.EditorHint)
        return;
      ChairV2.ActualSitSurface actualSitSurface = new ChairV2.ActualSitSurface(this);
      actualSitSurface.CollisionLayer = 1U;
      actualSitSurface.CollisionMask = 0U;
      this.AddChild((Node) actualSitSurface);
      CollisionShape2D collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = (Shape2D) new RectangleShape2D()
      {
        Extents = (this.Area / 2f)
      };
      collisionShape2D.Position = this.GetPixelPerfectOffset();
      actualSitSurface.AddChild((Node) collisionShape2D);
    }

    void IEditorArea.Update() => this.Update();

    public class ActualSitSurface : StaticBody2D, IActionObject
    {
      public const int StandUpDistance = 10;
      private ChairV2 _surface;
      private CharacterSprite _playerSittingSprite;
      private Vector2 _playerOriginalPosition;
      private Direction _playerOriginalDirection;
      private Direction _curSurfaceDirection;

      public ActualSitSurface(ChairV2 surface) => this._surface = surface;

      public override void _Ready() => this.SetProcessInput(false);

      public void Activate()
      {
        if (!this._surface.Enabled)
          return;
        Game.InputProcessor = Inputs.Processor.PlayerInObject;
        Game.Player.Node.Visible = false;
        Game.Player.CollisionEnabled = false;
        this._playerOriginalPosition = Game.Player.Node.GlobalPosition;
        this._playerOriginalDirection = Game.Player.SpriteDirection;
        this._curSurfaceDirection = this._playerOriginalDirection.GetOpposite();
        Game.Player.SetPlayerState(CharacterState.InObject);
        this._playerSittingSprite = new CharacterSprite();
        this._playerSittingSprite.Frame = 0;
        if (this._surface.RelatedNode.IsNullOrEmpty())
          this.AddChild((Node) this._playerSittingSprite);
        else
          this._surface.GetNode(this._surface.RelatedNode).AddChild((Node) this._playerSittingSprite);
        this._playerSittingSprite.CharacterId = Game.State.Party[0];
        this._playerSittingSprite.State = "sit";
        this._playerSittingSprite.Direction = (Direction.Enum) this._curSurfaceDirection;
        this._playerSittingSprite.ShowBehindParent = this._surface.ShowPlayerBehindObject;
        Vector2 vector2;
        switch (this._playerOriginalDirection.ToEnum())
        {
          case Direction.Enum.Left:
            vector2 = this._surface.PlayerOffsetLeft;
            break;
          case Direction.Enum.Up:
            vector2 = this._surface.PlayerOffsetUp;
            break;
          case Direction.Enum.Right:
            vector2 = this._surface.PlayerOffsetRight;
            break;
          case Direction.Enum.Down:
            vector2 = this._surface.PlayerOffsetDown;
            break;
          default:
            vector2 = Vector2.Zero;
            break;
        }
        this._playerSittingSprite.GlobalPosition = this._surface.GlobalPosition + vector2;
        if (this._curSurfaceDirection.HasY() && this._surface.WideX)
        {
          float val1_1 = this.GlobalPosition.x - this._surface.Area.x / 2f + (float) this._surface.WideMarginX;
          float val1_2 = this.GlobalPosition.x + this._surface.Area.x / 2f - (float) this._surface.WideMarginX;
          this._playerOriginalPosition.x = Math.Max(val1_1, this._playerOriginalPosition.x);
          this._playerOriginalPosition.x = Math.Min(val1_2, this._playerOriginalPosition.x);
          CharacterSprite playerSittingSprite = this._playerSittingSprite;
          playerSittingSprite.Position = playerSittingSprite.Position + new Vector2(this._playerOriginalPosition.x - this.GlobalPosition.x, 0.0f);
        }
        else if (this._curSurfaceDirection.HasX() && this._surface.WideY)
        {
          float val1_3 = this.GlobalPosition.y - this._surface.Area.y / 2f + (float) this._surface.WideMarginY;
          float val1_4 = this.GlobalPosition.y + this._surface.Area.y / 2f - (float) this._surface.WideMarginY;
          this._playerOriginalPosition.y = Math.Max(val1_3, this._playerOriginalPosition.y);
          this._playerOriginalPosition.y = Math.Min(val1_4, this._playerOriginalPosition.y);
          CharacterSprite playerSittingSprite = this._playerSittingSprite;
          playerSittingSprite.Position = playerSittingSprite.Position + new Vector2(0.0f, this._playerOriginalPosition.y - this.GlobalPosition.y);
        }
        Game.Player.Node.GlobalPosition = this._playerSittingSprite.GlobalPosition;
        this.SetProcessInput(true);
      }

      public void Deactivate()
      {
        this._playerSittingSprite.Delete();
        this._playerSittingSprite = (CharacterSprite) null;
        Game.Player.Node.Visible = true;
        Game.Player.SetDirection(this._playerOriginalDirection.GetOpposite());
        Vector2 vector2 = this.GlobalPosition;
        if (this._curSurfaceDirection.HasY())
          vector2 = new Vector2(this._surface.WideX ? (float) (int) this._playerOriginalPosition.x : (float) (int) this.GlobalPosition.x, (float) (int) ((double) this.GlobalPosition.y + (double) this._curSurfaceDirection.ToVector().y * ((double) this._surface.Area.y / 2.0 + 10.0)));
        else if (this._curSurfaceDirection.HasX())
          vector2 = new Vector2((float) (int) ((double) this.GlobalPosition.x + (double) this._curSurfaceDirection.ToVector().x * ((double) this._surface.Area.x / 2.0 + 10.0)), this._surface.WideY ? (float) (int) (double) (int) this._playerOriginalPosition.y : (float) (int) this.GlobalPosition.y);
        Game.Player.Node.GlobalPosition = vector2;
        Game.Player.CollisionEnabled = true;
        Game.InputProcessor = Inputs.Processor.Player;
        this.SetProcessInput(false);
      }

      public bool IsValidForDirection(Direction direction)
      {
        return Direction.FromByte(this._surface.Directions).HasDirection(direction);
      }

      public override void _Input(InputEvent @event)
      {
        switch (Inputs.Handle(@event, Inputs.Processor.PlayerInObject, ChairV2.HandledInputs))
        {
          case "input_action":
            this.Deactivate();
            break;
          case "input_left":
            if (!(this._curSurfaceDirection == Direction.Left))
              break;
            this.Deactivate();
            break;
          case "input_up":
            if (!(this._curSurfaceDirection == Direction.Up))
              break;
            this.Deactivate();
            break;
          case "input_right":
            if (!(this._curSurfaceDirection == Direction.Right))
              break;
            this.Deactivate();
            break;
          case "input_down":
            if (!(this._curSurfaceDirection == Direction.Down))
              break;
            this.Deactivate();
            break;
        }
      }
    }
  }
}
