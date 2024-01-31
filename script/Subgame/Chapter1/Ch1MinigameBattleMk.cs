// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleMk
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1MinigameBattleMk : Battle
  {
    [Export(PropertyHint.None, "")]
    private Texture texKnife;
    [Export(PropertyHint.None, "")]
    private Texture texKnifePanel;
    [Export(PropertyHint.None, "")]
    private Texture texKnifeSlash;
    [Export(PropertyHint.None, "")]
    private Texture texFire;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxStab;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxSlash;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxThrow;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxFire;
    private Random rnd = new Random();
    private const int KNIFE_SPAWN_OFFSET = 320;
    private const float TELEGRAPH_DURATION = 1f;
    private const float TELEGRAPH_OFFSET = 0.0f;

    public void RandomFallPattern(double t, out double finalT)
    {
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5 + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5 + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5 + 0.5 + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5 + 0.5 + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5 + 0.5 + 0.5 + 0.5);
      this.AttackPanel(this.rnd.Next(this.Width), this.rnd.Next(this.Height), t + 0.5 + 0.5 + 0.5 + 0.5);
      finalT = t + 3.0;
    }

    public void RandomSlashPattern(double t, out double finalT)
    {
      this.SlashPanel(this.rnd.Next(1, this.Width - 1), this.rnd.Next(this.Height - 1), t);
      this.SlashPanel(this.rnd.Next(1, this.Width - 1), this.rnd.Next(this.Height - 1), t + 0.75);
      this.SlashPanel(this.rnd.Next(1, this.Width - 1), this.rnd.Next(this.Height - 1), t + 0.75 + 0.75);
      this.SlashPanel(this.rnd.Next(1, this.Width - 1), this.rnd.Next(this.Height - 1), t + 0.75 + 0.75 + 0.75);
      this.SlashPanel(this.rnd.Next(1, this.Width - 1), this.rnd.Next(this.Height - 1), t + 0.75 + 0.75 + 0.75 + 0.75);
      this.SlashPanel(this.rnd.Next(1, this.Width - 1), this.rnd.Next(this.Height - 1), t + 0.75 + 0.75 + 0.75 + 0.75 + 0.75);
      finalT = t + 4.0;
    }

    public void SlashMPattern(double t, out double finalT)
    {
      this.SlashPanel(3, 0, t);
      this.SlashPanel(1, this.Height - 2, t);
      this.SlashPanel(this.Width - 2, this.Height - 2, t);
      ++t;
      this.SlashPanel(1, 0, t);
      this.SlashPanel(this.Width - 1 - 1, 0, t);
      this.SlashPanel(3, this.Height - 1 - 1, t);
      finalT = t + 2.0;
    }

    public void HorizontalSlowPattern(double t, int count, out double finalT, bool invert = false)
    {
      for (int index = 0; index < count; ++index)
      {
        int num = this.rnd.Next(this.Height);
        for (int row = 0; row < this.Height; ++row)
        {
          if (row != num)
            this.ThrowRowSlow(row, t, invert);
        }
        t += 1.2;
      }
      finalT = t;
    }

    public void HorizontalSlowPatternDouble(double t, int count, out double finalT)
    {
      int num1 = 0;
      int num2 = num1;
      for (int index = 0; index < count; ++index)
      {
        while (num2 == num1)
          num2 = this.rnd.Next(this.Height);
        num1 = num2;
        for (int row = 0; row < this.Height; ++row)
        {
          if (row != num2)
          {
            this.ThrowRowSlow(row, t, true);
            this.ThrowRowSlow(row, t);
          }
        }
        t += 1.2;
      }
      finalT = t;
    }

    public void VerticalSlowPattern(double t, int count, out double finalT)
    {
      for (int index = 0; index < count; ++index)
      {
        int num = this.rnd.Next(1, this.Width - 1);
        for (int col = 0; col < this.Width; ++col)
        {
          if (col != num)
            this.ThrowColSlow(col, t);
        }
        t += 2.0;
      }
      finalT = t;
    }

    public void DoubleSlowPattern(double t, int count, out double finalT)
    {
      bool invert = false;
      for (int index = 0; index < count; ++index)
      {
        double finalT1;
        this.HorizontalSlowPattern(t, 1, out finalT1, invert);
        this.VerticalSlowPattern(t, 1, out finalT1);
        t += 2.2;
        invert = !invert;
      }
      finalT = t;
    }

    public void ThrowCol(int column, double time, bool invert = false)
    {
      Ch1MinigameBattleMk.KnifeProjectileStandard process = new Ch1MinigameBattleMk.KnifeProjectileStandard(this);
      process.X = column;
      process.Y = invert ? this.Height - 1 : 0;
      process.ImpactTime = (float) time;
      process.Direction = invert ? Direction.Up : Direction.Down;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxThrow, (float) time - 1f, 1.5f));
    }

    public void BurnCol(int column, double time)
    {
      for (int index = 0; index < this.Height; ++index)
      {
        Ch1MinigameBattleMk.IncineratePanelAttackInfinite process = new Ch1MinigameBattleMk.IncineratePanelAttackInfinite(this);
        process.X = column;
        process.Y = index;
        process.ImpactTime = (float) time;
        this.AddProcess((Battle.BattleProcess) process);
        this.AddSound(new Battle.BattleSound(this.sfxFire, (float) time));
      }
    }

    public void BurnRow(int row, double time)
    {
      for (int index = 0; index < this.Width; ++index)
      {
        Ch1MinigameBattleMk.IncineratePanelAttackInfinite process = new Ch1MinigameBattleMk.IncineratePanelAttackInfinite(this);
        process.X = index;
        process.Y = row;
        process.ImpactTime = (float) time;
        this.AddProcess((Battle.BattleProcess) process);
        this.AddSound(new Battle.BattleSound(this.sfxFire, (float) time));
      }
    }

    public void BurnPlayerPanel(double time)
    {
      Ch1MinigameBattleMk.IncineratePlayerPanelAttack2 process = new Ch1MinigameBattleMk.IncineratePlayerPanelAttack2(this);
      process.ImpactTime = (float) time;
      process.PositionCalculationOffset = 1f;
      this.AddProcess((Battle.BattleProcess) process);
    }

    public void BurnPanel(int column, int row, double time)
    {
      Ch1MinigameBattleMk.IncineratePanelAttack incineratePanelAttack1 = new Ch1MinigameBattleMk.IncineratePanelAttack(this);
      incineratePanelAttack1.X = column;
      incineratePanelAttack1.Y = row;
      incineratePanelAttack1.ImpactTime = (float) time;
      Ch1MinigameBattleMk.IncineratePanelAttack process1 = incineratePanelAttack1;
      Ch1MinigameBattleMk.IncineratePanelAttack incineratePanelAttack2 = new Ch1MinigameBattleMk.IncineratePanelAttack(this);
      incineratePanelAttack2.X = column;
      incineratePanelAttack2.Y = row - 1;
      incineratePanelAttack2.ImpactTime = (float) time + 0.5f;
      Ch1MinigameBattleMk.IncineratePanelAttack process2 = incineratePanelAttack2;
      Ch1MinigameBattleMk.IncineratePanelAttack incineratePanelAttack3 = new Ch1MinigameBattleMk.IncineratePanelAttack(this);
      incineratePanelAttack3.X = column - 1;
      incineratePanelAttack3.Y = row;
      incineratePanelAttack3.ImpactTime = (float) time + 0.5f;
      Ch1MinigameBattleMk.IncineratePanelAttack process3 = incineratePanelAttack3;
      Ch1MinigameBattleMk.IncineratePanelAttack incineratePanelAttack4 = new Ch1MinigameBattleMk.IncineratePanelAttack(this);
      incineratePanelAttack4.X = column + 1;
      incineratePanelAttack4.Y = row;
      incineratePanelAttack4.ImpactTime = (float) time + 0.5f;
      Ch1MinigameBattleMk.IncineratePanelAttack process4 = incineratePanelAttack4;
      Ch1MinigameBattleMk.IncineratePanelAttack incineratePanelAttack5 = new Ch1MinigameBattleMk.IncineratePanelAttack(this);
      incineratePanelAttack5.X = column;
      incineratePanelAttack5.Y = row + 1;
      incineratePanelAttack5.ImpactTime = (float) time + 0.5f;
      Ch1MinigameBattleMk.IncineratePanelAttack process5 = incineratePanelAttack5;
      this.MakeTelegraph(column, row, time);
      this.MakeTelegraph(column + 1, row, time);
      this.MakeTelegraph(column - 1, row, time);
      this.MakeTelegraph(column, row + 1, time);
      this.MakeTelegraph(column, row - 1, time);
      this.AddProcess((Battle.BattleProcess) process1);
      this.AddProcess((Battle.BattleProcess) process2);
      this.AddProcess((Battle.BattleProcess) process3);
      this.AddProcess((Battle.BattleProcess) process4);
      this.AddProcess((Battle.BattleProcess) process5);
    }

    public void BombPanel(int column, int row, double time)
    {
      this.MakeTelegraph(column, row, time);
      double num = 0.1;
      Ch1MinigameBattleMk.IncineratePanelAttackQuick process = new Ch1MinigameBattleMk.IncineratePanelAttackQuick(this);
      process.X = column;
      process.Y = row;
      process.ImpactTime = (float) time;
      this.AddProcess((Battle.BattleProcess) process);
      int col1 = column;
      int row1 = row;
      double time1 = time;
      while (--col1 >= 0)
      {
        time1 += num;
        this.AddProcess((Battle.BattleProcess) new Ch1MinigameBattleMk.IncineratePanelAttackQuick(this, col1, row1, (float) time1));
        this.AddSound(new Battle.BattleSound(this.sfxFire, (float) time1));
      }
      int col2 = column;
      double time2 = time;
      while (++col2 < this.Width)
      {
        time2 += num;
        this.AddProcess((Battle.BattleProcess) new Ch1MinigameBattleMk.IncineratePanelAttackQuick(this, col2, row1, (float) time2));
        this.AddSound(new Battle.BattleSound(this.sfxFire, (float) time2));
      }
      int col3 = column;
      double time3 = time;
      while (--row1 >= 0)
      {
        time3 += num;
        this.AddProcess((Battle.BattleProcess) new Ch1MinigameBattleMk.IncineratePanelAttackQuick(this, col3, row1, (float) time3));
        this.AddSound(new Battle.BattleSound(this.sfxFire, (float) time3));
      }
      int row2 = row;
      double time4 = time;
      while (++row2 < this.Height)
      {
        time4 += num;
        this.AddProcess((Battle.BattleProcess) new Ch1MinigameBattleMk.IncineratePanelAttackQuick(this, col3, row2, (float) time4));
        this.AddSound(new Battle.BattleSound(this.sfxFire, (float) time4));
      }
    }

    public void ThrowRow(int row, double time, bool invert = false)
    {
      Ch1MinigameBattleMk.KnifeProjectileStandard process = new Ch1MinigameBattleMk.KnifeProjectileStandard(this);
      process.X = invert ? this.Width - 1 : 0;
      process.Y = row;
      process.ImpactTime = (float) time;
      process.Direction = invert ? Direction.Left : Direction.Right;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxThrow, (float) time - 1f, 1.5f));
    }

    public void ThrowRowSlow(int row, double time, bool invert = false)
    {
      Ch1MinigameBattleMk.KnifeProjectileSlow process = new Ch1MinigameBattleMk.KnifeProjectileSlow(this);
      process.X = invert ? this.Width - 1 : 0;
      process.Y = row;
      process.ImpactTime = (float) time;
      process.Direction = invert ? Direction.Left : Direction.Right;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxThrow, (float) time - 1.8f, 1.5f));
    }

    public void ThrowColSlow(int col, double time, bool invert = false)
    {
      Ch1MinigameBattleMk.KnifeProjectileSlow process = new Ch1MinigameBattleMk.KnifeProjectileSlow(this);
      process.X = col;
      process.Y = 0;
      process.ImpactTime = (float) time;
      process.Direction = Direction.Down;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxThrow, (float) time - 1.8f, 1.5f));
    }

    public void AttackPanel(int column, int row, double time)
    {
      Ch1MinigameBattleMk.KnifePanelAttackStandard panelAttackStandard = new Ch1MinigameBattleMk.KnifePanelAttackStandard(this);
      panelAttackStandard.X = column;
      panelAttackStandard.Y = row;
      panelAttackStandard.ImpactTime = (float) time;
      Ch1MinigameBattleMk.KnifePanelAttackStandard process = panelAttackStandard;
      this.MakeTelegraph(column, row, time);
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxStab, (float) time));
    }

    public void AttackPlayerPanel(double time)
    {
      Ch1MinigameBattleMk.KnifePlayerPanelAttackStandard process = new Ch1MinigameBattleMk.KnifePlayerPanelAttackStandard(this);
      process.ImpactTime = (float) time;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxStab, (float) time));
    }

    public void SlashPanel(int column, int row, double time)
    {
      Ch1MinigameBattleMk.KnifeSlashAttackStandard slashAttackStandard = new Ch1MinigameBattleMk.KnifeSlashAttackStandard(this);
      slashAttackStandard.X = column;
      slashAttackStandard.Y = row;
      slashAttackStandard.ImpactTime = (float) time;
      Ch1MinigameBattleMk.KnifeSlashAttackStandard process = slashAttackStandard;
      this.MakeTelegraph(column - 1, row, time);
      this.MakeTelegraph(column, row, time);
      this.MakeTelegraph(column + 1, row, time);
      this.MakeTelegraph(column - 1, row + 1, time);
      this.MakeTelegraph(column, row + 1, time);
      this.MakeTelegraph(column + 1, row + 1, time);
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxSlash, (float) time, 0.4f));
    }

    public void SlashPlayerPanel(double time)
    {
      Ch1MinigameBattleMk.KnifeSlashAttackTracking process = new Ch1MinigameBattleMk.KnifeSlashAttackTracking(this);
      process.ImpactTime = (float) time;
      process.PositionCalculationOffset = 1f;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxSlash, (float) time, 0.4f));
    }

    public void MakeTelegraph(int x, int y, double time)
    {
      if (x < 0 || y < 0 || x >= this.Width || y >= this.Height)
        return;
      this.AddProcess((Battle.BattleProcess) new BasicTelegraph()
      {
        X = x,
        Y = y,
        Time = (float) time,
        Duration = 1f,
        Offset = 0.0f
      });
    }

    public void MakeTrackingTelegraph(double time, int offsetX = 0, int offsetY = 0)
    {
      this.AddProcess((Battle.BattleProcess) new PlayerPanelTelegraph()
      {
        Time = (float) time,
        Duration = 1f,
        Offset = 0.0f,
        PanelOffsetX = offsetX,
        PanelOffsetY = offsetY
      });
    }

    public void TelegraphAll(double time)
    {
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
          this.MakeTelegraph(x, y, time);
      }
    }

    public class KnifeProjectileStandard : Ch1MinigameBattleMk.KnifeProjectile
    {
      public KnifeProjectileStandard(Ch1MinigameBattleMk battle)
      {
        this.Speed = 280f;
        this.SpawnOffset = 320;
        this.Texture = battle.texKnife;
      }
    }

    public class KnifeProjectileSlow : Ch1MinigameBattleMk.KnifeProjectile
    {
      public KnifeProjectileSlow(Ch1MinigameBattleMk battle)
      {
        this.Speed = 150f;
        this.SpawnOffset = 320;
        this.Texture = battle.texKnife;
      }
    }

    public class KnifeProjectile : Battle.BattleProcess
    {
      public int SpawnOffset;
      public float Speed;
      public Texture Texture;
      public int X;
      public int Y;
      public float ImpactTime;
      private Vector2 _origin;
      public Direction Direction;
      private Vector2 DirectionVector;
      protected int _id;
      private Battle _battle;
      private float _spawnTime;
      private float _expiryTime;
      private Sprite nSprite;

      public override int Id => this._id;

      public override float InitTime => this._spawnTime;

      public override void PreInit(Battle battle)
      {
        this._battle = battle;
        this._id = this._battle.CurAttackId;
        this._origin = this._battle.Cells[this.X, this.Y].Sprite.Position - (float) this.SpawnOffset * this.DirectionVector;
        this._spawnTime = this.ImpactTime - (float) this.SpawnOffset / this.Speed;
      }

      public override void Init()
      {
        this.DirectionVector = this.Direction.ToVector();
        this.nSprite = new Sprite();
        this.nSprite.Texture = this.Texture;
        this._expiryTime = this.ImpactTime + ((Game.Settings.BaseResolution + this.Texture.GetSize()) * this.DirectionVector).Length() / this.Speed;
        this.nSprite.Visible = false;
        switch ((byte) this.Direction)
        {
          case 1:
            this.nSprite.RotationDegrees = 180f;
            this.nSprite.FlipV = true;
            break;
          case 2:
            this.nSprite.RotationDegrees = -90f;
            this.nSprite.FlipV = true;
            break;
          case 8:
            this.nSprite.RotationDegrees = 90f;
            break;
        }
        this._battle.MainLayer.AddChild((Node) this.nSprite);
      }

      public override void Process(float time)
      {
        if (this.ShouldDelete)
          return;
        if (!this.nSprite.Visible && (double) time >= (double) this._spawnTime)
          this.nSprite.Visible = true;
        if (this.nSprite.Visible)
          this.nSprite.Position = this._origin - this.Speed * (this.ImpactTime - time) * this.DirectionVector;
        this._battle.DamagePanelAtPixel(this.nSprite.Position);
        if ((double) time < (double) this._expiryTime)
          return;
        this.nSprite.QueueFree();
        this.ShouldDelete = true;
      }
    }

    public class KnifePanelAttackStandard : BasicPanelAttack
    {
      public KnifePanelAttackStandard(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 6;
        this.Vframes = 1;
        this.Frames = "0,0,0,0,0,0,1,2,3,4,5";
        this.Fps = 10f;
        this.Loop = false;
        this.StartOffset = 0.6f;
        this.DamageDuration = 0.3f;
        this.Lifetime = 0.5f;
        this.Texture = battle.texKnifePanel;
        this.TextureOffset = new Vector2(0.0f, -76f);
      }
    }

    public class IncineratePanelAttack : BasicPanelAttack
    {
      public IncineratePanelAttack(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 8;
        this.Vframes = 1;
        this.Frames = "0,1,2,3,4,5,6,7";
        this.Fps = 10f;
        this.Loop = true;
        this.StartOffset = 0.0f;
        this.DamageDuration = 1f;
        this.Lifetime = 1f;
        this.Texture = battle.texFire;
        this.TextureOffset = new Vector2(0.0f, -16f);
      }
    }

    public class IncineratePanelAttackQuick : BasicPanelAttack
    {
      public IncineratePanelAttackQuick(Ch1MinigameBattleMk battle, int col, int row, float time)
        : this(battle)
      {
        this.X = col;
        this.Y = row;
        this.ImpactTime = time;
      }

      public IncineratePanelAttackQuick(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 8;
        this.Vframes = 1;
        this.Frames = "0,1,2,3,4,5,6,7";
        this.Fps = 10f;
        this.Loop = true;
        this.StartOffset = 0.0f;
        this.DamageDuration = 0.25f;
        this.Lifetime = 0.3f;
        this.Texture = battle.texFire;
        this.TextureOffset = new Vector2(0.0f, -16f);
      }
    }

    public class IncineratePlayerPanelAttack2 : Ch1MinigameBattleMk.IncineratePlayerPanelAttack
    {
      public IncineratePlayerPanelAttack2(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 8;
        this.Vframes = 1;
        this.Frames = "0,1,2,3,4,5,6,7";
        this.Fps = 10f;
        this.Loop = true;
        this.StartOffset = 0.0f;
        this.ImpactTime = 1f;
        this.DamageDuration = 1.5f;
        this.Lifetime = 1f;
        this.Texture = battle.texFire;
        this.TextureOffset = new Vector2(0.0f, -16f);
      }
    }

    public class IncineratePlayerPanelAttack : PlayerPanelAttack
    {
      private SimpleAnimatedSprite _extraSprL;
      private SimpleAnimatedSprite _extraSprU;
      private SimpleAnimatedSprite _extraSprR;
      private SimpleAnimatedSprite _extraSprD;

      public override void Process(float time)
      {
        if (this.ShouldDelete)
          return;
        if (!this._sprite.Visible && (double) time >= (double) this._spawnTime)
        {
          this._battle.Cells[this._x, this._y].Sprite.AddChild((Node) this._sprite);
          this._sprite.Visible = true;
          if (this._battle.CellIsValid(this._x - 1, this._y))
          {
            this._battle.Cells[this._x - 1, this._y].Sprite.AddChild((Node) this._extraSprL);
            this._extraSprL.Visible = true;
          }
          if (this._battle.CellIsValid(this._x, this._y - 1))
          {
            this._battle.Cells[this._x, this._y - 1].Sprite.AddChild((Node) this._extraSprU);
            this._extraSprU.Visible = true;
          }
          if (this._battle.CellIsValid(this._x + 1, this._y))
          {
            this._battle.Cells[this._x + 1, this._y].Sprite.AddChild((Node) this._extraSprR);
            this._extraSprR.Visible = true;
          }
          if (this._battle.CellIsValid(this._x, this._y + 1))
          {
            this._battle.Cells[this._x, this._y + 1].Sprite.AddChild((Node) this._extraSprD);
            this._extraSprD.Visible = true;
          }
        }
        if ((double) time >= (double) this.ImpactTime && (double) time < (double) this._dmgExpiryTime)
        {
          this._battle.DamagePanel(this._x, this._y);
          this._battle.DamagePanel(this._x - 1, this._y);
          this._battle.DamagePanel(this._x + 1, this._y);
          this._battle.DamagePanel(this._x, this._y - 1);
          this._battle.DamagePanel(this._x, this._y + 1);
        }
        if ((double) time >= (double) this._expiryTime)
        {
          this._sprite.DeleteIfValid();
          this._extraSprL.DeleteIfValid();
          this._extraSprU.DeleteIfValid();
          this._extraSprR.DeleteIfValid();
          this._extraSprD.DeleteIfValid();
          this.ShouldDelete = true;
        }
        if (this.ShouldDelete)
          return;
        base.Process(time);
      }

      public override void Init()
      {
        this._x = this._battle._playerX;
        this._y = this._battle._playerY;
        this._sprite = (Sprite) this.MakeAnimatedSprite();
        this._extraSprL = this.MakeAnimatedSprite();
        this._extraSprU = this.MakeAnimatedSprite();
        this._extraSprR = this.MakeAnimatedSprite();
        this._extraSprD = this.MakeAnimatedSprite();
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x, this._y, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x - 1, this._y, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x + 1, this._y, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x, this._y - 1, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x, this._y + 1, (double) this.ImpactTime);
      }
    }

    public class IncineratePanelAttackInfinite : BasicPanelAttack
    {
      public IncineratePanelAttackInfinite(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 8;
        this.Vframes = 1;
        this.Frames = "0,1,2,3,4,5,6,7";
        this.Fps = 10f;
        this.Loop = true;
        this.StartOffset = 0.0f;
        this.DamageDuration = 999f;
        this.Lifetime = 999f;
        this.Texture = battle.texFire;
        this.TextureOffset = new Vector2(0.0f, -16f);
      }
    }

    public class KnifePlayerPanelAttackStandard : Ch1MinigameBattleMk.StabPanelAttack
    {
      public KnifePlayerPanelAttackStandard(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 6;
        this.Vframes = 1;
        this.Frames = "0,0,0,0,0,0,1,2,3,4,5";
        this.Fps = 10f;
        this.Loop = false;
        this.StartOffset = 0.6f;
        this.DamageDuration = 0.15f;
        this.Lifetime = 0.5f;
        this.Texture = battle.texKnifePanel;
        this.TextureOffset = new Vector2(0.0f, -76f);
      }
    }

    public class KnifeSlashAttackStandard : Ch1MinigameBattleMk.SlashAttack
    {
      public KnifeSlashAttackStandard(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 6;
        this.Vframes = 1;
        this.Frames = "0,0,0,0,1,1,2,3,4,5,5,5,5";
        this.Fps = 20f;
        this.Loop = false;
        this.StartOffset = 0.3f;
        this.DamageDuration = 0.15f;
        this.Lifetime = 0.5f;
        this.Texture = battle.texKnifeSlash;
        this.TextureOffset = new Vector2(0.0f, 0.0f);
      }
    }

    public class KnifeSlashAttackTracking : Ch1MinigameBattleMk.TrackingSlashAttack
    {
      public KnifeSlashAttackTracking(Ch1MinigameBattleMk battle)
      {
        this.Hframes = 6;
        this.Vframes = 1;
        this.Frames = "0,0,0,0,1,1,2,3,4,5,5,5,5";
        this.Fps = 20f;
        this.Loop = false;
        this.StartOffset = 0.3f;
        this.DamageDuration = 0.15f;
        this.Lifetime = 0.5f;
        this.Texture = battle.texKnifeSlash;
        this.TextureOffset = new Vector2(0.0f, 0.0f);
      }
    }

    public class SlashAttack : BasicPanelAttack
    {
      public override void Process(float time)
      {
        if (this.ShouldDelete)
          return;
        base.Process(time);
        if ((double) time < (double) this.ImpactTime || (double) time >= (double) this._dmgExpiryTime)
          return;
        this._battle.DamagePanel(this.X - 1, this.Y);
        this._battle.DamagePanel(this.X + 1, this.Y);
        this._battle.DamagePanel(this.X - 1, this.Y + 1);
        this._battle.DamagePanel(this.X, this.Y + 1);
        this._battle.DamagePanel(this.X + 1, this.Y + 1);
      }
    }

    public class StabPanelAttack : PlayerPanelAttack
    {
      public override void Init()
      {
        this._x = this._battle._playerX;
        this._y = this._battle._playerY;
        this._sprite = (Sprite) this.MakeAnimatedSprite();
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x, this._y, (double) this.ImpactTime);
      }
    }

    public class TrackingSlashAttack : PlayerPanelAttack
    {
      public override void Init()
      {
        this._x = Math.Clamp(this._battle._playerX, 1, this._battle.Width - 2);
        this._y = Math.Clamp(this._battle._playerY, 0, this._battle.Height - 2);
        this._sprite = (Sprite) this.MakeAnimatedSprite();
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x - 1, this._y, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x, this._y, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x + 1, this._y, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x - 1, this._y + 1, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x, this._y + 1, (double) this.ImpactTime);
        ((Ch1MinigameBattleMk) this._battle).MakeTelegraph(this._x + 1, this._y + 1, (double) this.ImpactTime);
      }

      public override void Process(float time)
      {
        if (this.ShouldDelete)
          return;
        base.Process(time);
        if ((double) time < (double) this.ImpactTime || (double) time >= (double) this._dmgExpiryTime)
          return;
        this._battle.DamagePanel(this._x - 1, this._y);
        this._battle.DamagePanel(this._x + 1, this._y);
        this._battle.DamagePanel(this._x - 1, this._y + 1);
        this._battle.DamagePanel(this._x, this._y + 1);
        this._battle.DamagePanel(this._x + 1, this._y + 1);
      }
    }
  }
}
