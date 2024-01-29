using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1BlackPassKitchen : GameRoom
	{
		[GetNode("Background/RedItem")]
		private Node2D nRedItemSpr;

		[GetNode("Other/Events/misc_reditem")]
		private EventTrigger nRedItemEvent;

		[GetNode("Main/chr_mom")]
		private Node2D nMom;

		[GetNode("Main/MomCuttingCutscene")]
		private IAnimation2D nMomCuttingCutscene;

		[GetNode("Main/MomCutting")]
		private IAnimation2D nMomCuttingLoop;

		private PVar varRedItemSpawned = "general.blackpass_red_item_2_spawned";

		private PVar varFinishedQuest = "ch1.blackpass_retrieved_knife";

		public override void _UpdateRoom()
		{
			bool redItemSpawned = varRedItemSpawned.Value;
			nRedItemSpr.Visible = redItemSpawned;
			nRedItemEvent.Enabled = redItemSpawned;
			nMom.Visible = !varFinishedQuest.Value;
			nMomCuttingLoop.Node.Visible = varFinishedQuest.Value;
		}

		public async void PlayCuttingCutscene()
		{
			nMom.Visible = false;
			nMomCuttingCutscene.Node.Visible = true;
			await this.DelaySeconds(1.0);
			nMomCuttingCutscene.Play();
		}
	}
}
