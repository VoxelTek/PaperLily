using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneIntro : GameRoom
	{
		[GetNode("Home_Exterior_Front")]
		private GameRoom nRoom;

		[GetNode("Animation")]
		private AnimationPlayer nAnimation;

		[GetNode("Home_Exterior_Front/Foreground/lacie_lean")]
		private Sprite nLacieLean;

		public override void _UpdateRoom()
		{
			nRoom._UpdateRoom();
		}

		public void PlayCutscene()
		{
			nAnimation.PlayFirstAnimation();
		}

		public void LacieLean()
		{
			nLacieLean.Frame = 0;
		}
	}
}
