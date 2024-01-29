using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneBusA : GameRoom
	{
		[GetNode("Background/seats_a5/lacie")]
		private Sprite nLacieSitting;

		[GetNode("Background/seats_a5/lacie/Animation")]
		private AnimationPlayer nLacieScoochAnimation;

		[GetNode("Background/curtains_a/Animation")]
		private AnimationPlayer nTicketAnimation;

		[GetNode("BusMoveAnimation")]
		private AnimationPlayer nBusMoveAnimation;

		private NPCWalker nLacie;

		public override void _BeforeFadeIn()
		{
			nLacieSitting.Visible = false;
			nLacie = new NPCWalker("lacie", "up");
			nLacie.Position = GetPoint("enter");
			GetMainLayer().AddChild(nLacie);
		}

		public void LacieScoochIn()
		{
			nLacie.Visible = false;
			nLacieSitting.Visible = true;
			nLacieScoochAnimation.PlayFirstAnimation();
		}

		public void PlayTicketAnimation()
		{
			nTicketAnimation.PlayFirstAnimation();
		}

		public void PlayBusAnimation()
		{
			nBusMoveAnimation.GetAnimation("animation").TrackSetPath(0, string.Concat(Game.Camera.Node.GetPath(), ":offset"));
			nBusMoveAnimation.PlayFirstAnimation();
		}

		public void LacieCloseEyes()
		{
			nLacieSitting.Frame = 5;
		}
	}
}
