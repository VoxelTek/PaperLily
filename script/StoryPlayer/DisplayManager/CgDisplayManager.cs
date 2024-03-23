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
            mainCgNode = storyPlayer.nCg;
            altCgNode = storyPlayer.nCgAlt;
            curCgLayer = CgLayer.CG;
            curCg = null;
        }

        // Token: 0x0600059D RID: 1437 RVA: 0x000159FE File Offset: 0x00013BFE
        public void Reset()
        {
            mainCgNode.Clear();
            altCgNode.Clear();
            mainCgNode.Modulate = ColorNotVisible;
            altCgNode.Modulate = ColorNotVisible;
        }

        // Token: 0x0600059E RID: 1438 RVA: 0x00015A38 File Offset: 0x00013C38
        public void Execute(StoryPlayerCgCommand block)
        {
            if (block.Operation == StoryPlayerCgCommand.CGOperation.Show)
            {
                ShowCG(block.Image, block.Layer, block.Position, block.Time ?? DefaultFadeDuration, block.Scene, block.Mini);
                return;
            }
            if (block.Operation == StoryPlayerCgCommand.CGOperation.Hide)
            {
                StopInOutAnimations();
                Game.Screen.UndarkenScreen();
                mainCgNode.Modulate = (altCgNode.Modulate = ColorNotVisible);
                var time = block.Time;
                var num = 0f;
                var transition = ((time.GetValueOrDefault() == num) & (time != null)) ? StoryPlayerCgCommand.TransitionType.Instant : StoryPlayerCgCommand.TransitionType.Fade;
                HideCG(curCg, transition, block.Time ?? DefaultFadeDuration);
                curCg = null;
                return;
            }
            if (block.Operation == StoryPlayerCgCommand.CGOperation.Pan)
            {
                StopPanningAnimations();
                if (curCg.IsEmpty() || !(curCg.GetChild(0) is TextureRect))
                {
                    Log.Error("Attempt to PAN something that isn't a CG or no CG present");
                    return;
                }
                var img = curCg.GetChild<TextureRect>(0);
                var height = img.Texture.GetHeight() * Game.Settings.BaseResolution.x / img.Texture.GetWidth();
                var startX = 0f;
                var startY = 0f;
                var endX = 0f;
                var endY = 0f;
                if (block.Direction == Direction.Up)
                {
                    startY = 0f - height + Game.Settings.BaseResolution.y;
                }
                else if (block.Direction == Direction.Down)
                {
                    endY = 0f - height + Game.Settings.BaseResolution.y;
                }
                Game.Animations.Play(new PanAnimationControl(img, new Vector2(startX, startY), new Vector2(endX, endY), block.Time ?? DefaultPanDuration));
            }
        }

        // Token: 0x0600059F RID: 1439 RVA: 0x00015C5C File Offset: 0x00013E5C
        private void ShowCG(string image, CgDisplayManager.CgLayer layer, StoryPlayerCgCommand.CGPosition position, float duration, bool scene, bool mini)
        {
            StopInOutAnimations();
            var flag = curCg != null;
            var newCgNode = (flag ? altCgNode : mainCgNode);
            if (flag && (curCgLayer == layer || layer == CgLayer.BG))
            {
                newCgNode.Modulate = ColorVisible;
            }
            else
            {
                newCgNode.Modulate = ColorNotVisible;
            }
            if (flag && curCgLayer == layer)
            {
                MoveUnderCurrentCgNode(newCgNode);
            }
            else if (layer == CgLayer.CG)
            {
                MoveToCgPosition(newCgNode);
            }
            else if (layer == CgLayer.BG)
            {
                MoveToBgPosition(newCgNode);
            }
            else if (layer == CgLayer.FG)
            {
                MoveToFgPosition(newCgNode);
            }
            if (mini)
            {
                Game.Screen.DarkenScreen();
            }
            newCgNode.Clear();
            if (scene)
            {
                var scn = GDUtil.Instance<Node>("res://resources/nodes/cg/" + image + ".tscn");
                newCgNode.AddChild(scn, false);
            }
            else
            {
                var img = GDUtil.MakeNode<TextureRect>("Image");
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
                    var height = img.Texture.GetHeight() * Game.Settings.BaseResolution.x / img.Texture.GetWidth();
                    img.RectSize = new Vector2(Game.Settings.BaseResolution.x, height);
                    var vPos = ((position == StoryPlayerCgCommand.CGPosition.Top) ? 0f : (0f - height + Game.Settings.BaseResolution.y));
                    img.RectPosition = new Vector2(0f, vPos);
                }
                else if (position == StoryPlayerCgCommand.CGPosition.AboveText)
                {
                    var scale = 0.8f;
                    img.RectSize = new Vector2(Game.Settings.BaseResolution.x * scale, Game.Settings.BaseResolution.y * scale);
                    img.RectPosition = new Vector2((1f - scale) * (Game.Settings.BaseResolution.x / DefaultPanDuration), 0f);
                }
            }
            var transition = ((duration > 0f) ? StoryPlayerCgCommand.TransitionType.Fade : StoryPlayerCgCommand.TransitionType.Instant);
            if (flag && (curCgLayer == layer || layer == CgLayer.BG))
            {
                HideCG(mainCgNode, transition, duration);
            }
            else
            {
                ShowCG(newCgNode, transition, duration);
            }
            curCg = newCgNode;
            curCgLayer = layer;
            if (flag)
            {
                FlipIdentifiers();
            }
        }

        // Token: 0x060005A0 RID: 1440 RVA: 0x00015ED3 File Offset: 0x000140D3
        private void StopInOutAnimations()
        {
            Game.Animations.StopAnimations<FadeAnimation>(mainCgNode);
            Game.Animations.StopAnimations<FadeAnimation>(altCgNode);
        }

        // Token: 0x060005A1 RID: 1441 RVA: 0x00015EF5 File Offset: 0x000140F5
        private void StopPanningAnimations()
        {
            Game.Animations.StopAnimations<PanAnimationControl>(mainCgNode);
            Game.Animations.StopAnimations<PanAnimationControl>(altCgNode);
        }

        // Token: 0x060005A2 RID: 1442 RVA: 0x00015F17 File Offset: 0x00014117
        private void MoveToCgPosition(Node node)
        {
            storyPlayer.MoveChild(node, storyPlayer.nCharRight.GetIndex() + 1);
        }

        // Token: 0x060005A3 RID: 1443 RVA: 0x00015F37 File Offset: 0x00014137
        private void MoveToBgPosition(Node node)
        {
            storyPlayer.MoveChild(node, 0);
        }

        // Token: 0x060005A4 RID: 1444 RVA: 0x00015F46 File Offset: 0x00014146
        private void MoveToFgPosition(Node node)
        {
            storyPlayer.MoveChild(node, storyPlayer.GetChildren().Count - 1);
        }

        // Token: 0x060005A5 RID: 1445 RVA: 0x00015F66 File Offset: 0x00014166
        public void MoveUnderCurrentCgNode(Node node)
        {
            storyPlayer.MoveChild(node, mainCgNode.GetIndex());
        }

        // Token: 0x060005A6 RID: 1446 RVA: 0x00015F80 File Offset: 0x00014180
        private void FlipIdentifiers()
        {
            var control = mainCgNode;
            var control2 = altCgNode;
            altCgNode = control;
            mainCgNode = control2;
        }

        // Token: 0x060005A7 RID: 1447 RVA: 0x00015FA9 File Offset: 0x000141A9
        private void ShowCG(Control img, StoryPlayerCgCommand.TransitionType mode, float duration)
        {
            if (mode == StoryPlayerCgCommand.TransitionType.Fade)
            {
                Game.Animations.Play(new FadeInAnimation(img, duration));
                return;
            }
            img.Modulate = ColorVisible;
        }

        // Token: 0x060005A8 RID: 1448 RVA: 0x00015FCC File Offset: 0x000141CC
        private void HideCG(Control img, StoryPlayerCgCommand.TransitionType mode, float duration)
        {
            if (!img.IsValid())
            {
                Log.Warn("Attempted to hide a CG when no CG is being shown.");
                return;
            }
            if (mode == StoryPlayerCgCommand.TransitionType.Fade)
            {
                Game.Animations.Play(new FadeOutAnimation(img, duration));
                return;
            }
            img.Modulate = ColorNotVisible;
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
        private readonly StoryPlayer storyPlayer;

        // Token: 0x040004A5 RID: 1189
        private Control mainCgNode;

        // Token: 0x040004A6 RID: 1190
        private Control altCgNode;

        // Token: 0x040004A7 RID: 1191
        private CgLayer curCgLayer;

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
