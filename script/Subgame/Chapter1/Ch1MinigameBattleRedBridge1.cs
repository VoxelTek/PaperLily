using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Modules.Tutorials;

namespace LacieEngine.Minigames
{
    // Token: 0x02000119 RID: 281
    public class Ch1MinigameBattleRedBridge1 : Ch1MinigameBattleRed
    {
        // Token: 0x060009D8 RID: 2520 RVA: 0x0002748C File Offset: 0x0002568C
        public override async void Start()
        {
            base.SetProcess(false);
            base.SetProcessInput(false);
            await this.DelaySeconds(1.5);
            string tutorialId = GDUtil.GetFileNameFromPath(this.tutBattle.ResourcePath, true);
            this.Tutorials.ShowTutorial(tutorialId);
            this.Tutorials.TutorialEnded += this.ResumeAfterTutorial;
        }

        // Token: 0x060009D9 RID: 2521 RVA: 0x000274C4 File Offset: 0x000256C4
        private async void ResumeAfterTutorial()
        {
            this.Tutorials.TutorialEnded -= this.ResumeAfterTutorial;
            await this.DelaySeconds(1.0);
            base.SetProcess(true);
            base.SetProcessInput(true);
            base.Start();
        }

        // Token: 0x060009DA RID: 2522 RVA: 0x000274FC File Offset: 0x000256FC
        public override void Init()
        {
            base.Init();
            float t = 2f;
            base.SpikesOnColumn(0, (double)t);
            base.SpikesOnColumn(1, (double)t + 0.5);
            base.SpikesOnColumn(2, (double)(t + 1f));
            t = 4f;
            base.SpikesOnColumn(2, (double)t);
            base.SpikesOnColumn(1, (double)t + 0.5);
            base.SpikesOnColumn(0, (double)(t + 1f));
            t = 6f;
            base.SpikesOnRow(0, (double)t);
            base.SpikesOnRow(1, (double)t + 0.5);
            base.SpikesOnRow(2, (double)(t + 1f));
            t = 8f;
            base.SpikesOnRow(2, (double)t);
            base.SpikesOnRow(1, (double)t + 0.5);
            base.SpikesOnRow(0, (double)(t + 1f));
        }

        // Token: 0x0400083E RID: 2110
        [Export(PropertyHint.None, "")]
        private Resource tutBattle;

        // Token: 0x0400083F RID: 2111
        [Inject]
        private ITutorialManager Tutorials;
    }
}
