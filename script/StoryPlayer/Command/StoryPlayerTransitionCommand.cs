using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
    // Token: 0x020000A0 RID: 160
    [Serializable]
    public class StoryPlayerTransitionCommand : StoryPlayerCommand
    {
        // Token: 0x170000F9 RID: 249
        // (get) Token: 0x06000583 RID: 1411 RVA: 0x000155DE File Offset: 0x000137DE
        // (set) Token: 0x06000584 RID: 1412 RVA: 0x000155E6 File Offset: 0x000137E6
        public StoryPlayerTransitionCommand.TransitionType Type { get; set; }

        // Token: 0x170000FA RID: 250
        // (get) Token: 0x06000585 RID: 1413 RVA: 0x000155EF File Offset: 0x000137EF
        // (set) Token: 0x06000586 RID: 1414 RVA: 0x000155F7 File Offset: 0x000137F7
        public string Scene { get; set; }

        // Token: 0x170000FB RID: 251
        // (get) Token: 0x06000587 RID: 1415 RVA: 0x00015600 File Offset: 0x00013800
        // (set) Token: 0x06000588 RID: 1416 RVA: 0x00015608 File Offset: 0x00013808
        public float? Time { get; set; }

        // Token: 0x170000FC RID: 252
        // (get) Token: 0x06000589 RID: 1417 RVA: 0x00015611 File Offset: 0x00013811
        // (set) Token: 0x0600058A RID: 1418 RVA: 0x00015619 File Offset: 0x00013819
        public bool ContinueImmediately { get; set; }

        // Token: 0x0600058B RID: 1419 RVA: 0x00015624 File Offset: 0x00013824
        public override void Execute(StoryPlayer storyPlayer)
        {
            float time = this.Time ?? 0.25f;
            storyPlayer.Characters.HideAllCharacters();
            storyPlayer.UI.HideDialogueBox();
            storyPlayer.nTransitionOverlay.Clear();
            Node scn = GDUtil.Instance<Node>("res://resources/nodes/cg/" + this.Scene + ".tscn");
            storyPlayer.nTransitionOverlay.AddChild(scn, false);
            ITransitionScene transition = scn as ITransitionScene;
            if (transition == null)
            {
                Log.Error(new object[] { "Scene ", this.Scene, " does not implement ", "ITransitionScene" });
                storyPlayer.SetNextBlockContinue();
                storyPlayer.Next();
            }
            Game.Animations.StopAnimations(scn);
            if (this.Type == StoryPlayerTransitionCommand.TransitionType.Out)
            {
                transition.TransitionOut(time);
            }
            else if (this.Type == StoryPlayerTransitionCommand.TransitionType.In)
            {
                transition.TransitionIn(time);
            }
            storyPlayer.SetNextBlockContinue();
            if (this.ContinueImmediately || time <= 0f)
            {
                storyPlayer.Next();
                return;
            }
            storyPlayer.NextAfterSeconds(time);
        }

        // Token: 0x0600058C RID: 1420 RVA: 0x0001572B File Offset: 0x0001392B
        public override IList<string> GetDependencies()
        {
            return new List<string>(1) { "res://resources/nodes/cg/" + this.Scene + ".tscn" };
        }

        // Token: 0x04000499 RID: 1177
        private const float DEFAULT_FADE_DURATION = 0.25f;

        // Token: 0x02000245 RID: 581
        public enum TransitionType
        {
            // Token: 0x04000CE9 RID: 3305
            In,
            // Token: 0x04000CEA RID: 3306
            Out
        }
    }
}
