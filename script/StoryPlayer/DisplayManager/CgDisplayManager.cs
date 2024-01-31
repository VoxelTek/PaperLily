using System;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
    // Token: 0x020000A3 RID: 163
    public class CgDisplayManager
    {
        // Token: 0x0600059C RID: 1436 RVA: 0x000159A8 File Offset: 0x00013BA8
        public CgDisplayManager(StoryPlayer storyPlayer)
        {
            this.storyPlayer = storyPlayer;
            this.mainCgNode = storyPlayer.nCg;
            this.altCgNode = storyPlayer.nCgAlt;
            this.curCgLayer = CgDisplayManager.CgLayer.CG;
            this.curCg = null;
        }

        // Token: 0x0600059D RID: 1437 RVA: 0x000159FE File Offset: 0x00013BFE
        public void Reset()
        {
            this.mainCgNode.Clear();
            this.altCgNode.Clear();
            this.mainCgNode.Modulate = this.ColorNotVisible;
            this.altCgNode.Modulate = this.ColorNotVisible;
        }

        // Token: 0x0600059E RID: 1438 RVA: 0x00015A38 File Offset: 0x00013C38
        public void Execute(StoryPlayerCgCommand block)
        {
            if (block.Operation == StoryPlayerCgCommand.CGOperation.Show)
            {
                this.ShowCG(block.Image, block.Layer, block.Position, block.Time ?? 0.25f, block.Scene, block.Mini);
                return;
            }
            if (block.Operation == StoryPlayerCgCommand.CGOperation.Hide)
            {
                this.StopInOutAnimations();
                Game.Screen.UndarkenScreen();
                this.mainCgNode.Modulate = (this.altCgNode.Modulate = this.ColorNotVisible);
                float? time = block.Time;
                float num = 0f;
                StoryPlayerCgCommand.TransitionType transition = (((time.GetValueOrDefault() == num) & (time != null)) ? StoryPlayerCgCommand.TransitionType.Instant : StoryPlayerCgCommand.TransitionType.Fade);
                this.HideCG(this.curCg, transition, block.Time ?? 0.25f);
                this.curCg = null;
                return;
            }
            if (block.Operation == StoryPlayerCgCommand.CGOperation.Pan)
            {
                this.StopPanningAnimations();
                if (this.curCg.IsEmpty() || !(this.curCg.GetChild(0) is TextureRect))
                {
                    Log.Error(new object[] { "Attempt to PAN something that isn't a CG or no CG present" });
                    return;
                }
                TextureRect img = this.curCg.GetChild<TextureRect>(0);
                float height = (float)img.Texture.GetHeight() * Game.Settings.BaseResolution.x / (float)img.Texture.GetWidth();
                float startX = 0f;
                float startY = 0f;
                float endX = 0f;
                float endY = 0f;
                if (block.Direction == Direction.Up)
                {
                    startY = 0f - height + Game.Settings.BaseResolution.y;
                }
                else if (block.Direction == Direction.Down)
                {
                    endY = 0f - height + Game.Settings.BaseResolution.y;
                }
                Game.Animations.Play(new PanAnimationControl(img, new Vector2(startX, startY), new Vector2(endX, endY), block.Time ?? 2f));
            }
        }

        // Token: 0x0600059F RID: 1439 RVA: 0x00015C5C File Offset: 0x00013E5C
        private void ShowCG(string image, CgDisplayManager.CgLayer layer, StoryPlayerCgCommand.CGPosition position, float duration, bool scene, bool mini)
        {
            this.StopInOutAnimations();
            bool flag = this.curCg != null;
            Control newCgNode = (flag ? this.altCgNode : this.mainCgNode);
            if (flag && (this.curCgLayer == layer || layer == CgDisplayManager.CgLayer.BG))
            {
                newCgNode.Modulate = this.ColorVisible;
            }
            else
            {
                newCgNode.Modulate = this.ColorNotVisible;
            }
            if (flag && this.curCgLayer == layer)
            {
                this.MoveUnderCurrentCgNode(newCgNode);
            }
            else if (layer == CgDisplayManager.CgLayer.CG)
            {
                this.MoveToCgPosition(newCgNode);
            }
            else if (layer == CgDisplayManager.CgLayer.BG)
            {
                this.MoveToBgPosition(newCgNode);
            }
            else if (layer == CgDisplayManager.CgLayer.FG)
            {
                this.MoveToFgPosition(newCgNode);
            }
            if (mini)
            {
                Game.Screen.DarkenScreen();
            }
            newCgNode.Clear();
            if (scene)
            {
                Node scn = GDUtil.Instance<Node>("res://resources/nodes/cg/" + image + ".tscn");
                newCgNode.AddChild(scn, false);
            }
            else
            {
                TextureRect img = GDUtil.MakeNode<TextureRect>("Image");
                img.Expand = true;
                img.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
                img.Texture = GD.Load<Texture>("res://assets/img/cg/" + image + ".png");
                newCgNode.AddChild(img, false);
                if (position == StoryPlayerCgCommand.CGPosition.Fill)
                {
                    img.RectSize = Game.Settings.BaseResolution;
                    img.RectPosition = new Vector2(0f, 0f);
                }
                else if (position == StoryPlayerCgCommand.CGPosition.Top || position == StoryPlayerCgCommand.CGPosition.Bottom)
                {
                    float height = (float)img.Texture.GetHeight() * Game.Settings.BaseResolution.x / (float)img.Texture.GetWidth();
                    img.RectSize = new Vector2(Game.Settings.BaseResolution.x, height);
                    float vPos = ((position == StoryPlayerCgCommand.CGPosition.Top) ? 0f : (0f - height + Game.Settings.BaseResolution.y));
                    img.RectPosition = new Vector2(0f, vPos);
                }
                else if (position == StoryPlayerCgCommand.CGPosition.AboveText)
                {
                    float scale = 0.8f;
                    img.RectSize = new Vector2(Game.Settings.BaseResolution.x * scale, Game.Settings.BaseResolution.y * scale);
                    img.RectPosition = new Vector2((1f - scale) * (Game.Settings.BaseResolution.x / 2f), 0f);
                }
            }
            StoryPlayerCgCommand.TransitionType transition = ((duration > 0f) ? StoryPlayerCgCommand.TransitionType.Fade : StoryPlayerCgCommand.TransitionType.Instant);
            if (flag && (this.curCgLayer == layer || layer == CgDisplayManager.CgLayer.BG))
            {
                this.HideCG(this.mainCgNode, transition, duration);
            }
            else
            {
                this.ShowCG(newCgNode, transition, duration);
            }
            this.curCg = newCgNode;
            this.curCgLayer = layer;
            if (flag)
            {
                this.FlipIdentifiers();
            }
        }

        // Token: 0x060005A0 RID: 1440 RVA: 0x00015ED3 File Offset: 0x000140D3
        private void StopInOutAnimations()
        {
            Game.Animations.StopAnimations<FadeAnimation>(this.mainCgNode);
            Game.Animations.StopAnimations<FadeAnimation>(this.altCgNode);
        }

        // Token: 0x060005A1 RID: 1441 RVA: 0x00015EF5 File Offset: 0x000140F5
        private void StopPanningAnimations()
        {
            Game.Animations.StopAnimations<PanAnimationControl>(this.mainCgNode);
            Game.Animations.StopAnimations<PanAnimationControl>(this.altCgNode);
        }

        // Token: 0x060005A2 RID: 1442 RVA: 0x00015F17 File Offset: 0x00014117
        private void MoveToCgPosition(Node node)
        {
            this.storyPlayer.MoveChild(node, this.storyPlayer.nCharRight.GetIndex() + 1);
        }

        // Token: 0x060005A3 RID: 1443 RVA: 0x00015F37 File Offset: 0x00014137
        private void MoveToBgPosition(Node node)
        {
            this.storyPlayer.MoveChild(node, 0);
        }

        // Token: 0x060005A4 RID: 1444 RVA: 0x00015F46 File Offset: 0x00014146
        private void MoveToFgPosition(Node node)
        {
            this.storyPlayer.MoveChild(node, this.storyPlayer.GetChildren().Count - 1);
        }

        // Token: 0x060005A5 RID: 1445 RVA: 0x00015F66 File Offset: 0x00014166
        public void MoveUnderCurrentCgNode(Node node)
        {
            this.storyPlayer.MoveChild(node, this.mainCgNode.GetIndex());
        }

        // Token: 0x060005A6 RID: 1446 RVA: 0x00015F80 File Offset: 0x00014180
        private void FlipIdentifiers()
        {
            Control control = this.mainCgNode;
            Control control2 = this.altCgNode;
            this.altCgNode = control;
            this.mainCgNode = control2;
        }

        // Token: 0x060005A7 RID: 1447 RVA: 0x00015FA9 File Offset: 0x000141A9
        private void ShowCG(Control img, StoryPlayerCgCommand.TransitionType mode, float duration)
        {
            if (mode == StoryPlayerCgCommand.TransitionType.Fade)
            {
                Game.Animations.Play(new FadeInAnimation(img, duration));
                return;
            }
            img.Modulate = this.ColorVisible;
        }

        // Token: 0x060005A8 RID: 1448 RVA: 0x00015FCC File Offset: 0x000141CC
        private void HideCG(Control img, StoryPlayerCgCommand.TransitionType mode, float duration)
        {
            if (!img.IsValid())
            {
                Log.Warn(new object[] { "Attempted to hide a CG when no CG is being shown." });
                return;
            }
            if (mode == StoryPlayerCgCommand.TransitionType.Fade)
            {
                Game.Animations.Play(new FadeOutAnimation(img, duration));
                return;
            }
            img.Modulate = this.ColorNotVisible;
        }

        // Token: 0x040004A0 RID: 1184
        private const float DefaultFadeDuration = 0.25f;

        // Token: 0x040004A1 RID: 1185
        private const float DefaultPanDuration = 2f;

        // Token: 0x040004A2 RID: 1186
        private readonly Color ColorNotVisible = UIUtil.Transparent;

        // Token: 0x040004A3 RID: 1187
        private readonly Color ColorVisible = Colors.White;

        // Token: 0x040004A4 RID: 1188
        private StoryPlayer storyPlayer;

        // Token: 0x040004A5 RID: 1189
        private Control mainCgNode;

        // Token: 0x040004A6 RID: 1190
        private Control altCgNode;

        // Token: 0x040004A7 RID: 1191
        private CgDisplayManager.CgLayer curCgLayer;

        // Token: 0x040004A8 RID: 1192
        private Control curCg;

        // Token: 0x02000247 RID: 583
        public enum CgLayer
        {
            // Token: 0x04000CF1 RID: 3313
            CG,
            // Token: 0x04000CF2 RID: 3314
            BG,
            // Token: 0x04000CF3 RID: 3315
            FG
        }
    }
}
