// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleRedBridge2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1MinigameBattleRedBridge2 : Ch1MinigameBattleRed
  {
    public override void Init()
    {
      base.Init();
      float time1 = 2f;
      this.SpikesOnColumn(0, (double) time1);
      this.SpikesOnColumn(1, (double) time1);
      this.SpikesOnColumn(1, (double) time1 + 1.0);
      this.SpikesOnColumn(2, (double) time1 + 1.0);
      float time2 = 4f;
      this.SpikesOnRow(0, (double) time2);
      this.SpikesOnRow(1, (double) time2);
      this.SpikesOnRow(1, (double) time2 + 1.0);
      this.SpikesOnRow(2, (double) time2 + 1.0);
      float time3 = 6f;
      this.SpikesOnColumn(0, (double) time3);
      this.SpikesOnColumn(2, (double) time3);
      this.SpikesOnPanel(1, 2, (double) time3);
      this.SpikesOnPanel(1, 0, (double) time3);
      float time4 = 7f;
      this.SpikesOnColumn(1, (double) time4);
      this.SpikesOnPanel(0, 1, (double) time4);
      this.SpikesOnPanel(2, 1, (double) time4);
    }
  }
}
