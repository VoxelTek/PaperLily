using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
    // Token: 0x0200008C RID: 140
    [Serializable]
    public class StoryPlayerBubbleCommand : StoryPlayerCommand
    {
        // Token: 0x170000A4 RID: 164
        // (get) Token: 0x06000487 RID: 1159 RVA: 0x000131EF File Offset: 0x000113EF
        // (set) Token: 0x06000488 RID: 1160 RVA: 0x000131F7 File Offset: 0x000113F7
        public string Bubble { get; set; }

        // Token: 0x170000A5 RID: 165
        // (get) Token: 0x06000489 RID: 1161 RVA: 0x00013200 File Offset: 0x00011400
        // (set) Token: 0x0600048A RID: 1162 RVA: 0x00013208 File Offset: 0x00011408
        public string TargetNode { get; set; }

        // Token: 0x170000A6 RID: 166
        // (get) Token: 0x0600048B RID: 1163 RVA: 0x00013211 File Offset: 0x00011411
        // (set) Token: 0x0600048C RID: 1164 RVA: 0x00013219 File Offset: 0x00011419
        public Vector2? Offset { get; set; }

        // Token: 0x170000A7 RID: 167
        // (get) Token: 0x0600048D RID: 1165 RVA: 0x00013222 File Offset: 0x00011422
        // (set) Token: 0x0600048E RID: 1166 RVA: 0x0001322A File Offset: 0x0001142A
        public float? Time { get; set; }

        // Token: 0x170000A8 RID: 168
        // (get) Token: 0x0600048F RID: 1167 RVA: 0x00013233 File Offset: 0x00011433
        // (set) Token: 0x06000490 RID: 1168 RVA: 0x0001323B File Offset: 0x0001143B
        public bool ContinueImmediately { get; set; }

        // Token: 0x06000491 RID: 1169 RVA: 0x00013244 File Offset: 0x00011444
        public override void Execute(StoryPlayer storyPlayer)
        {
            TargetNode ??= "Player";
            Offset ??= DEFAULT_OFFSET_NPC;
            if (Game.Room.FindNodeInRoom(TargetNode) == null)
            {
                Log.Error("Invalid parent node for bubble: ", TargetNode);
                storyPlayer.SetNextBlockContinue();
                storyPlayer.Next();
                return;
            }
            var root = Game.Room.FindNodeInRoom(TargetNode) as Node2D;
            var bubble = GDUtil.Instance<Node2D>("res://resources/nodes/common/bubble/" + Bubble + ".tscn");
            storyPlayer.HideAllUI();
            bubble.Position = root.GlobalPosition + Offset.Value;
            Game.CurrentRoom.AddChild(bubble, false);
            float time = Time ?? DefaultWaitTime;
            storyPlayer.SetNextBlockContinue();
            if (ContinueImmediately || time <= 0f)
            {
                storyPlayer.Next();
                return;
            }
            storyPlayer.NextAfterSeconds(time);
        }

        // Token: 0x06000492 RID: 1170 RVA: 0x0001337A File Offset: 0x0001157A
        public override IList<string> GetDependencies()
        {
            return new List<string> { "res://resources/nodes/common/bubble/" + Bubble + ".tscn" };
        }

        // Token: 0x04000433 RID: 1075
        private const float DefaultWaitTime = 1.5f;

        // Token: 0x04000434 RID: 1076
        private static readonly Vector2 DEFAULT_OFFSET_NPC = new Vector2(0f, -65f);

        // Token: 0x04000435 RID: 1077
        private static readonly Vector2 DEFAULT_OFFSET_POINT = Vector2.Zero;
    }
}
