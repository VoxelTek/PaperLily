using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Subgame.Chapter1
{
	public class BlinkTransition : TextureRect, ITransitionScene
	{
		public void TransitionIn(float duration)
		{
			ShaderProgressAnimation animation = new ShaderProgressAnimation(this, duration);
			Game.Animations.Play(animation);
		}

		public void TransitionOut(float duration)
		{
			ShaderProgressAnimation animation = new ShaderProgressAnimation(this, duration, invert: true);
			Game.Animations.Play(animation);
		}
	}
}
