// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleMkIntro
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1MinigameBattleMkIntro : Ch1MinigameBattleMk
  {
    public override void _Ready()
    {
      double time1 = 2.5;
      this.ThrowCol(0, time1);
      this.ThrowCol(2, time1);
      double time2 = time1 + 1.2;
      this.ThrowCol(0, time2);
      this.ThrowCol(1, time2);
      double time3 = time2 + 1.2;
      this.ThrowCol(1, time3);
      this.ThrowCol(2, time3);
      double time4 = time3 + 1.2;
      this.ThrowCol(0, time4);
      this.ThrowCol(1, time4);
      double time5 = time4 + 1.2;
      this.ThrowCol(0, time5);
      this.ThrowCol(2, time5);
      double time6 = time5 + 2.0;
      this.AttackPlayerPanel(time6);
      this.AttackPlayerPanel(time6 + 0.8);
      this.AttackPlayerPanel(time6 + 0.8 + 0.8);
      this.AttackPlayerPanel(time6 + 0.8 + 0.8 + 0.8);
      double time7 = time6 + 4.0;
      this.SlashPanel(1, 1, time7);
      this.SlashPanel(1, 0, time7 + 1.0);
      this.SlashPanel(1, 1, time7 + 2.0);
    }
  }
}
