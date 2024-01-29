namespace LacieEngine.Minigames
{
	public class BasicTelegraph : Battle.BattleProcess
	{
		public float Time;

		public int X;

		public int Y;

		public float Duration;

		public float Offset;

		protected int _id;

		protected Battle _battle;

		protected float _spawnTime;

		protected float _expiryTime;

		public override int Id => _id;

		public override float InitTime => _spawnTime;

		public override void PreInit(Battle battle)
		{
			_battle = battle;
			_id = _battle.CurAttackId;
			_spawnTime = Time - Offset - Duration;
			_expiryTime = _spawnTime + Duration;
		}

		public override void Init()
		{
		}

		public override void Process(float time)
		{
			if (!base.ShouldDelete)
			{
				if (time >= _spawnTime)
				{
					_battle.Cells[X, Y].Telegraphing = true;
				}
				if (time >= _expiryTime)
				{
					base.ShouldDelete = true;
				}
			}
		}
	}
}
