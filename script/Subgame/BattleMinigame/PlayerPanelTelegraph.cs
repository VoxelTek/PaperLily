// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.PlayerPanelTelegraph
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Minigames
{
  public class PlayerPanelTelegraph : Battle.BattleProcess
  {
    public float Time;
    public float Duration;
    public int PanelOffsetX;
    public int PanelOffsetY;
    public float Offset;
    protected int _id;
    protected int _x;
    protected int _y;
    protected Battle _battle;
    protected float _spawnTime;
    protected float _expiryTime;

    public override int Id => this._id;

    public override float InitTime => this._spawnTime;

    public override void PreInit(Battle battle)
    {
      this._battle = battle;
      this._id = this._battle.CurAttackId;
      this._spawnTime = this.Time - this.Offset - this.Duration;
      this._expiryTime = this._spawnTime + this.Duration;
    }

    public override void Init()
    {
      this._x = this._battle._playerX + this.PanelOffsetX;
      this._y = this._battle._playerY + this.PanelOffsetY;
      if (this._x >= 0 && this._y >= 0 && this._x < this._battle.Width && this._y < this._battle.Height)
        return;
      this.ShouldDelete = true;
    }

    public override void Process(float time)
    {
      if (this.ShouldDelete)
        return;
      if ((double) time >= (double) this._spawnTime)
        this._battle.Cells[this._x, this._y].Telegraphing = true;
      if ((double) time < (double) this._expiryTime)
        return;
      this.ShouldDelete = true;
    }
  }
}
