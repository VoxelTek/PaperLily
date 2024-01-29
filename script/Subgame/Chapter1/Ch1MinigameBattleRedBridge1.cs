using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Modules.Tutorials;

namespace LacieEngine.Minigames
{
	public class Ch1MinigameBattleRedBridge1 : Ch1MinigameBattleRed
	{
		[Export(PropertyHint.None, "")]
		private Resource tutBattle;

		[Inject]
		private ITutorialManager Tutorials;

		public override async void Start()
		{
			SetProcess(enable: false);
			SetProcessInput(enable: false);
			await this.DelaySeconds(1.5);
			string tutorialId = GDUtil.GetFileNameFromPath(tutBattle.ResourcePath, removeExtension: true);
			Tutorials.ShowTutorial(tutorialId);
			Tutorials.TutorialEnded += ResumeAfterTutorial;
		}

		private async void ResumeAfterTutorial()
		{
			Tutorials.TutorialEnded -= ResumeAfterTutorial;
			await this.DelaySeconds(1.0);
			SetProcess(enable: true);
			SetProcessInput(enable: true);
			base.Start();
		}

		public override void Init()
		{
			base.Init();
			float t = 2f;
			SpikesOnColumn(0, t);
			SpikesOnColumn(1, (double)t + 0.5);
			SpikesOnColumn(2, t + 1f);
			t = 4f;
			SpikesOnColumn(2, t);
			SpikesOnColumn(1, (double)t + 0.5);
			SpikesOnColumn(0, t + 1f);
			t = 6f;
			SpikesOnRow(0, t);
			SpikesOnRow(1, (double)t + 0.5);
			SpikesOnRow(2, t + 1f);
			t = 8f;
			SpikesOnRow(2, t);
			SpikesOnRow(1, (double)t + 0.5);
			SpikesOnRow(0, t + 1f);
		}
	}
}
