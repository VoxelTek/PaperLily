namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleMkIntro : Ch1MinigameBattleMk
	{
		public override void _Ready()
		{
			double t = 2.5;
			ThrowCol(0, t);
			ThrowCol(2, t);
			t += 1.2;
			ThrowCol(0, t);
			ThrowCol(1, t);
			t += 1.2;
			ThrowCol(1, t);
			ThrowCol(2, t);
			t += 1.2;
			ThrowCol(0, t);
			ThrowCol(1, t);
			t += 1.2;
			ThrowCol(0, t);
			ThrowCol(2, t);
			t += 2.0;
			AttackPlayerPanel(t);
			AttackPlayerPanel(t + 0.8);
			AttackPlayerPanel(t + 0.8 + 0.8);
			AttackPlayerPanel(t + 0.8 + 0.8 + 0.8);
			t += 4.0;
			SlashPanel(1, 1, t);
			SlashPanel(1, 0, t + 1.0);
			SlashPanel(1, 1, t + 2.0);
		}
	}
}
