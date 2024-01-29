using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1RedB1f : GameRoom
	{
		[Inject]
		private IExtraFunctions Functions;

		[GetNode("Background/MiscJar")]
		private Node2D nJar;

		[GetNode("Background/WallMirror")]
		private Node2D nMirror;

		[GetNode("Background/MiscMirrorBroken")]
		private Node2D nMirrorBroken;

		[GetNode("Main/FurnitureCage/LacieDeathPrisoner")]
		private SimpleAnimatedSprite nLacieStrangle;

		[GetNode("Main/FurnitureCage/PrisonerLight")]
		private SimpleAnimatedSprite nPrisonerLight;

		private PVar varCode = "persistent.ch1_red_passage_code";

		private PVar varTookJar = "ch1.forest_red_took_jar";

		private PVar varBrokeMirror = "ch1.forest_red_cracked_mirror";

		private PVar varPrisonerQuestDone = "ch1.forest_red_prisoner_stage";

		private PVar varTookNotes = "ch1.forest_red_took_2nd_notes";

		public override void _BeforeFadeIn()
		{
			if (!varCode.Value)
			{
				Random random = new Random();
				varCode.NewValue = random.Next(1000, 10000);
				PersistentState.Save();
			}
		}

		public override void _UpdateRoom()
		{
			nJar.Visible = !varTookJar.Value;
			nMirror.Visible = !varBrokeMirror.Value;
			nMirrorBroken.Visible = varBrokeMirror.Value;
			nPrisonerLight.Visible = varPrisonerQuestDone.Value == 6;
			nPrisonerLight.Playing = varPrisonerQuestDone.Value == 6;
		}

		public void LacieStrangle()
		{
			Game.Player.Node.Visible = false;
			nLacieStrangle.Visible = true;
			nLacieStrangle.Playing = true;
		}

		public void LacieEscape()
		{
			nLacieStrangle.Visible = false;
			nLacieStrangle.Playing = false;
			Game.Player.SetPlayerState(CharacterState.InObject);
			Game.Player.SpriteState = "fall";
			Game.Player.SpriteDirection = Direction.Up;
			Game.Player.Node.Visible = true;
		}

		public void StopTimer()
		{
			Functions.StopTimer();
		}
	}
}
