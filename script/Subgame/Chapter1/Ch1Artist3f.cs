using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Artist3f : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texKettMad;

		[GetNode("Background/Candleman")]
		private Node2D nCandleman;

		[GetNode("Background/CandlemanBroken")]
		private Node2D nCandlemanBroken;

		[GetNode("Background/CandlemanBase")]
		private Node2D nCandlemanGone;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		[GetNode("Background/CandlemanBroken/Shiny")]
		private Node2D nUnlitCandle;

		[GetNode("Events/item_unlitcandle_get")]
		private EventTrigger nUnlitCandleEvt;

		private PVar varCandlemanDead = "ch1.forest_candleman_dead";

		private PVar varKettChase = "ch1.forest_kett_chase";

		private PVar varCandlemanBlew = "ch1.forest_candleman_blew_candle";

		private PVar varTookUnlitCandle = "ch1.forest_candleman_took_unlit_candle";

		private PVar varCandlemanSaved = "ch1.forest_candleman_saved";

		private NPCChaser nKettChasing;

		public override void _UpdateRoom()
		{
			nCandleman.Visible = !varCandlemanDead.Value && !varCandlemanSaved.Value;
			nCandlemanBroken.Visible = varCandlemanDead.Value;
			nCandlemanGone.Visible = varCandlemanSaved.Value;
			if ((bool)varCandlemanBlew.Value && !varTookUnlitCandle.Value)
			{
				nUnlitCandle.Visible = true;
				nUnlitCandleEvt.Enabled = true;
			}
			else
			{
				nUnlitCandle.Visible = false;
				nUnlitCandleEvt.Enabled = false;
			}
		}

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
