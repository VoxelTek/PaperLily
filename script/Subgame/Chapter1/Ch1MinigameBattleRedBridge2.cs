namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleRedBridge2 : Ch1MinigameBattleRed
	{
		public override void Init()
		{
			base.Init();
			float t = 2f;
			SpikesOnColumn(0, t);
			SpikesOnColumn(1, t);
			SpikesOnColumn(1, t + 1f);
			SpikesOnColumn(2, t + 1f);
			t = 4f;
			SpikesOnRow(0, t);
			SpikesOnRow(1, t);
			SpikesOnRow(1, t + 1f);
			SpikesOnRow(2, t + 1f);
			t = 6f;
			SpikesOnColumn(0, t);
			SpikesOnColumn(2, t);
			SpikesOnPanel(1, 2, t);
			SpikesOnPanel(1, 0, t);
			t = 7f;
			SpikesOnColumn(1, t);
			SpikesOnPanel(0, 1, t);
			SpikesOnPanel(2, 1, t);
		}
	}
}
