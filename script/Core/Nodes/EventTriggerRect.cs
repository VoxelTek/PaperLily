using System;
using Godot;
using LacieEngine.API;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
    // Token: 0x020001B3 RID: 435
    [Tool]
    public class EventTriggerRect : EventTrigger, IEditorArea
    {
        // Token: 0x1700029C RID: 668
        // (get) Token: 0x06000F20 RID: 3872 RVA: 0x000399E7 File Offset: 0x00037BE7
        // (set) Token: 0x06000F21 RID: 3873 RVA: 0x000399EF File Offset: 0x00037BEF
        [Export(PropertyHint.None, "")]
        public Vector2 Area { get; set; } = new Vector2(32f, 32f);

        // Token: 0x06000F22 RID: 3874 RVA: 0x000399F8 File Offset: 0x00037BF8
        public override void _EnterTree()
        {
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
            this._shapeNode = GDUtil.MakeCollisionRect(this.Area, this.GetPixelPerfectOffset());
            this._collisionNode.AddChild(this._shapeNode, false);
            if (base.Trigger != EventTrigger.ETrigger.Item)
            {
                Direction direction = Direction.FromByte(base.Directions);
                StoryPlayerEventTrigger trigger = ((base.Trigger == EventTrigger.ETrigger.Action) ? StoryPlayerEventTrigger.Action : StoryPlayerEventTrigger.Touch);
                Game.Events.AddTrigger(base.Name, base.EffectiveEventName, trigger, direction);
            }
        }

        // Token: 0x06000F23 RID: 3875 RVA: 0x00039ADB File Offset: 0x00037CDB
        public override void _Ready()
        {
            base.Priority = base.GetIndex();
        }

        // Token: 0x06000F24 RID: 3876 RVA: 0x00039AE9 File Offset: 0x00037CE9
        public override void UpdateValues()
        {
            if (this._collisionNode.IsValid())
            {
                this._collisionNode.CollisionLayer = ((base.Enabled) ? 1U : 0U);
            }
            if (Engine.EditorHint)
            {
                base.Update();
            }
        }

        // Token: 0x06000F26 RID: 3878 RVA: 0x00039B36 File Offset: 0x00037D36
        void IEditorArea.Update()
        {
            base.Update();
        }

        // Token: 0x04000B36 RID: 2870
        private CollisionObject2D _collisionNode;

        // Token: 0x04000B37 RID: 2871
        private CollisionShape2D _shapeNode;
    }
}
