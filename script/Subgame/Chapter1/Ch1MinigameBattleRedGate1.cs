// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleRedGate1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1MinigameBattleRedGate1 : Ch1MinigameBattleRed
  {
    public override void Init()
    {
      base.Init();
      float time1 = 2f;
      this.MakeTelegraph(0, 0, (double) time1);
      this.MakeTelegraph(2, 1, (double) time1 + 0.6);
      this.MakeTelegraph(1, 2, (double) time1 + 0.6 + 0.6);
      this.MakeTelegraph(1, 0, (double) time1 + 0.6 + 0.6 + 0.6);
      float time2 = time1 + 2.2f;
      this.LongSpikesOnPanel(0, 0, (double) time2);
      this.LongSpikesOnPanel(2, 1, (double) time2);
      this.LongSpikesOnPanel(1, 2, (double) time2);
      this.LongSpikesOnPanel(1, 0, (double) time2);
    }
  }
}
