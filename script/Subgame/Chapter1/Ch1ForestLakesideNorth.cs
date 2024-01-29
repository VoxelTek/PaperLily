using Godot;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakesideNorth : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private AudioStream sfxRain;

		public override void _BeforeFadeIn()
		{
			Game.Audio.PlayAmbiance(sfxRain);
		}

		public override void _BeforeFadeOut()
		{
			Game.Audio.StopAmbiance();
		}
	}
}
