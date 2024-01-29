using Godot;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkB2fCages : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private AudioStream bgmQuiet;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmDynamic;

		private PVar varSerious = "general.serious";

		private PVar varCrowDead = "ch1.mk_crow_dead";

		public override void _BeforeFadeIn()
		{
			if (!varSerious.Value && Game.State.OverrideBgm != "res://assets/bgm/silent.ogg")
			{
				if (!varCrowDead.Value)
				{
					Game.Audio.PlayBgm(bgmQuiet, 1f, crossFade: true);
				}
				else
				{
					Game.Audio.PlayBgm(bgmDynamic, 1f, crossFade: true);
				}
			}
		}
	}
}
