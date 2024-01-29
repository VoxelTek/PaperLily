namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleRedGate3 : Ch1MinigameBattleRed
	{
		public override void Init()
		{
			base.Init();
			float t = 2f;
			MakeTelegraph(2, 1, t);
			MakeTelegraph(1, 1, (double)t + 0.6);
			MakeTelegraph(0, 0, (double)t + 0.6 + 0.6);
			MakeTelegraph(1, 2, (double)t + 0.6 + 0.6 + 0.6);
			MakeTelegraph(1, 0, (double)t + 0.6 + 0.6 + 0.6 + 0.6);
			MakeTelegraph(2, 2, (double)t + 0.6 + 0.6 + 0.6 + 0.6 + 0.6);
			t += 3.4f;
			LongSpikesOnPanel(2, 1, t);
			LongSpikesOnPanel(1, 1, t);
			LongSpikesOnPanel(0, 0, t);
			LongSpikesOnPanel(1, 2, t);
			LongSpikesOnPanel(1, 0, t);
			LongSpikesOnPanel(2, 2, t);
		}
	}
}
