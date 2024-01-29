using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerTransitionCommand : StoryPlayerCommand
	{
		public enum TransitionType
		{
			In,
			Out
		}

		private const float DEFAULT_FADE_DURATION = 0.25f;

		public TransitionType Type { get; set; }

		public string Scene { get; set; }

		public float? Time { get; set; }

		public bool ContinueImmediately { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			float time = Time ?? 0.25f;
			storyPlayer.Characters.HideAllCharacters();
			storyPlayer.UI.HideDialogueBox();
			storyPlayer.nTransitionOverlay.Clear();
			Node scn = GDUtil.Instance<Node>("res://resources/nodes/cg/" + Scene + ".tscn");
			storyPlayer.nTransitionOverlay.AddChild(scn);
			ITransitionScene transition = scn as ITransitionScene;
			if (transition == null)
			{
				Log.Error("Scene ", Scene, " does not implement ", "ITransitionScene");
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
			}
			Game.Animations.StopAnimations(scn);
			if (Type == TransitionType.Out)
			{
				transition.TransitionOut(time);
			}
			else if (Type == TransitionType.In)
			{
				transition.TransitionIn(time);
			}
			storyPlayer.SetNextBlockContinue();
			if (ContinueImmediately || time <= 0f)
			{
				storyPlayer.Next();
			}
			else
			{
				storyPlayer.NextAfterSeconds(time);
			}
		}

		public override IList<string> GetDependencies()
		{
			return new List<string>(1) { "res://resources/nodes/cg/" + Scene + ".tscn" };
		}
	}
}
