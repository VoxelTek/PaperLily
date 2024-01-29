using Godot;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	public class BasicPanelAttack : Battle.BattleProcess
	{
		public Texture Texture;

		public Vector2 TextureOffset = Vector2.Zero;

		public int X;

		public int Y;

		public float ImpactTime;

		public int Hframes;

		public int Vframes;

		public string Frames;

		public float Fps;

		public bool Loop;

		public float StartOffset;

		public float DamageDuration;

		public float Lifetime;

		protected int _id;

		protected Battle _battle;

		protected float _spawnTime;

		protected float _dmgExpiryTime;

		protected float _expiryTime;

		private Sprite nSprite;

		public override int Id => _id;

		public override float InitTime => _spawnTime;

		public override void PreInit(Battle battle)
		{
			_battle = battle;
			_id = _battle.CurAttackId;
			_spawnTime = ImpactTime - StartOffset;
			_dmgExpiryTime = ImpactTime + DamageDuration;
			_expiryTime = ImpactTime + Lifetime;
		}

		public override void Init()
		{
			nSprite = MakeAnimatedSprite();
			_battle.Cells[X, Y].Sprite.AddChild(nSprite);
		}

		public override void Process(float time)
		{
			if (!base.ShouldDelete)
			{
				if (!nSprite.Visible && time >= _spawnTime)
				{
					nSprite.Visible = true;
				}
				if (time >= ImpactTime && time < _dmgExpiryTime)
				{
					_battle.DamagePanel(X, Y);
				}
				if (time >= _expiryTime)
				{
					nSprite.QueueFree();
					base.ShouldDelete = true;
				}
			}
		}

		protected SimpleAnimatedSprite MakeAnimatedSprite()
		{
			SimpleAnimatedSprite simpleAnimatedSprite = GDUtil.MakeNode<SimpleAnimatedSprite>("Attack");
			simpleAnimatedSprite.Texture = Texture;
			simpleAnimatedSprite.Offset = TextureOffset;
			simpleAnimatedSprite.Hframes = Hframes;
			simpleAnimatedSprite.Vframes = Vframes;
			simpleAnimatedSprite.FPS = Fps;
			simpleAnimatedSprite.Loop = Loop;
			simpleAnimatedSprite.AnimationFrames = Frames;
			simpleAnimatedSprite.Visible = false;
			return simpleAnimatedSprite;
		}
	}
}
