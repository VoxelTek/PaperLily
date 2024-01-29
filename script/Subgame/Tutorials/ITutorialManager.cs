using System;
using System.Collections.Generic;
using LacieEngine.API;

namespace LacieEngine.Modules.Tutorials
{
	[InjectableInterface(unique = true)]
	public interface ITutorialManager : IModule
	{
		event Action TutorialStarted;

		event Action TutorialEnded;

		void ShowTutorial(string tutorialId, bool animate);

		void ShowTutorial(string tutorialId);

		void HideTutorial();

		bool Exists(string id);

		void LoadResourcesForTutorial(string tutorialId);

		IList<string> GetDependencies(string tutorialId);
	}
}
