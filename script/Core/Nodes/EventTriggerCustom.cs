using System;
using Godot;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
    // Token: 0x020001AD RID: 429
    [Tool]
    public class EventTriggerCustom : EventTrigger
    {
        // Token: 0x06000F05 RID: 3845 RVA: 0x00039544 File Offset: 0x00037744
        public override void _EnterTree()
        {
            this.UpdateValues();
            if (Engine.EditorHint)
            {
                return;
            }
            if (Game.Room.Cutscene)
            {
                return;
            }
            if (base.Solid)
            {
                this._collisionNode = new StaticBody2D();
            }
            else
            {
                this._collisionNode = new Area2D();
            }
            this._collisionNode.Name = base.Name;
            this._collisionNode.CollisionLayer = ((base.Enabled) ? 1U : 0U);
            this._collisionNode.CollisionMask = 0U;
            base.AddChild(this._collisionNode, false);
            foreach (object obj in base.GetChildren())
            {
                Node node = (Node)obj;
                if (node is CollisionShape2D || node is CollisionPolygon2D)
                {
                    node.Reparent(this._collisionNode);
                }
            }
            Direction direction = Direction.FromByte(base.Directions);
            StoryPlayerEventTrigger trigger = ((base.Trigger == EventTrigger.ETrigger.Action) ? StoryPlayerEventTrigger.Action : StoryPlayerEventTrigger.Touch);
            Game.Events.AddTrigger(base.Name, base.EffectiveEventName, trigger, direction);
        }

        // Token: 0x06000F06 RID: 3846 RVA: 0x00039660 File Offset: 0x00037860
        public override void UpdateValues()
        {
            if (this._collisionNode.IsValid())
            {
                this._collisionNode.CollisionLayer = ((base.Enabled) ? 1U : 0U);
            }
        }

        // Token: 0x04000B2A RID: 2858
        private static readonly Color SolidModulate = new Color(0f, 0f, 1f, 1f);

        // Token: 0x04000B2B RID: 2859
        private static readonly Color NonSolidModulate = new Color(1f, 1f, 0f, 1f);

        // Token: 0x04000B2C RID: 2860
        private CollisionObject2D _collisionNode;
    }
}
