using Godot;

namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleRed : Battle
	{
		public class SpikeAttackStandard : BasicPanelAttack
		{
			public SpikeAttackStandard(Ch1MinigameBattleRed battle)
			{
				Hframes = 5;
				Vframes = 1;
				Frames = "0,0,0,0,0,0,1,2,3,4,3";
				Fps = 15f;
				Loop = false;
				StartOffset = 0.59f;
				Lifetime = 0.2f;
				DamageDuration = 0.1f;
				Texture = battle.texSpikes;
			}
		}

		public class SpikeAttackInfinite : BasicPanelAttack
		{
			public SpikeAttackInfinite(Ch1MinigameBattleRed battle)
			{
				Hframes = 5;
				Vframes = 1;
				Frames = "0,0,0,0,0,0,1,2,3,4,3";
				Fps = 15f;
				Loop = false;
				StartOffset = 0.59f;
				Lifetime = 30f;
				DamageDuration = 30f;
				Texture = battle.texSpikes;
			}
		}

		[Export(PropertyHint.None, "")]
		public Texture texSpikes;

		[Export(PropertyHint.None, "")]
		public AudioStream sfxSpikes;

		public const float SPIKE_DURATION = 0.8f;

		public void AttackColumn2(int column, double time, bool invert = false)
		{
			SpikeAttackStandard atk = new SpikeAttackStandard(this)
			{
				X = column,
				Y = (invert ? 3 : 0),
				ImpactTime = (float)time
			};
			AddProcess(atk);
		}

		public void SpikesOnPanel(int column, int row, double time)
		{
			SpikeAttackStandard atk = new SpikeAttackStandard(this)
			{
				X = column,
				Y = row,
				ImpactTime = (float)time
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxSpikes, (float)time - 0.2f, 0.4f));
		}

		public void SpikesOnRow(int row, double time)
		{
			for (int i = 0; i < Height; i++)
			{
				SpikeAttackStandard atk = new SpikeAttackStandard(this)
				{
					X = i,
					Y = row,
					ImpactTime = (float)time
				};
				AddProcess(atk);
			}
			AddSound(new BattleSound(sfxSpikes, (float)time - 0.2f, 0.4f));
		}

		public void SpikesOnColumn(int column, double time)
		{
			for (int i = 0; i < Width; i++)
			{
				SpikeAttackStandard atk = new SpikeAttackStandard(this)
				{
					X = column,
					Y = i,
					ImpactTime = (float)time
				};
				AddProcess(atk);
			}
			AddSound(new BattleSound(sfxSpikes, (float)time - 0.2f, 0.4f));
		}

		public void LongSpikesOnPanel(int column, int row, double time)
		{
			SpikeAttackInfinite atk = new SpikeAttackInfinite(this)
			{
				X = column,
				Y = row,
				ImpactTime = (float)time
			};
			AddProcess(atk);
			AddSound(new BattleSound(sfxSpikes, (float)time - 0.2f, 0.4f));
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
					Duration = 0.5f,
					Offset = 0.5f
				});
			}
		}
	}
}
