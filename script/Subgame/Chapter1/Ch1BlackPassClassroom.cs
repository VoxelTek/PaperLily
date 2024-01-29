using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1BlackPassClassroom : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texLacieDark;

		[GetNode("Main/Bouncer")]
		private Node2D nBouncer;

		[GetNode("Main/People_1")]
		private Node2D nPeople1;

		[GetNode("Main/People_2")]
		private Node2D nPeople2;

		[GetNode("Main/People_3")]
		private Node2D nPeople3;

		[GetNode("Main/Desk/FurnitureDesk4/MiscKnife")]
		private Node2D nKnife;

		[GetNode("Main/Wardrobe")]
		private Sprite nWardrobe;

		[GetNode("Main/Desk/FurnitureDesk4/Crow")]
		private Sprite nCrow;

		[GetNode("Other/Events/misc_people_1")]
		private EventTrigger nWall1Event;

		[GetNode("Other/Events/misc_people_2")]
		private EventTrigger nWall2Event;

		[GetNode("Other/Events/misc_person_left")]
		private EventTrigger nWall3Event;

		[GetNode("Other/Events/misc_person_right")]
		private EventTrigger nWall4Event;

		[GetNode("Other/Events/misc_wardrobe")]
		private EventTrigger nWardrobeEvent;

		[GetNode("Main/Desk/FurnitureDesk4/Crow/RedItem")]
		private Node2D nRedItem;

		private PVar varTookKnife = "ch1.blackpass_classroom_took_knife";

		private PVar varKilledBird = "ch1.blackpass_classroom_killed_bird";

		private PVar varCutsceneStage = "ch1.blackpass_classroom_cutscene_stage";

		private PVar varTookRedItem = "general.blackpass_red_item_3_took";

		private PVar varSpawnedRedItem = "general.blackpass_red_item_3_spawned";

		public override void _UpdateRoom()
		{
			nKnife.Visible = !varTookKnife.Value;
			nBouncer.Visible = varCutsceneStage.Value == 2;
			nPeople1.Visible = varCutsceneStage.Value == 1;
			nPeople2.Visible = varCutsceneStage.Value == 2;
			nPeople3.Visible = varCutsceneStage.Value == 3;
			nWall1Event.Enabled = (int)varCutsceneStage.Value < 4;
			nWall2Event.Enabled = (int)varCutsceneStage.Value < 4;
			nWall3Event.Enabled = (int)varCutsceneStage.Value < 4;
			nWall4Event.Enabled = (int)varCutsceneStage.Value < 4;
			nWardrobe.Visible = (int)varCutsceneStage.Value >= 4;
			nWardrobeEvent.Enabled = (int)varCutsceneStage.Value >= 4;
			nCrow.Frame = (((bool)varKilledBird.Value) ? 1 : 0);
			nRedItem.Visible = (bool)varSpawnedRedItem.Value && !varTookRedItem.Value;
		}

		public async void OpenWardrobe()
		{
			nWardrobe.Frame = 1;
			await this.DelaySeconds(0.5);
			nWardrobe.Frame = 2;
		}

		public void SetLacieShadow()
		{
			WalkingCharacter obj = Game.Player.Node as WalkingCharacter;
			obj.OverrideTextureForState("stand", texLacieDark);
			obj.OverrideTextureForState("walk", texLacieDark);
		}
	}
}
