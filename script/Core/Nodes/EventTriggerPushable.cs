using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
    // Token: 0x020001B2 RID: 434
    [Tool]
    public class EventTriggerPushable : EventTrigger, IEditorArea
    {
        // Token: 0x1700029B RID: 667
        // (get) Token: 0x06000F19 RID: 3865 RVA: 0x00039868 File Offset: 0x00037A68
        // (set) Token: 0x06000F1A RID: 3866 RVA: 0x00039870 File Offset: 0x00037A70
        [Export(PropertyHint.None, "")]
        public Vector2 Area { get; set; } = new Vector2(32f, 32f);

        // Token: 0x06000F1B RID: 3867 RVA: 0x0003987C File Offset: 0x00037A7C
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
            this._collisionNode = new Area2D();
            this._collisionNode.Name = base.Name;
            this._collisionNode.CollisionLayer = 0U;
            this._collisionNode.CollisionMask = (base.Enabled ? 16U : 0U);
            base.AddChild(this._collisionNode, false);
            this._shapeNode = new CollisionShape2D();
            RectangleShape2D rect = new RectangleShape2D();
            rect.Extents = this.Area / 2f;
            this._shapeNode.Shape = rect;
            this._collisionNode.AddChild(this._shapeNode, false);
            this._collisionNode.Connect("body_entered", this, "TriggerEvent", null, 0U);
            Game.Events.PreloadEvent(base.EffectiveEventName);
        }

        // Token: 0x06000F1C RID: 3868 RVA: 0x0003995F File Offset: 0x00037B5F
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

        // Token: 0x06000F1D RID: 3869 RVA: 0x0003998F File Offset: 0x00037B8F
        public void TriggerEvent(Node node)
        {
            if (!this.Nodes.IsNullOrEmpty<string>() && !this.Nodes.Contains(node.Name))
            {
                return;
            }
            Game.Events.PlayEvent(base.EffectiveEventName);
        }

        // Token: 0x06000F1F RID: 3871 RVA: 0x000399DF File Offset: 0x00037BDF
        void IEditorArea.Update()
        {
            base.Update();
        }

        // Token: 0x04000B32 RID: 2866
        [Export(PropertyHint.None, "")]
        public string[] Nodes;

        // Token: 0x04000B33 RID: 2867
        private CollisionObject2D _collisionNode;

        // Token: 0x04000B34 RID: 2868
        private CollisionShape2D _shapeNode;
    }
}
