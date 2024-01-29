using Godot;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class Bubble : Sprite
	{
		[Export(PropertyHint.None, "")]
		private AudioStream sfxSound;

		private AnimationPlayer nAnimation;

		public override void _Ready()
		{
			nAnimation = GetNode<AnimationPlayer>("Animation");
			PlayAndDestroyWhenDone();
		}

		public void PlaySound()
		{
			Game.Audio.PlaySfx(sfxSound);
		}

		private async void PlayAndDestroyWhenDone()
		{
			nAnimation.PlayFirstAnimation();
			await nAnimation.WaitUntilFinished();
			this.Delete();
		}
	}
}
