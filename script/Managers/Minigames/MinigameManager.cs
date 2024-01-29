using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	[Injectable]
	public class MinigameManager : IMinigameManager
	{
		private const float SlideAnimationTime = 0.3f;

		private IMinigame minigame;

		private Control nMinigameContainer;

		private Control nMinigameInfo;

		public bool Running => nMinigameContainer.IsValid();

		public Node Node => minigame?.Node;

		public async void Start(string file)
		{
			Log.Debug("Starting minigame: ", file);
			Game.InputProcessor = Inputs.Processor.None;
			MinigameResource minigameRes = GD.Load<MinigameResource>("res://resources/minigame/" + file + ".tres");
			nMinigameContainer = GDUtil.MakeNode<CenterContainer>("MinigameContainer");
			nMinigameContainer.RectPosition = new Vector2(0f, Game.Settings.BaseResolution.y);
			nMinigameContainer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
			minigame = minigameRes.MakeMinigame();
			nMinigameContainer.AddChild(minigame.Node);
			if (minigame.CustomTransition)
			{
				await minigame.TransitionIn();
				Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, nMinigameContainer);
			}
			else
			{
				Game.Screen.DarkenScreen();
				Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, nMinigameContainer);
				LacieAnimation animation = new SlideInBottomAnimation(nMinigameContainer, Game.Settings.BaseResolution.y, Vector2.Zero, 0.3f);
				Game.Animations.Play(animation);
				await animation.WaitUntilFinished();
			}
			nMinigameInfo = GDUtil.MakeNode<Control>("MinigameInfo");
			nMinigameInfo.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
			Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, nMinigameInfo);
			minigame.AddUiElements(nMinigameInfo);
			Game.InputProcessor = Inputs.Processor.Minigame;
			minigame.Start();
		}

		public async void End(string @event)
		{
			if (nMinigameContainer != null)
			{
				nMinigameInfo.DeleteIfValid();
				Game.Screen.UndarkenScreen();
				if (minigame.CustomTransition)
				{
					await minigame.TransitionOut();
				}
				else
				{
					LacieAnimation animation = new SlideOutBottomAnimation(nMinigameContainer, Game.Settings.BaseResolution.y, Vector2.Zero, 0.3f);
					Game.Animations.Play(animation);
					await animation.WaitUntilFinished();
				}
				nMinigameContainer.Delete();
				nMinigameContainer = null;
				nMinigameInfo = null;
				minigame = null;
				if (!@event.IsNullOrEmpty())
				{
					Game.Events.PlayEvent(@event);
				}
				Game.Core.AssignInputProcessor();
			}
		}

		public void End()
		{
			End(null);
		}

		public bool Function(string function)
		{
			if (!Running)
			{
				return false;
			}
			if (minigame.Node.HasMethod(function))
			{
				minigame.Node.Call(function);
				return true;
			}
			Log.Warn("Method ", function, "() not found in minigame: ", minigame.Node.Name);
			return false;
		}
	}
}
