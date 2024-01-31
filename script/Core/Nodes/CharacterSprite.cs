// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.CharacterSprite
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "face")]
  public class CharacterSprite : SimpleAnimatedSprite
  {
    public const string STATE_STAND = "stand";
    public const string STATE_WALK = "walk";
    public const string STATE_RUN = "run";
    public const string STATE_SNEAK = "sneak";
    public const string STATE_IDLE = "idle";
    public const string STATE_SIT = "sit";
    public const string STATE_PUSH = "push";
    public const string STATE_PUSH_STAND = "push_stand";
    private float _baseFps;
    private float _fpsMultiplier = 1f;
    private string _state;
    private LacieEngine.Core.Direction _direction = LacieEngine.Core.Direction.None;
    private Dictionary<string, string> _stateOverrides = new Dictionary<string, string>();
    private Dictionary<string, Texture> _textureOverrides = new Dictionary<string, Texture>();

    [Export(PropertyHint.None, "")]
    public string CharacterId { get; set; }

    [Export(PropertyHint.None, "")]
    public string State
    {
      get => this._state;
      set => this.SetState(value);
    }

    [Export(PropertyHint.None, "")]
    public LacieEngine.Core.Direction.Enum Direction
    {
      get => (LacieEngine.Core.Direction.Enum) this._direction;
      set => this.SetDirection((LacieEngine.Core.Direction) value);
    }

    public float FpsMultiplier
    {
      get => this._fpsMultiplier;
      set => this.SetFpsMultiplier(value);
    }

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      LacieEngine.Core.Direction direction = this._direction;
      if (this._state != null)
        this.UpdateState();
      if (direction != LacieEngine.Core.Direction.None)
      {
        this._direction = direction;
        this.UpdateDirection();
      }
      this.LightMask = 2;
    }

    public void OverrideTextureForState(string state, Texture texture)
    {
      if (texture != null)
        this._textureOverrides[state] = texture;
      if (!(this._state == state))
        return;
      this.Texture = texture;
    }

    public void OverrideStateWithState(string oldState, string newState)
    {
      this._stateOverrides[oldState] = newState;
      if (!(this._state == oldState))
        return;
      this.UpdateState();
    }

    public void ClearOverrides()
    {
      this._textureOverrides.Clear();
      this._stateOverrides.Clear();
    }

    private void SetState(string state)
    {
      if (!this.IsInsideTree())
      {
        this._state = state;
      }
      else
      {
        if (this._state == state)
          return;
        this._state = state;
        this.UpdateState();
      }
    }

    private void SetDirection(LacieEngine.Core.Direction direction)
    {
      if (!this.IsInsideTree())
      {
        this._direction = direction;
      }
      else
      {
        if (this._direction == direction)
          return;
        this._direction = direction;
        this.UpdateDirection();
      }
    }

    private void SetFpsMultiplier(float fpsMultiplier)
    {
      if ((double) this._fpsMultiplier == (double) fpsMultiplier)
        return;
      this._fpsMultiplier = fpsMultiplier;
      this.FPS = this._baseFps * this._fpsMultiplier;
    }

    private void UpdateState()
    {
      if (Engine.EditorHint)
        return;
      string str = this._state;
      if (this._stateOverrides.ContainsKey(str))
        str = this._stateOverrides[str];
      Game.Characters.Get(this.CharacterId).UpdateSpriteState((Node) this, str);
      if (this._textureOverrides.ContainsKey(str))
        this.Texture = this._textureOverrides[str];
      this._baseFps = this.FPS;
      this._fpsMultiplier = 1f;
    }

    private void UpdateDirection()
    {
      if (Engine.EditorHint)
        return;
      string str = this._state;
      if (this._stateOverrides.ContainsKey(str))
        str = this._stateOverrides[str];
      Game.Characters.Get(this.CharacterId).UpdateSpriteDirection((Node) this, this._direction, str);
    }
  }
}
