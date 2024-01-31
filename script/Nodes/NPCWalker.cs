// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.NPCWalker
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  public class NPCWalker : WalkingCharacter
  {
    public NPCWalker()
    {
    }

    public NPCWalker(string characterName)
      : this()
    {
      this.CharacterId = characterName;
      this.DefaultDirection = Direction.Down;
    }

    public NPCWalker(string characterName, string defaultDirection)
      : this()
    {
      this.CharacterId = characterName;
      this.DefaultDirection = Direction.FromString(defaultDirection);
    }

    public override void _EnterTree()
    {
      this.Name = this.CharacterId.ToPascalCase();
      this.CreateCharacterSprite();
      Game.Room.RegisteredNPCs[this.CharacterId] = (Node2D) this;
    }

    protected override void _CharacterReady()
    {
      this.SpriteState = "stand";
      this.TurnToDefault();
      this.Ready = true;
    }

    public override void _PhysicsProcess(float delta)
    {
      if (!this.Ready)
        return;
      if (this.AutoMovement)
        this.NewDirection = this.AutoDirection;
      this.UpdateState();
      this.MoveAndSlide(this.EightDirectionalMotion());
      if (this.AutoMovement)
        this.CheckIfAutoMovementOver();
      this.PlayAnimations();
    }

    public override Vector2 GetMovementWithFloorModifier(Vector2 movement) => movement;
  }
}
