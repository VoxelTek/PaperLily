// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.Chair
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
    public class Chair : Node2D, IEditorArea
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
        public Vector2 PlayerOffset = new Vector2(0.0f, 0.0f);
        [Export(PropertyHint.None, "")]
        public LacieEngine.Core.Direction.Enum Direction = LacieEngine.Core.Direction.Enum.Down;

        [Export(PropertyHint.None, "")]
        public Vector2 Area { get; set; } = new Vector2(32f, 32f);

        [Export(PropertyHint.None, "")]
        public bool Wide { get; set; }

        [Export(PropertyHint.None, "")]
        public int WideMargin { get; set; } = 8;

        [Export(PropertyHint.None, "")]
        public NodePath RelatedNode { get; private set; } = new NodePath();

        [Export(PropertyHint.None, "")]
        public bool ShowPlayerBehindObject { get; set; }

        public override void _EnterTree()
        {
            if (Engine.EditorHint)
                return;
            Chair.ActualSitSurface actualSitSurface = new Chair.ActualSitSurface(this);
            actualSitSurface.CollisionLayer = 1U;
            actualSitSurface.CollisionMask = 0U;
            this.AddChild((Node)actualSitSurface);
            CollisionShape2D collisionShape2D = new CollisionShape2D();
            collisionShape2D.Shape = (Shape2D)new RectangleShape2D() {
                Extents = (this.Area / 2f)
            };
            collisionShape2D.Position = this.GetPixelPerfectOffset();
            actualSitSurface.AddChild((Node)collisionShape2D);
        }

        void IEditorArea.Update() => this.Update();

        public class ActualSitSurface : StaticBody2D, IActionObject
        {
            public const int StandUpDistance = 10;
            private Chair _surface;
            private CharacterSprite _playerSittingSprite;
            private Vector2 _playerOriginalPosition;

            public ActualSitSurface(Chair surface) => this._surface = surface;

            public override void _Ready() => this.SetProcessInput(false);

            public void Activate()
            {
                Game.InputProcessor = Inputs.Processor.PlayerInObject;
                Game.Player.Node.Visible = false;
                Game.Player.CollisionEnabled = false;
                LacieEngine.Core.Direction direction = (LacieEngine.Core.Direction)this._surface.Direction;
                this._playerOriginalPosition = Game.Player.Node.GlobalPosition;
                Game.Player.SetPlayerState(CharacterState.InObject);
                this._playerSittingSprite = new CharacterSprite();
                this._playerSittingSprite.Frame = 0;
                if (this._surface.RelatedNode.IsNullOrEmpty())
                    this.AddChild((Node)this._playerSittingSprite);
                else
                    this._surface.GetNode(this._surface.RelatedNode).AddChild((Node)this._playerSittingSprite);
                this._playerSittingSprite.CharacterId = Game.State.Party[0];
                this._playerSittingSprite.State = "sit";
                this._playerSittingSprite.Direction = (LacieEngine.Core.Direction.Enum)direction;
                this._playerSittingSprite.ShowBehindParent = this._surface.ShowPlayerBehindObject;
                this._playerSittingSprite.GlobalPosition = this._surface.GlobalPosition + this._surface.PlayerOffset;
                if (direction.HasY() && this._surface.Wide)
                {
                    float val1_1 = this.GlobalPosition.x - this._surface.Area.x / 2f + (float)this._surface.WideMargin;
                    float val1_2 = this.GlobalPosition.x + this._surface.Area.x / 2f - (float)this._surface.WideMargin;
                    this._playerOriginalPosition.x = Math.Max(val1_1, this._playerOriginalPosition.x);
                    this._playerOriginalPosition.x = Math.Min(val1_2, this._playerOriginalPosition.x);
                    CharacterSprite playerSittingSprite = this._playerSittingSprite;
                    playerSittingSprite.Position = playerSittingSprite.Position + new Vector2(this._playerOriginalPosition.x - this.GlobalPosition.x, 0.0f);
                }
                else if (direction.HasX() && this._surface.Wide)
                {
                    float val1_3 = this.GlobalPosition.y - this._surface.Area.y / 2f + (float)this._surface.WideMargin;
                    float val1_4 = this.GlobalPosition.y + this._surface.Area.y / 2f - (float)this._surface.WideMargin;
                    this._playerOriginalPosition.y = Math.Max(val1_3, this._playerOriginalPosition.y);
                    this._playerOriginalPosition.y = Math.Min(val1_4, this._playerOriginalPosition.y);
                    CharacterSprite playerSittingSprite = this._playerSittingSprite;
                    playerSittingSprite.Position = playerSittingSprite.Position + new Vector2(0.0f, this._playerOriginalPosition.y - this.GlobalPosition.y);
                }
                Game.Player.Node.GlobalPosition = this._playerSittingSprite.GlobalPosition;
                Game.StoryPlayer.Connect("DialogueStarted", this, "Deactivate", null, 0U);
                this.SetProcessInput(true);
            }

            public void Deactivate()
            {
                this._playerSittingSprite.Delete();
                this._playerSittingSprite = (CharacterSprite)null;
                Game.Player.Node.Visible = true;
                Game.Player.SetDirection((LacieEngine.Core.Direction)this._surface.Direction);
                Vector2 vector2 = this.GlobalPosition;
                LacieEngine.Core.Direction direction = (LacieEngine.Core.Direction)this._surface.Direction;
                if (direction.HasX())
                    vector2 = new Vector2((float)(int)((double)this.GlobalPosition.x + (double)direction.ToVector().x * ((double)this._surface.Area.x / 2.0 + 10.0)), this._surface.Wide ? (float)(int)(double)(int)this._playerOriginalPosition.y : (float)(int)this.GlobalPosition.y);
                else if (direction.HasY())
                    vector2 = new Vector2(this._surface.Wide ? (float)(int)this._playerOriginalPosition.x : (float)(int)this.GlobalPosition.x, (float)(int)((double)this.GlobalPosition.y + (double)direction.ToVector().y * ((double)this._surface.Area.y / 2.0 + 10.0)));
                Game.StoryPlayer.Disconnect("DialogueStarted", this, "Deactivate");
                Game.Player.Node.GlobalPosition = vector2;
                Game.Player.CollisionEnabled = true;
                Game.InputProcessor = Inputs.Processor.Player;
                this.SetProcessInput(false);
            }

            public bool IsValidForDirection(LacieEngine.Core.Direction direction)
            {
                return this._surface.Direction.ToDirection().GetOpposite() == direction;
            }

            public override void _Input(InputEvent @event)
            {
                switch (Inputs.Handle(@event, Inputs.Processor.PlayerInObject, Chair.HandledInputs))
                {
                    case "input_action":
                        this.Deactivate();
                        break;
                    case "input_left":
                        if (!((LacieEngine.Core.Direction)this._surface.Direction == LacieEngine.Core.Direction.Left))
                            break;
                        this.Deactivate();
                        break;
                    case "input_up":
                        if (!((LacieEngine.Core.Direction)this._surface.Direction == LacieEngine.Core.Direction.Up))
                            break;
                        this.Deactivate();
                        break;
                    case "input_right":
                        if (!((LacieEngine.Core.Direction)this._surface.Direction == LacieEngine.Core.Direction.Right))
                            break;
                        this.Deactivate();
                        break;
                    case "input_down":
                        if (!((LacieEngine.Core.Direction)this._surface.Direction == LacieEngine.Core.Direction.Down))
                            break;
                        this.Deactivate();
                        break;
                }
            }
        }
    }
}
