// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleRedBridge3
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1MinigameBattleRedBridge3 : Ch1MinigameBattleRed
  {
    private List<double> PlayerPanelAttacks = new List<double>();

    public override void Init()
    {
      base.Init();
      float time1 = 2f;
      this.SpikesOnPanel(1, 1, (double) time1);
      this.SpikesOnColumn(0, (double) time1 + 1.0);
      this.SpikesOnColumn(2, (double) time1 + 1.0);
      this.SpikesOnPanel(1, 2, (double) time1 + 1.0);
      this.SpikesOnPanel(1, 0, (double) time1 + 1.0);
      this.PlayerPanelAttacks.Add(4.0);
      this.PlayerPanelAttacks.Add(4.5);
      this.PlayerPanelAttacks.Add(5.0);
      this.PlayerPanelAttacks.Add(5.5);
      this.PlayerPanelAttacks.Add(6.0);
      this.PlayerPanelAttacks.Add(6.5);
      this.PlayerPanelAttacks.Add(7.0);
      this.PlayerPanelAttacks.Add(7.5);
      this.PlayerPanelAttacks.Add(8.0);
      this.PlayerPanelAttacks.Add(8.5);
      this.PlayerPanelAttacks.Add(9.0);
      this.PlayerPanelAttacks.Add(9.5);
      float time2 = 11.5f;
      this.SpikesOnColumn(1, (double) time2);
      this.SpikesOnColumn(2, (double) time2);
      this.SpikesOnPanel(0, 1, (double) time2);
      this.SpikesOnPanel(0, 2, (double) time2);
      this.MakeSpacedTelegraph(0, 1, (double) time2);
      this.MakeSpacedTelegraph(0, 2, (double) time2);
      this.MakeSpacedTelegraph(1, 0, (double) time2);
      this.MakeSpacedTelegraph(1, 1, (double) time2);
      this.MakeSpacedTelegraph(1, 2, (double) time2);
      this.MakeSpacedTelegraph(2, 0, (double) time2);
      this.MakeSpacedTelegraph(2, 1, (double) time2);
      this.MakeSpacedTelegraph(2, 2, (double) time2);
      float time3 = 13f;
      this.SpikesOnColumn(0, (double) time3);
      this.SpikesOnColumn(1, (double) time3);
      this.SpikesOnPanel(2, 0, (double) time3);
      this.SpikesOnPanel(2, 1, (double) time3);
      this.MakeSpacedTelegraph(0, 0, (double) time3);
      this.MakeSpacedTelegraph(0, 1, (double) time3);
      this.MakeSpacedTelegraph(0, 2, (double) time3);
      this.MakeSpacedTelegraph(1, 0, (double) time3);
      this.MakeSpacedTelegraph(1, 1, (double) time3);
      this.MakeSpacedTelegraph(1, 2, (double) time3);
      this.MakeSpacedTelegraph(2, 0, (double) time3);
      this.MakeSpacedTelegraph(2, 1, (double) time3);
    }

    public void MakeSpacedTelegraph(int x, int y, double time)
    {
      if (x < 0 || y < 0 || x >= this.Width || y >= this.Height)
        return;
      this.AddProcess((Battle.BattleProcess) new BasicTelegraph()
      {
        X = x,
        Y = y,
        Time = (float) time,
        Duration = 0.5f,
        Offset = 1f
      });
    }

    public override void _Process(float delta)
    {
      base._Process(delta);
      foreach (double playerPanelAttack in this.PlayerPanelAttacks)
      {
        if (playerPanelAttack - 0.800000011920929 - this._elapsed <= 0.0)
          this.SpikesOnPanel(this._playerX, this._playerY, playerPanelAttack);
      }
      this.PlayerPanelAttacks.RemoveAll((Predicate<double>) (triggerTime => triggerTime - 0.800000011920929 - this._elapsed <= 0.0));
    }
  }
}
