using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Artist2f : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texKettMad;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varKettChase = "ch1.forest_kett_chase";

		private NPCChaser nKettChasing;

		public override void _BeforeFadeIn()
		{
			if ((bool)varKettChase.Value)
			{
				nKettChasing = new NPCChaser("kett");
				nKettChasing.MoveSpeedMultiplier = 1.3f;
				GetMainLayer().AddChild(nKettChasing);
				nKettChasing.OverrideTextureForState("stand", texKettMad);
				nKettChasing.OverrideTextureForState("walk", texKettMad);
				nKettChasing.SetNavigation(nNavigation);
				nKettChasing.Visible = false;
				nKettChasing.Position = GetPoint("from_downstairs");
			}
		}

		public override void _AfterFadeIn()
		{
			if (nKettChasing.IsValid())
			{
				SpawnKett();
			}
		}

		public async void SpawnKett()
		{
			await this.DelaySeconds(0.5);
			nKettChasing.Caught += PlayerDeath;
			nKettChasing.Visible = true;
			nKettChasing.Chasing = true;
		}

		public void PlayerDeath()
		{
			Game.Events.PlayEvent("ch1_death_impact");
		}

		private void PauseChaser()
		{
			if (nKettChasing.IsValid())
			{
				nKettChasing.Chasing = false;
			}
		}
	}
}
