using Godot;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Subgame.ProjectKat
{
	public class FinalNoteScreen : SimpleScreen
	{
		[Export(PropertyHint.None, "")]
		public AudioStream Bgm;

		private AnimationPlayer nAnimation;

		public override void _Ready()
		{
			base._Ready();
			SetProcessInput(enable: false);
			Game.Audio.PlayBgm(Bgm);
			nAnimation = GetNode<AnimationPlayer>("CenterContainer/VBoxContainer/CenterContainer/Arrow/Animation");
		}

		public void EnableContinue()
		{
			nAnimation.PlayFirstAnimation();
			SetProcessInput(enable: true);
		}
	}
}
