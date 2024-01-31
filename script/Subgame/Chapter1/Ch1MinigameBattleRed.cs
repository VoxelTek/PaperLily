// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleRed
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1MinigameBattleRed : Battle
  {
    [Export(PropertyHint.None, "")]
    public Texture texSpikes;
    [Export(PropertyHint.None, "")]
    public AudioStream sfxSpikes;
    public const float SPIKE_DURATION = 0.8f;

    public void AttackColumn2(int column, double time, bool invert = false)
    {
      Ch1MinigameBattleRed.SpikeAttackStandard process = new Ch1MinigameBattleRed.SpikeAttackStandard(this);
      process.X = column;
      process.Y = invert ? 3 : 0;
      process.ImpactTime = (float) time;
      this.AddProcess((Battle.BattleProcess) process);
    }

    public void SpikesOnPanel(int column, int row, double time)
    {
      Ch1MinigameBattleRed.SpikeAttackStandard process = new Ch1MinigameBattleRed.SpikeAttackStandard(this);
      process.X = column;
      process.Y = row;
      process.ImpactTime = (float) time;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxSpikes, (float) time - 0.2f, 0.4f));
    }

    public void SpikesOnRow(int row, double time)
    {
      for (int index = 0; index < this.Height; ++index)
      {
        Ch1MinigameBattleRed.SpikeAttackStandard process = new Ch1MinigameBattleRed.SpikeAttackStandard(this);
        process.X = index;
        process.Y = row;
        process.ImpactTime = (float) time;
        this.AddProcess((Battle.BattleProcess) process);
      }
      this.AddSound(new Battle.BattleSound(this.sfxSpikes, (float) time - 0.2f, 0.4f));
    }

    public void SpikesOnColumn(int column, double time)
    {
      for (int index = 0; index < this.Width; ++index)
      {
        Ch1MinigameBattleRed.SpikeAttackStandard process = new Ch1MinigameBattleRed.SpikeAttackStandard(this);
        process.X = column;
        process.Y = index;
        process.ImpactTime = (float) time;
        this.AddProcess((Battle.BattleProcess) process);
      }
      this.AddSound(new Battle.BattleSound(this.sfxSpikes, (float) time - 0.2f, 0.4f));
    }

    public void LongSpikesOnPanel(int column, int row, double time)
    {
      Ch1MinigameBattleRed.SpikeAttackInfinite process = new Ch1MinigameBattleRed.SpikeAttackInfinite(this);
      process.X = column;
      process.Y = row;
      process.ImpactTime = (float) time;
      this.AddProcess((Battle.BattleProcess) process);
      this.AddSound(new Battle.BattleSound(this.sfxSpikes, (float) time - 0.2f, 0.4f));
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
        Duration = 0.5f,
        Offset = 0.5f
      });
    }

    public class SpikeAttackStandard : BasicPanelAttack
    {
      public SpikeAttackStandard(Ch1MinigameBattleRed battle)
      {
        this.Hframes = 5;
        this.Vframes = 1;
        this.Frames = "0,0,0,0,0,0,1,2,3,4,3";
        this.Fps = 15f;
        this.Loop = false;
        this.StartOffset = 0.59f;
        this.Lifetime = 0.2f;
        this.DamageDuration = 0.1f;
        this.Texture = battle.texSpikes;
      }
    }

    public class SpikeAttackInfinite : BasicPanelAttack
    {
      public SpikeAttackInfinite(Ch1MinigameBattleRed battle)
      {
        this.Hframes = 5;
        this.Vframes = 1;
        this.Frames = "0,0,0,0,0,0,1,2,3,4,3";
        this.Fps = 15f;
        this.Loop = false;
        this.StartOffset = 0.59f;
        this.Lifetime = 30f;
        this.DamageDuration = 30f;
        this.Texture = battle.texSpikes;
      }
    }
  }
}
