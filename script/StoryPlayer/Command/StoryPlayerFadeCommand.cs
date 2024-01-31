using System;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
    // Token: 0x02000095 RID: 149
    [Serializable]
    public class StoryPlayerFadeCommand : StoryPlayerCommand
    {
        // Token: 0x170000C9 RID: 201
        // (get) Token: 0x060004F1 RID: 1265 RVA: 0x00013FF9 File Offset: 0x000121F9
        // (set) Token: 0x060004F2 RID: 1266 RVA: 0x00014001 File Offset: 0x00012201
        public StoryPlayerFadeCommand.FadeType Type { get; set; }

        // Token: 0x170000CA RID: 202
        // (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001400A File Offset: 0x0001220A
        // (set) Token: 0x060004F4 RID: 1268 RVA: 0x00014012 File Offset: 0x00012212
        public float? Time { get; set; }

        // Token: 0x170000CB RID: 203
        // (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001401B File Offset: 0x0001221B
        // (set) Token: 0x060004F6 RID: 1270 RVA: 0x00014023 File Offset: 0x00012223
        public string Color { get; set; }

        // Token: 0x170000CC RID: 204
        // (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001402C File Offset: 0x0001222C
        // (set) Token: 0x060004F8 RID: 1272 RVA: 0x00014034 File Offset: 0x00012234
        public bool ContinueImmediately { get; set; }

        // Token: 0x060004F9 RID: 1273 RVA: 0x00014040 File Offset: 0x00012240
        public override void Execute(StoryPlayer storyPlayer)
        {
            ColorRect overlay = ((this.Type == StoryPlayerFadeCommand.FadeType.Flash) ? storyPlayer.nFlashOverlay : storyPlayer.nFadeOverlay);
            float time = this.Time ?? 0.25f;
            overlay.Color = ((!this.Color.IsNullOrEmpty()) ? GDUtil.StringToColor(this.Color) : this.DEFAULT_FADE_COLOR);
            storyPlayer.Characters.HideAllCharacters();
            storyPlayer.UI.HideDialogueBox();
            Game.Animations.StopAnimations(overlay);
            if (this.Type == StoryPlayerFadeCommand.FadeType.FadeOut)
            {
                this.ShowOverlay(overlay, time);
            }
            else if (this.Type == StoryPlayerFadeCommand.FadeType.FadeIn)
            {
                this.HideOverlay(overlay, time);
            }
            else if (this.Type == StoryPlayerFadeCommand.FadeType.Flash)
            {
                this.Flash(overlay, time);
            }
            storyPlayer.SetNextBlockContinue();
            if (this.ContinueImmediately || time <= 0f)
            {
                storyPlayer.Next();
                return;
            }
            storyPlayer.NextAfterSeconds(time);
        }

        // Token: 0x060004FA RID: 1274 RVA: 0x00014125 File Offset: 0x00012325
        private void ShowOverlay(Control control, float time)
        {
            if (control.Modulate.a == 1f)
            {
                return;
            }
            if (time > 0f)
            {
                Game.Animations.Play(new FadeInAnimation(control, time));
                return;
            }
            control.Modulate = Colors.White;
        }

        // Token: 0x060004FB RID: 1275 RVA: 0x0001415F File Offset: 0x0001235F
        private void HideOverlay(Control control, float time)
        {
            if (control.Modulate.a == 0f)
            {
                return;
            }
            if (time > 0f)
            {
                Game.Animations.Play(new FadeOutAnimation(control, time));
                return;
            }
            control.Modulate = Colors.Transparent;
        }

        // Token: 0x060004FC RID: 1276 RVA: 0x00014199 File Offset: 0x00012399
        private void Flash(Control control, float time)
        {
            control.Modulate = Colors.White;
            this.HideOverlay(control, time);
        }

        // Token: 0x0400045B RID: 1115
        private const float DEFAULT_FADE_DURATION = 0.25f;

        // Token: 0x0400045C RID: 1116
        private readonly Color DEFAULT_FADE_COLOR = Colors.Black;

        // Token: 0x0200023A RID: 570
        public enum FadeType
        {
            // Token: 0x04000CAB RID: 3243
            FadeIn,
            // Token: 0x04000CAC RID: 3244
            FadeOut,
            // Token: 0x04000CAD RID: 3245
            Flash
        }
    }
}
