// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.PlayerPanelAttack
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Minigames
{
  public class PlayerPanelAttack : Battle.BattleProcess
  {
    public Texture Texture;
    public Vector2 TextureOffset = Vector2.Zero;
    public float ImpactTime;
    public int Hframes;
    public int Vframes;
    public string Frames;
    public float Fps;
    public bool Loop;
    public float StartOffset;
    public float PositionCalculationOffset;
    public float DamageDuration;
    public float Lifetime;
    protected int _id;
    protected int _x;
    protected int _y;
    protected Battle _battle;
    protected float _spawnTime;
    protected float _dmgExpiryTime;
    protected float _expiryTime;
    protected float _initTime;
    protected Sprite _sprite;

    public override int Id => this._id;

    public override float InitTime => this._initTime;

    public override void PreInit(Battle battle)
    {
      this._battle = battle;
      this._id = this._battle.CurAttackId;
      this._spawnTime = this.ImpactTime - this.StartOffset;
      this._dmgExpiryTime = this.ImpactTime + this.DamageDuration;
      this._expiryTime = this.ImpactTime + this.Lifetime;
      this._initTime = Math.Min(this._spawnTime, this.ImpactTime - this.PositionCalculationOffset);
    }

    public override void Init()
    {
      this._x = this._battle._playerX;
      this._y = this._battle._playerY;
      this._sprite = (Sprite) this.MakeAnimatedSprite();
    }

    public override void Process(float time)
    {
      if (this.ShouldDelete)
        return;
      if (!this._sprite.Visible && (double) time >= (double) this._spawnTime)
      {
        this._battle.Cells[this._x, this._y].Sprite.AddChild((Node) this._sprite);
        this._sprite.Visible = true;
      }
      if ((double) time >= (double) this.ImpactTime && (double) time < (double) this._dmgExpiryTime)
        this._battle.DamagePanel(this._x, this._y);
      if ((double) time < (double) this._expiryTime)
        return;
      this._sprite.DeleteIfValid();
      this.ShouldDelete = true;
    }

    protected SimpleAnimatedSprite MakeAnimatedSprite()
    {
      SimpleAnimatedSprite simpleAnimatedSprite = GDUtil.MakeNode<SimpleAnimatedSprite>("Attack");
      simpleAnimatedSprite.Texture = this.Texture;
      simpleAnimatedSprite.Offset = this.TextureOffset;
      simpleAnimatedSprite.Hframes = this.Hframes;
      simpleAnimatedSprite.Vframes = this.Vframes;
      simpleAnimatedSprite.FPS = this.Fps;
      simpleAnimatedSprite.Loop = this.Loop;
      simpleAnimatedSprite.AnimationFrames = this.Frames;
      simpleAnimatedSprite.Visible = false;
      return simpleAnimatedSprite;
    }
  }
}
