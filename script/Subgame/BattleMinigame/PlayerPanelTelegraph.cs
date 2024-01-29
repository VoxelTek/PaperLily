namespace LacieEngine.Minigames
{
	public class PlayerPanelTelegraph : Battle.BattleProcess
	{
		public float Time;

		public float Duration;

		public int PanelOffsetX;

		public int PanelOffsetY;

		public float Offset;

		protected int _id;

		protected int _x;

		protected int _y;

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
			_x = _battle._playerX + PanelOffsetX;
			_y = _battle._playerY + PanelOffsetY;
			if (_x < 0 || _y < 0 || _x >= _battle.Width || _y >= _battle.Height)
			{
				base.ShouldDelete = true;
			}
		}

		public override void Process(float time)
		{
			if (!base.ShouldDelete)
			{
				if (time >= _spawnTime)
				{
					_battle.Cells[_x, _y].Telegraphing = true;
				}
				if (time >= _expiryTime)
				{
					base.ShouldDelete = true;
				}
			}
		}
	}
}
