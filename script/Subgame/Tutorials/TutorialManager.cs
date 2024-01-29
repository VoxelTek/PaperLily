using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Modules.Tutorials
{
	[Injectable]
	public class TutorialManager : ITutorialManager, IModule, ITranslatable
	{
		private TutorialScreen currentTutorial;

		public event Action TutorialStarted;

		public event Action TutorialEnded;

		public void ShowTutorial(string tutorialId, bool animate)
		{
			Game.InputProcessor = Inputs.Processor.None;
			if (!Game.State.SeenTutorials.Contains(tutorialId))
			{
				Game.State.SeenTutorials.Add(tutorialId);
			}
			currentTutorial = new TutorialScreen();
			currentTutorial.Tutorial = LoadTutorial(tutorialId);
			Game.Screen.AddToLayer(IScreenManager.Layer.UI, currentTutorial);
			currentTutorial.Start(animate);
			this.TutorialStarted?.Invoke();
		}

		public void ShowTutorial(string tutorialId)
		{
			ShowTutorial(tutorialId, animate: true);
		}

		public void HideTutorial()
		{
			if (currentTutorial != null)
			{
				currentTutorial.Delete();
				currentTutorial = null;
				Game.Core.AssignInputProcessor();
				this.TutorialEnded?.Invoke();
			}
		}

		public bool Exists(string id)
		{
			return GDUtil.FileExists(IdToPath(id));
		}

		public string IdToPath(string id)
		{
			return "res://resources/tutorials/" + id + ".tres";
		}

		public Tutorial LoadTutorial(string id)
		{
			Tutorial tutorial = GD.Load<Tutorial>(IdToPath(id));
			tutorial.Id = id;
			tutorial.Name = Game.Language.GetCaption(tutorial.Name);
			tutorial.Text = Game.Language.GetCaption(tutorial.Text);
			return tutorial;
		}

		public void LoadResourcesForTutorial(string tutorialId)
		{
			Game.Memory.Cache(IdToPath(tutorialId));
		}

		public IList<string> GetDependencies(string tutorialId)
		{
			return new List<string> { IdToPath(tutorialId) };
		}

		public void ApplyTranslationOverrides()
		{
		}

		private string TutorialNameContext(Tutorial tutorial)
		{
			return "tutorials.name." + tutorial.Id;
		}

		private string TutorialTextContext(Tutorial tutorial)
		{
			return "tutorials.text." + tutorial.Id;
		}
	}
}
