using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1CutsceneMkSpidey1 : GameRoomNested
	{
		[GetNode("Room/Background/SpiderIn")]
		private IAnimation2D nSpiderIn;

		[GetNode("Room/Background/SpiderIdle")]
		private IAnimation2D nSpiderIdle;

		public override void _BeforeFadeIn()
		{
			if (nSpiderIn.Node is Sprite nSpiderInSpr)
			{
				nSpiderInSpr.Frame = 1;
			}
		}

		public async void SpiderPopup()
		{
			nSpiderIn.Play();
			await this.DelaySeconds(1.0);
			nSpiderIn.Node.Visible = false;
			nSpiderIdle.Node.Visible = true;
			nSpiderIdle.Play();
		}
	}
}
