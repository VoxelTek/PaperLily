using System.Collections.Generic;

namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleRedBridge3 : Ch1MinigameBattleRed
	{
		private List<double> PlayerPanelAttacks = new List<double>();

		public override void Init()
		{
			base.Init();
			float t = 2f;
			SpikesOnPanel(1, 1, t);
			SpikesOnColumn(0, t + 1f);
			SpikesOnColumn(2, t + 1f);
			SpikesOnPanel(1, 2, t + 1f);
			SpikesOnPanel(1, 0, t + 1f);
			PlayerPanelAttacks.Add(4.0);
			PlayerPanelAttacks.Add(4.5);
			PlayerPanelAttacks.Add(5.0);
			PlayerPanelAttacks.Add(5.5);
			PlayerPanelAttacks.Add(6.0);
			PlayerPanelAttacks.Add(6.5);
			PlayerPanelAttacks.Add(7.0);
			PlayerPanelAttacks.Add(7.5);
			PlayerPanelAttacks.Add(8.0);
			PlayerPanelAttacks.Add(8.5);
			PlayerPanelAttacks.Add(9.0);
			PlayerPanelAttacks.Add(9.5);
			t = 11.5f;
			SpikesOnColumn(1, t);
			SpikesOnColumn(2, t);
			SpikesOnPanel(0, 1, t);
			SpikesOnPanel(0, 2, t);
			MakeSpacedTelegraph(0, 1, t);
			MakeSpacedTelegraph(0, 2, t);
			MakeSpacedTelegraph(1, 0, t);
			MakeSpacedTelegraph(1, 1, t);
			MakeSpacedTelegraph(1, 2, t);
			MakeSpacedTelegraph(2, 0, t);
			MakeSpacedTelegraph(2, 1, t);
			MakeSpacedTelegraph(2, 2, t);
			t = 13f;
			SpikesOnColumn(0, t);
			SpikesOnColumn(1, t);
			SpikesOnPanel(2, 0, t);
			SpikesOnPanel(2, 1, t);
			MakeSpacedTelegraph(0, 0, t);
			MakeSpacedTelegraph(0, 1, t);
			MakeSpacedTelegraph(0, 2, t);
			MakeSpacedTelegraph(1, 0, t);
			MakeSpacedTelegraph(1, 1, t);
			MakeSpacedTelegraph(1, 2, t);
			MakeSpacedTelegraph(2, 0, t);
			MakeSpacedTelegraph(2, 1, t);
		}

		public void MakeSpacedTelegraph(int x, int y, double time)
		{
			if (x >= 0 && y >= 0 && x < Width && y < Height)
			{
				AddProcess(new BasicTelegraph
				{
					X = x,
					Y = y,
					Time = (float)time,
					Duration = 0.5f,
					Offset = 1f
				});
			}
		}

		public override void _Process(float delta)
		{
			base._Process(delta);
			foreach (double triggerTime2 in PlayerPanelAttacks)
			{
				if (triggerTime2 - 0.800000011920929 - base._elapsed <= 0.0)
				{
					SpikesOnPanel(base._playerX, base._playerY, triggerTime2);
				}
			}
			PlayerPanelAttacks.RemoveAll((double triggerTime) => triggerTime - 0.800000011920929 - base._elapsed <= 0.0);
		}
	}
}
