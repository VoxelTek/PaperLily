using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleMk : Battle
	{
		public class KnifeProjectileStandard : KnifeProjectile
		{
			public KnifeProjectileStandard(Ch1MinigameBattleMk battle)
			{
				Speed = 280f;
				SpawnOffset = 320;
				Texture = battle.texKnife;
			}
		}

		public class KnifeProjectileSlow : KnifeProjectile
		{
			public KnifeProjectileSlow(Ch1MinigameBattleMk battle)
			{
				Speed = 150f;
				SpawnOffset = 320;
				Texture = battle.texKnife;
			}
		}

		public class KnifeProjectile : BattleProcess
		{
			public int SpawnOffset;

			public float Speed;

			public Texture Texture;

			public int X;

			public int Y;

			public float ImpactTime;

			private Vector2 _origin;

			public Direction Direction;

			private Vector2 DirectionVector;

			protected int _id;

			private Battle _battle;

			private float _spawnTime;

			private float _expiryTime;

			private Sprite nSprite;

			public override int Id => _id;

			public override float InitTime => _spawnTime;

			public override void PreInit(Battle battle)
			{
				_battle = battle;
				_id = _battle.CurAttackId;
				_origin = _battle.Cells[X, Y].Sprite.Position - SpawnOffset * DirectionVector;
				_spawnTime = ImpactTime - (float)SpawnOffset / Speed;
			}

			public override void Init()
			{
				DirectionVector = Direction.ToVector();
				nSprite = new Sprite();
				nSprite.Texture = Texture;
				_expiryTime = ImpactTime + ((Game.Settings.BaseResolution + Texture.GetSize()) * DirectionVector).Length() / Speed;
				nSprite.Visible = false;
				switch ((byte)Direction)
				{
				case 1:
					nSprite.RotationDegrees = 180f;
					nSprite.FlipV = true;
					break;
				case 2:
					nSprite.RotationDegrees = -90f;
					nSprite.FlipV = true;
					break;
				case 8:
					nSprite.RotationDegrees = 90f;
					break;
				}
				_battle.MainLayer.AddChild(nSprite);
			}

			public override void Process(float time)
			{
				if (!base.ShouldDelete)
				{
					if (!nSprite.Visible && time >= _spawnTime)
					{
						nSprite.Visible = true;
					}
					if (nSprite.Visible)
					{
						nSprite.Position = _origin - Speed * (ImpactTime - time) * DirectionVector;
					}
					_battle.DamagePanelAtPixel(nSprite.Position);
					if (time >= _expiryTime)
					{
						nSprite.QueueFree();
						base.ShouldDelete = true;
					}
				}
			}
		}

		public class KnifePanelAttackStandard : BasicPanelAttack
		{
			public KnifePanelAttackStandard(Ch1MinigameBattleMk battle)
			{
				Hframes = 6;
				Vframes = 1;
				Frames = "0,0,0,0,0,0,1,2,3,4,5";
				Fps = 10f;
				Loop = false;
				StartOffset = 0.6f;
				DamageDuration = 0.3f;
				Lifetime = 0.5f;
				Texture = battle.texKnifePanel;
				TextureOffset = new Vector2(0f, -76f);
			}
		}

		public class IncineratePanelAttack : BasicPanelAttack
		{
			public IncineratePanelAttack(Ch1MinigameBattleMk battle)
			{
				Hframes = 8;
				Vframes = 1;
				Frames = "0,1,2,3,4,5,6,7";
				Fps = 10f;
				Loop = true;
				StartOffset = 0f;
				DamageDuration = 1f;
				Lifetime = 1f;
				Texture = battle.texFire;
				TextureOffset = new Vector2(0f, -16f);
			}
		}

		public class IncineratePanelAttackQuick : BasicPanelAttack
		{
			public IncineratePanelAttackQuick(Ch1MinigameBattleMk battle, int col, int row, float time)
				: this(battle)
			{
				X = col;
				Y = row;
				ImpactTime = time;
			}

			public IncineratePanelAttackQuick(Ch1MinigameBattleMk battle)
			{
				Hframes = 8;
				Vframes = 1;
				Frames = "0,1,2,3,4,5,6,7";
				Fps = 10f;
				Loop = true;
				StartOffset = 0f;
				DamageDuration = 0.25f;
				Lifetime = 0.3f;
				Texture = battle.texFire;
				TextureOffset = new Vector2(0f, -16f);
			}
		}

		public class IncineratePlayerPanelAttack2 : IncineratePlayerPanelAttack
		{
			public IncineratePlayerPanelAttack2(Ch1MinigameBattleMk battle)
			{
				Hframes = 8;
				Vframes = 1;
				Frames = "0,1,2,3,4,5,6,7";
				Fps = 10f;
				Loop = true;
				StartOffset = 0f;
				ImpactTime = 1f;
				DamageDuration = 1.5f;
				Lifetime = 1f;
				Texture = battle.texFire;
				TextureOffset = new Vector2(0f, -16f);
			}
		}

		public class IncineratePlayerPanelAttack : PlayerPanelAttack
		{
			private SimpleAnimatedSprite _extraSprL;

			private SimpleAnimatedSprite _extraSprU;

			private SimpleAnimatedSprite _extraSprR;

			private SimpleAnimatedSprite _extraSprD;

			public override void Process(float time)
			{
				if (base.ShouldDelete)
				{
					return;
				}
				if (!_sprite.Visible && time >= _spawnTime)
				{
					_battle.Cells[_x, _y].Sprite.AddChild(_sprite);
					_sprite.Visible = true;
					if (_battle.CellIsValid(_x - 1, _y))
					{
						_battle.Cells[_x - 1, _y].Sprite.AddChild(_extraSprL);
						_extraSprL.Visible = true;
					}
					if (_battle.CellIsValid(_x, _y - 1))
					{
						_battle.Cells[_x, _y - 1].Sprite.AddChild(_extraSprU);
						_extraSprU.Visible = true;
					}
					if (_battle.CellIsValid(_x + 1, _y))
					{
						_battle.Cells[_x + 1, _y].Sprite.AddChild(_extraSprR);
						_extraSprR.Visible = true;
					}
					if (_battle.CellIsValid(_x, _y + 1))
					{
						_battle.Cells[_x, _y + 1].Sprite.AddChild(_extraSprD);
						_extraSprD.Visible = true;
					}
				}
				if (time >= ImpactTime && time < _dmgExpiryTime)
				{
					_battle.DamagePanel(_x, _y);
					_battle.DamagePanel(_x - 1, _y);
					_battle.DamagePanel(_x + 1, _y);
					_battle.DamagePanel(_x, _y - 1);
					_battle.DamagePanel(_x, _y + 1);
				}
				if (time >= _expiryTime)
				{
					_sprite.DeleteIfValid();
					_extraSprL.DeleteIfValid();
					_extraSprU.DeleteIfValid();
					_extraSprR.DeleteIfValid();
					_extraSprD.DeleteIfValid();
					base.ShouldDelete = true;
				}
				if (!base.ShouldDelete)
				{
					base.Process(time);
				}
			}

			public override void Init()
			{
				_x = _battle._playerX;
				_y = _battle._playerY;
				_sprite = MakeAnimatedSprite();
				_extraSprL = MakeAnimatedSprite();
				_extraSprU = MakeAnimatedSprite();
				_extraSprR = MakeAnimatedSprite();
				_extraSprD = MakeAnimatedSprite();
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x, _y, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x - 1, _y, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x + 1, _y, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x, _y - 1, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x, _y + 1, ImpactTime);
			}
		}

		public class IncineratePanelAttackInfinite : BasicPanelAttack
		{
			public IncineratePanelAttackInfinite(Ch1MinigameBattleMk battle)
			{
				Hframes = 8;
				Vframes = 1;
				Frames = "0,1,2,3,4,5,6,7";
				Fps = 10f;
				Loop = true;
				StartOffset = 0f;
				DamageDuration = 999f;
				Lifetime = 999f;
				Texture = battle.texFire;
				TextureOffset = new Vector2(0f, -16f);
			}
		}

		public class KnifePlayerPanelAttackStandard : StabPanelAttack
		{
			public KnifePlayerPanelAttackStandard(Ch1MinigameBattleMk battle)
			{
				Hframes = 6;
				Vframes = 1;
				Frames = "0,0,0,0,0,0,1,2,3,4,5";
				Fps = 10f;
				Loop = false;
				StartOffset = 0.6f;
				DamageDuration = 0.15f;
				Lifetime = 0.5f;
				Texture = battle.texKnifePanel;
				TextureOffset = new Vector2(0f, -76f);
			}
		}

		public class KnifeSlashAttackStandard : SlashAttack
		{
			public KnifeSlashAttackStandard(Ch1MinigameBattleMk battle)
			{
				Hframes = 6;
				Vframes = 1;
				Frames = "0,0,0,0,1,1,2,3,4,5,5,5,5";
				Fps = 20f;
				Loop = false;
				StartOffset = 0.3f;
				DamageDuration = 0.15f;
				Lifetime = 0.5f;
				Texture = battle.texKnifeSlash;
				TextureOffset = new Vector2(0f, 0f);
			}
		}

		public class KnifeSlashAttackTracking : TrackingSlashAttack
		{
			public KnifeSlashAttackTracking(Ch1MinigameBattleMk battle)
			{
				Hframes = 6;
				Vframes = 1;
				Frames = "0,0,0,0,1,1,2,3,4,5,5,5,5";
				Fps = 20f;
				Loop = false;
				StartOffset = 0.3f;
				DamageDuration = 0.15f;
				Lifetime = 0.5f;
				Texture = battle.texKnifeSlash;
				TextureOffset = new Vector2(0f, 0f);
			}
		}

		public class SlashAttack : BasicPanelAttack
		{
			public override void Process(float time)
			{
				if (!base.ShouldDelete)
				{
					base.Process(time);
					if (time >= ImpactTime && time < _dmgExpiryTime)
					{
						_battle.DamagePanel(X - 1, Y);
						_battle.DamagePanel(X + 1, Y);
						_battle.DamagePanel(X - 1, Y + 1);
						_battle.DamagePanel(X, Y + 1);
						_battle.DamagePanel(X + 1, Y + 1);
					}
				}
			}
		}

		public class StabPanelAttack : PlayerPanelAttack
		{
			public override void Init()
			{
				_x = _battle._playerX;
				_y = _battle._playerY;
				_sprite = MakeAnimatedSprite();
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x, _y, ImpactTime);
			}
		}

		public class TrackingSlashAttack : PlayerPanelAttack
		{
			public override void Init()
			{
				_x = Math.Clamp(_battle._playerX, 1, _battle.Width - 2);
				_y = Math.Clamp(_battle._playerY, 0, _battle.Height - 2);
				_sprite = MakeAnimatedSprite();
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x - 1, _y, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x, _y, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x + 1, _y, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x - 1, _y + 1, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x, _y + 1, ImpactTime);
				((Ch1MinigameBattleMk)_battle).MakeTelegraph(_x + 1, _y + 1, ImpactTime);
			}

			public override void Process(float time)
			{
				if (!base.ShouldDelete)
				{
					base.Process(time);
					if (time >= ImpactTime && time < _dmgExpiryTime)
					{
						_battle.DamagePanel(_x - 1, _y);
						_battle.DamagePanel(_x + 1, _y);
						_battle.DamagePanel(_x - 1, _y + 1);
						_battle.DamagePanel(_x, _y + 1);
						_battle.DamagePanel(_x + 1, _y + 1);
					}
				}
			}
		}

		[Export(PropertyHint.None, "")]
		private Texture texKnife;

		[Export(PropertyHint.None, "")]
		private Texture texKnifePanel;

		[Export(PropertyHint.None, "")]
		private Texture texKnifeSlash;

		[Export(PropertyHint.None, "")]
		private Texture texFire;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxStab;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxSlash;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxThrow;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxFire;

		private Random rnd = new Random();

		private const int KNIFE_SPAWN_OFFSET = 320;

		private const float TELEGRAPH_DURATION = 1f;

		private const float TELEGRAPH_OFFSET = 0f;

		public void RandomFallPattern(double t, out double finalT)
		{
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5 + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5 + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5 + 0.5 + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5 + 0.5 + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5 + 0.5 + 0.5 + 0.5);
			AttackPanel(rnd.Next(Width), rnd.Next(Height), t + 0.5 + 0.5 + 0.5 + 0.5);
			finalT = t + 3.0;
		}

		public void RandomSlashPattern(double t, out double finalT)
		{
			SlashPanel(rnd.Next(1, Width - 1), rnd.Next(Height - 1), t);
			SlashPanel(rnd.Next(1, Width - 1), rnd.Next(Height - 1), t + 0.75);
			SlashPanel(rnd.Next(1, Width - 1), rnd.Next(Height - 1), t + 0.75 + 0.75);
			SlashPanel(rnd.Next(1, Width - 1), rnd.Next(Height - 1), t + 0.75 + 0.75 + 0.75);
			SlashPanel(rnd.Next(1, Width - 1), rnd.Next(Height - 1), t + 0.75 + 0.75 + 0.75 + 0.75);
			SlashPanel(rnd.Next(1, Width - 1), rnd.Next(Height - 1), t + 0.75 + 0.75 + 0.75 + 0.75 + 0.75);
			finalT = t + 4.0;
		}

		public void SlashMPattern(double t, out double finalT)
		{
			SlashPanel(3, 0, t);
			SlashPanel(1, Height - 2, t);
			SlashPanel(Width - 2, Height - 2, t);
			t += 1.0;
			SlashPanel(1, 0, t);
			SlashPanel(Width - 1 - 1, 0, t);
			SlashPanel(3, Height - 1 - 1, t);
			finalT = t + 2.0;
		}

		public void HorizontalSlowPattern(double t, int count, out double finalT, bool invert = false)
		{
			for (int i = 0; i < count; i++)
			{
				int excludeRow = rnd.Next(Height);
				for (int j = 0; j < Height; j++)
				{
					if (j != excludeRow)
					{
						ThrowRowSlow(j, t, invert);
					}
				}
				t += 1.2;
			}
			finalT = t;
		}

		public void HorizontalSlowPatternDouble(double t, int count, out double finalT)
		{
			int prevExcludeRow = 0;
			int excludeRow = prevExcludeRow;
			for (int i = 0; i < count; i++)
			{
				while (excludeRow == prevExcludeRow)
				{
					excludeRow = rnd.Next(Height);
				}
				prevExcludeRow = excludeRow;
				for (int j = 0; j < Height; j++)
				{
					if (j != excludeRow)
					{
						ThrowRowSlow(j, t, invert: true);
						ThrowRowSlow(j, t);
					}
				}
				t += 1.2;
			}
			finalT = t;
		}

		public void VerticalSlowPattern(double t, int count, out double finalT)
		{
			for (int i = 0; i < count; i++)
			{
				int excludeCol = rnd.Next(1, Width - 1);
				for (int j = 0; j < Width; j++)
				{
					if (j != excludeCol)
					{
						ThrowColSlow(j, t);
					}
				}
				t += 2.0;
			}
			finalT = t;
		}

		public void DoubleSlowPattern(double t, int count, out double finalT)
		{
			bool invert = false;
			for (int i = 0; i < count; i++)
			{
				HorizontalSlowPattern(t, 1, out var finalT2, invert);
				VerticalSlowPattern(t, 1, out finalT2);
				t += 2.2;
				invert = !invert;
			}
			finalT = t;
		}

		public void ThrowCol(int column, double time, bool invert = false)
		{
			KnifeProjectileStandard atk = new KnifeProjectileStandard(this)
			{
				X = column,
				Y = (invert ? (Height - 1) : 0),
				ImpactTime = (float)time,
				Direction = (invert ? Direction.Up : Direction.Down)
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxThrow, (float)time - 1f, 1.5f));
		}

		public void BurnCol(int column, double time)
		{
			for (int i = 0; i < Height; i++)
			{
				IncineratePanelAttackInfinite atk = new IncineratePanelAttackInfinite(this)
				{
					X = column,
					Y = i,
					ImpactTime = (float)time
				};
				AddProcess(atk);
				AddSound(new BattleSound(sfxFire, (float)time));
			}
		}

		public void BurnRow(int row, double time)
		{
			for (int i = 0; i < Width; i++)
			{
				IncineratePanelAttackInfinite atk = new IncineratePanelAttackInfinite(this)
				{
					X = i,
					Y = row,
					ImpactTime = (float)time
				};
				AddProcess(atk);
				AddSound(new BattleSound(sfxFire, (float)time));
			}
		}

		public void BurnPlayerPanel(double time)
		{
			IncineratePlayerPanelAttack2 atk = new IncineratePlayerPanelAttack2(this)
			{
				ImpactTime = (float)time,
				PositionCalculationOffset = 1f
			};
			AddProcess(atk);
		}

		public void BurnPanel(int column, int row, double time)
		{
			IncineratePanelAttack atk = new IncineratePanelAttack(this)
			{
				X = column,
				Y = row,
				ImpactTime = (float)time
			};
			IncineratePanelAttack atkL = new IncineratePanelAttack(this)
			{
				X = column,
				Y = row - 1,
				ImpactTime = (float)time + 0.5f
			};
			IncineratePanelAttack atkU = new IncineratePanelAttack(this)
			{
				X = column - 1,
				Y = row,
				ImpactTime = (float)time + 0.5f
			};
			IncineratePanelAttack atkR = new IncineratePanelAttack(this)
			{
				X = column + 1,
				Y = row,
				ImpactTime = (float)time + 0.5f
			};
			IncineratePanelAttack atkD = new IncineratePanelAttack(this)
			{
				X = column,
				Y = row + 1,
				ImpactTime = (float)time + 0.5f
			};
			MakeTelegraph(column, row, time);
			MakeTelegraph(column + 1, row, time);
			MakeTelegraph(column - 1, row, time);
			MakeTelegraph(column, row + 1, time);
			MakeTelegraph(column, row - 1, time);
			AddProcess(atk);
			AddProcess(atkL);
			AddProcess(atkU);
			AddProcess(atkR);
			AddProcess(atkD);
		}

		public void BombPanel(int column, int row, double time)
		{
			MakeTelegraph(column, row, time);
			double timeStep = 0.1;
			AddProcess(new IncineratePanelAttackQuick(this)
			{
				X = column,
				Y = row,
				ImpactTime = (float)time
			});
			int bombCol = column;
			int bombRow = row;
			double bombTime = time;
			while (--bombCol >= 0)
			{
				bombTime += timeStep;
				AddProcess(new IncineratePanelAttackQuick(this, bombCol, bombRow, (float)bombTime));
				AddSound(new BattleSound(sfxFire, (float)bombTime));
			}
			bombCol = column;
			bombTime = time;
			while (++bombCol < Width)
			{
				bombTime += timeStep;
				AddProcess(new IncineratePanelAttackQuick(this, bombCol, bombRow, (float)bombTime));
				AddSound(new BattleSound(sfxFire, (float)bombTime));
			}
			bombCol = column;
			bombTime = time;
			while (--bombRow >= 0)
			{
				bombTime += timeStep;
				AddProcess(new IncineratePanelAttackQuick(this, bombCol, bombRow, (float)bombTime));
				AddSound(new BattleSound(sfxFire, (float)bombTime));
			}
			bombRow = row;
			bombTime = time;
			while (++bombRow < Height)
			{
				bombTime += timeStep;
				AddProcess(new IncineratePanelAttackQuick(this, bombCol, bombRow, (float)bombTime));
				AddSound(new BattleSound(sfxFire, (float)bombTime));
			}
		}

		public void ThrowRow(int row, double time, bool invert = false)
		{
			KnifeProjectileStandard atk = new KnifeProjectileStandard(this)
			{
				X = (invert ? (Width - 1) : 0),
				Y = row,
				ImpactTime = (float)time,
				Direction = (invert ? Direction.Left : Direction.Right)
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxThrow, (float)time - 1f, 1.5f));
		}

		public void ThrowRowSlow(int row, double time, bool invert = false)
		{
			KnifeProjectileSlow atk = new KnifeProjectileSlow(this)
			{
				X = (invert ? (Width - 1) : 0),
				Y = row,
				ImpactTime = (float)time,
				Direction = (invert ? Direction.Left : Direction.Right)
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxThrow, (float)time - 1.8f, 1.5f));
		}

		public void ThrowColSlow(int col, double time, bool invert = false)
		{
			KnifeProjectileSlow atk = new KnifeProjectileSlow(this)
			{
				X = col,
				Y = 0,
				ImpactTime = (float)time,
				Direction = Direction.Down
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxThrow, (float)time - 1.8f, 1.5f));
		}

		public void AttackPanel(int column, int row, double time)
		{
			KnifePanelAttackStandard atk = new KnifePanelAttackStandard(this)
			{
				X = column,
				Y = row,
				ImpactTime = (float)time
			};
			MakeTelegraph(column, row, time);
			AddProcess(atk);
			AddSound(new BattleSound(sfxStab, (float)time));
		}

		public void AttackPlayerPanel(double time)
		{
			KnifePlayerPanelAttackStandard atk = new KnifePlayerPanelAttackStandard(this)
			{
				ImpactTime = (float)time
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxStab, (float)time));
		}

		public void SlashPanel(int column, int row, double time)
		{
			KnifeSlashAttackStandard atk = new KnifeSlashAttackStandard(this)
			{
				X = column,
				Y = row,
				ImpactTime = (float)time
			};
			MakeTelegraph(column - 1, row, time);
			MakeTelegraph(column, row, time);
			MakeTelegraph(column + 1, row, time);
			MakeTelegraph(column - 1, row + 1, time);
			MakeTelegraph(column, row + 1, time);
			MakeTelegraph(column + 1, row + 1, time);
			AddProcess(atk);
			AddSound(new BattleSound(sfxSlash, (float)time, 0.4f));
		}

		public void SlashPlayerPanel(double time)
		{
			KnifeSlashAttackTracking atk = new KnifeSlashAttackTracking(this)
			{
				ImpactTime = (float)time,
				PositionCalculationOffset = 1f
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxSlash, (float)time, 0.4f));
		}

		public void MakeTelegraph(int x, int y, double time)
		{
			if (x >= 0 && y >= 0 && x < Width && y < Height)
			{
				AddProcess(new BasicTelegraph
				{
					X = x,
					Y = y,
					Time = (float)time,
					Duration = 1f,
					Offset = 0f
				});
			}
		}

		public void MakeTrackingTelegraph(double time, int offsetX = 0, int offsetY = 0)
		{
			AddProcess(new PlayerPanelTelegraph
			{
				Time = (float)time,
				Duration = 1f,
				Offset = 0f,
				PanelOffsetX = offsetX,
				PanelOffsetY = offsetY
			});
		}

		public void TelegraphAll(double time)
		{
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					MakeTelegraph(i, j, time);
				}
			}
		}
	}
}
