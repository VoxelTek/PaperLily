using Godot;

namespace LacieEngine.Animation
{
	public abstract class FadeAnimation : LacieAnimation
	{
		protected static readonly Color VisibleColor = Colors.White;

		protected static readonly Color HiddenColor = new Color(1f, 1f, 1f, 0f);
	}
}
