using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	public class PlayerPanelAttack : Battle.BattleProcess
	{
		public Texture Texture;

		public Vector2 TextureOffset = Vector2.Zero;

		public float ImpactTime;

		public int Hframes;

		public int Vframes;

		public string Frames;

		public float Fps;

		public bool Loop;

		public float StartOffset;

		public float PositionCalculationOffset;

		public float DamageDuration;

		public float Lifetime;

		protected int _id;

		protected int _x;

		protected int _y;

		protected Battle _battle;

		protected float _spawnTime;

		protected float _dmgExpiryTime;

		protected float _expiryTime;

		protected float _initTime;

		protected Sprite _sprite;

		public override int Id => _id;

		public override float InitTime => _initTime;

		public override void PreInit(Battle battle)
		{
			_battle = battle;
			_id = _battle.CurAttackId;
			_spawnTime = ImpactTime - StartOffset;
			_dmgExpiryTime = ImpactTime + DamageDuration;
			_expiryTime = ImpactTime + Lifetime;
			_initTime = Math.Min(_spawnTime, ImpactTime - PositionCalculationOffset);
		}

		public override void Init()
		{
			_x = _battle._playerX;
			_y = _battle._playerY;
			_sprite = MakeAnimatedSprite();
		}

		public override void Process(float time)
		{
			if (!base.ShouldDelete)
			{
				if (!_sprite.Visible && time >= _spawnTime)
				{
					_battle.Cells[_x, _y].Sprite.AddChild(_sprite);
					_sprite.Visible = true;
				}
				if (time >= ImpactTime && time < _dmgExpiryTime)
				{
					_battle.DamagePanel(_x, _y);
				}
				if (time >= _expiryTime)
				{
					_sprite.DeleteIfValid();
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
