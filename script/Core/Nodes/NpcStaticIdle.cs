// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.NpcStaticIdle
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
  public class NpcStaticIdle : Node2D, IIdle, ICharaWithStates
  {
    private CharacterSprite nSprite;

    [Export(PropertyHint.None, "")]
    public string CharacterId { get; set; }

    [Export(PropertyHint.None, "")]
    public string IdleSpriteState { get; set; } = "idle";

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
      Game.Room.RegisteredNPCs[this.CharacterId] = (Node2D) this;
    }

    public void StartIdleAnimation() => this.nSprite.Playing = true;

    public void StopIdleAnimation() => this.nSprite.Playing = false;

    private bool Validate()
    {
      if (!this.CharacterId.IsNullOrEmpty())
        return true;
      Log.Error((object) "Character ID not specified!");
      return false;
    }
  }
}
