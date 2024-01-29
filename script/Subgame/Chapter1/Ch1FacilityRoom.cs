using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Subgame.Chapter1;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityRoom : GameRoom
	{
		[Export(PropertyHint.None, "")]
		protected Lighting lightOn;

		[Export(PropertyHint.None, "")]
		protected Lighting lightOff;

		[Export(PropertyHint.None, "")]
		protected float primalChanceEz;

		[Export(PropertyHint.None, "")]
		protected float primalChanceHd;

		[Export(PropertyHint.None, "")]
		protected string primalSpawnVar;

		[Export(PropertyHint.None, "")]
		protected string primalTypeVar;

		[GetNode("Lighting")]
		protected Node2D nLights;

		private PVar varLightsOff = "ch1.facility_lights_off";

		private PVar varReleased = "ch1.facility_dungeon_2_released";

		private PVar varPrimalsCanSpawn = "ch1.facility_primals_can_spawn";

		private PEvent evtDeath = "ch1_death_impact";

		protected PVar varPrimalSpawn;

		protected PVar varPrimalType;

		protected NPCChaser nChaser;

		public override void _UpdateRoom()
		{
			bool lightsOn = !varLightsOff.Value;
			nLights.Visible = lightsOn;
			if (lightsOn)
			{
				lightOn.Apply();
			}
			else
			{
				lightOff.Apply();
			}
		}

		public override void _AfterFadeIn()
		{
			if (nChaser is Ch1FacilityPrimal2)
			{
				nChaser.Chasing = true;
			}
		}

		public void TrySpawnPrimal(Navigation2D nav, int spawnPoints)
		{
			if ((bool)varPrimalsCanSpawn.Value && !primalSpawnVar.IsNullOrEmpty())
			{
				varPrimalSpawn = primalSpawnVar;
				varPrimalType = primalTypeVar;
				float spawnChance = ((!varPrimalSpawn.Value) ? (varReleased.Value ? primalChanceHd : primalChanceEz) : 100f);
				if (DrkieUtil.RollPercent(spawnChance))
				{
					SpawnEnemy(nav, spawnPoints);
				}
			}
		}

		private void SpawnEnemy(Navigation2D nav, int spawnPoints)
		{
			if (!varPrimalSpawn.Value)
			{
				varPrimalSpawn.NewValue = DecideSpawn(spawnPoints);
				varPrimalType.NewValue = DecideEnemy();
			}
			nChaser = MakeEnemy(varPrimalType.Value);
			SpawnPoint spawn = GetSpawnPoint(varPrimalSpawn.Value);
			nChaser.Position = spawn.Position;
			nChaser.DefaultDirection = spawn.Direction;
			GetMainLayer().AddChild(nChaser);
			nChaser.SetNavigation(nav);
			nChaser.Caught += PlayerDeath;
		}

		public void PlayerDeath()
		{
			evtDeath.Play();
		}

		private string DecideSpawn(int max)
		{
			Random rnd = new Random();
			return "primal_spawn_" + rnd.Next(1, max + 1);
		}

		private int DecideEnemy()
		{
			if (DrkieUtil.RollPercent(75.0))
			{
				return 1;
			}
			return 2;
		}

		private NPCChaser MakeEnemy(int number)
		{
			if (number == 1)
			{
				return new Ch1FacilityPrimal();
			}
			return new Ch1FacilityPrimal2();
		}
	}
}
