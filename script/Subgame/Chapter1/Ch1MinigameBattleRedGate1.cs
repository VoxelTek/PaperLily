namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleRedGate1 : Ch1MinigameBattleRed
	{
		public override void Init()
		{
			base.Init();
			float t = 2f;
			MakeTelegraph(0, 0, t);
			MakeTelegraph(2, 1, (double)t + 0.6);
			MakeTelegraph(1, 2, (double)t + 0.6 + 0.6);
			MakeTelegraph(1, 0, (double)t + 0.6 + 0.6 + 0.6);
			t += 2.2f;
			LongSpikesOnPanel(0, 0, t);
			LongSpikesOnPanel(2, 1, t);
			LongSpikesOnPanel(1, 2, t);
			LongSpikesOnPanel(1, 0, t);
		}
	}
}
