using System;
using Godot;
using LacieEngine.API;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
    // Token: 0x020001AC RID: 428
    [Tool]
    public class EventTriggerCircular : EventTrigger, IEditorArea
    {
        // Token: 0x17000295 RID: 661
        // (get) Token: 0x06000EFD RID: 3837 RVA: 0x0003938B File Offset: 0x0003758B
        // (set) Token: 0x06000EFE RID: 3838 RVA: 0x00039393 File Offset: 0x00037593
        [Export(PropertyHint.None, "")]
        public Vector2 Area {
            get {
                return this._area;
            }
            set {
                this._area = value;
                this.UpdateValues();
            }
        }

        // Token: 0x06000EFF RID: 3839 RVA: 0x000393A4 File Offset: 0x000375A4
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
            CollisionShape2D collisionShape = GDUtil.MakeCollisionCapsule(this.Area, this.GetPixelPerfectOffset());
            this._collisionNode.AddChild(collisionShape, false);
            if (base.Trigger != EventTrigger.ETrigger.Item)
            {
                Direction direction = Direction.FromByte(base.Directions);
                StoryPlayerEventTrigger trigger = ((base.Trigger == EventTrigger.ETrigger.Action) ? StoryPlayerEventTrigger.Action : StoryPlayerEventTrigger.Touch);
                Game.Events.AddTrigger(base.Name, base.EffectiveEventName, trigger, direction);
            }
        }

        // Token: 0x06000F00 RID: 3840 RVA: 0x00039483 File Offset: 0x00037683
        public override void _Ready()
        {
            base.Priority = base.GetIndex();
        }

        // Token: 0x06000F01 RID: 3841 RVA: 0x00039491 File Offset: 0x00037691
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

        // Token: 0x06000F02 RID: 3842 RVA: 0x000394C4 File Offset: 0x000376C4
        private Vector2 GetPixelPerfectOffset()
        {
            return new Vector2((this.Area.x % 2f != 0f) ? 0.5f : 0f, (this.Area.y % 2f != 0f) ? 0.5f : 0f);
        }

        // Token: 0x06000F04 RID: 3844 RVA: 0x0003953B File Offset: 0x0003773B
        void IEditorArea.Update()
        {
            base.Update();
        }

        // Token: 0x04000B28 RID: 2856
        private Vector2 _area = new Vector2(32f, 32f);

        // Token: 0x04000B29 RID: 2857
        private CollisionObject2D _collisionNode;
    }
}
