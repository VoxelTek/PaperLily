using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakesideLake : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private AudioStream sfxPaddle1;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxPaddle2;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxPaddle3;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxPaddle4;

		[GetNode("Background/LacieRowing")]
		private IAnimation2D nLaciePaddle;

		private Random random = new Random();

		public void Paddle()
		{
			AudioStream toPlay = random.Next(4) switch
			{
				0 => sfxPaddle1, 
				1 => sfxPaddle2, 
				2 => sfxPaddle3, 
				3 => sfxPaddle4, 
				_ => sfxPaddle1, 
			};
			Game.Audio.PlaySfx(toPlay, 0.3f);
			nLaciePaddle.Play();
		}
	}
}
