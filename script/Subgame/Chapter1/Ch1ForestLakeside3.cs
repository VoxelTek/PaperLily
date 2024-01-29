using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakeside3 : GameRoom
	{
		[GetNode("Background/HiddenDoor")]
		private SimpleAnimatedSprite nHiddenDoor;

		[GetNode("Background/LogMovable")]
		private Node2D nLog;

		[GetNode("Ground/LogInWater")]
		private Node2D nLogWater;

		[GetNode("Underwater/MiscLogUnderwater")]
		private Node2D nLogWaterShadow;

		[GetNode("Other/Events/misc_log_push")]
		private EventTrigger nPushLogEvt;

		[GetNode("Other/Events/misc_water")]
		private EventTrigger nWaterEvt;

		private PVar varPushedLog = "ch1.forest_lakeside_pushed_log";

		public override void _UpdateRoom()
		{
			bool pushedLog = varPushedLog.Value;
			nLog.Visible = !pushedLog;
			nLogWater.Visible = pushedLog;
			nLogWaterShadow.Visible = pushedLog;
			nPushLogEvt.Enabled = !pushedLog;
			nWaterEvt.Enabled = !pushedLog;
		}

		public void OpenHiddenDoor1()
		{
			Game.Player.VisualNode.Modulate = Colors.Transparent;
		}

		public async void OpenHiddenDoor2()
		{
			nHiddenDoor.Playing = true;
			await this.DelaySeconds(2.0);
			Game.Animations.Play(new FadeInAnimation(Game.Player.VisualNode, 1f));
			await this.DelaySeconds(1.5);
			if (Game.Player.Node is IWalker walker)
			{
				walker.Walk(0, 32, Direction.Down, IWalker.MoveSpeed.Normal);
			}
			await this.DelaySeconds(1.0);
			nHiddenDoor.AnimationFrames = "4,3,2,1,0";
			nHiddenDoor.Playing = true;
		}
	}
}
