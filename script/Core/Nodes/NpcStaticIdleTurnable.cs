// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.NpcStaticIdleTurnable
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "person")]
  public class NpcStaticIdleTurnable : Node2D, ITurnable, IIdle, ICharaWithStates
  {
    private CharacterSprite nSprite;

    [Export(PropertyHint.None, "")]
    public string CharacterId { get; set; }

    [Export(PropertyHint.None, "")]
    public LacieEngine.Core.Direction.Enum DefaultDirection { get; set; } = (LacieEngine.Core.Direction.Enum) LacieEngine.Core.Direction.Down;

    [Export(PropertyHint.None, "")]
    public bool TurningEnabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public string TurnSpriteState { get; set; } = "stand";

    [Export(PropertyHint.None, "")]
    public string IdleSpriteState { get; set; } = "idle";

    public LacieEngine.Core.Direction.Enum Direction
    {
      get => this.nSprite.Direction;
      set => this.Turn((LacieEngine.Core.Direction) value);
    }

    public string SpriteState
    {
      get => this.nSprite.State;
      set => this.nSprite.State = value;
    }

    public override void _EnterTree()
    {
      if (Engine.EditorHint || !this.Validate())
        return;
      this.nSprite = this.EnsureNode<CharacterSprite>("Sprite");
      this.nSprite.CharacterId = this.CharacterId;
      this.nSprite.State = this.IdleSpriteState;
      this.nSprite.Direction = this.DefaultDirection;
      Game.Room.RegisteredNPCs[this.CharacterId] = (Node2D) this;
    }

    public void Turn(LacieEngine.Core.Direction direction)
    {
      if (!this.TurningEnabled)
        return;
      this.nSprite.State = this.TurnSpriteState;
      this.nSprite.Direction = (LacieEngine.Core.Direction.Enum) direction;
    }

    public void TurnToDefault() => this.StartIdleAnimation();

    public void StartIdleAnimation() => this.nSprite.State = this.IdleSpriteState;

    public void StopIdleAnimation()
    {
      this.nSprite.State = this.TurnSpriteState;
      this.nSprite.Direction = this.DefaultDirection;
    }

    private bool Validate()
    {
      if (!this.CharacterId.IsNullOrEmpty())
        return true;
      Log.Error((object) "Character ID not specified!");
      return false;
    }
  }
}
