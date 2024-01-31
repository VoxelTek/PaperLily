using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
    // Token: 0x02000150 RID: 336
    [Tool]
    [ExportType(icon = "square")]
    public class AreaActionTrigger : Node2D, IEditorArea, IToggleable
    {
        // Token: 0x170001E7 RID: 487
        // (get) Token: 0x06000B8E RID: 2958 RVA: 0x0002DEC0 File Offset: 0x0002C0C0
        // (set) Token: 0x06000B8F RID: 2959 RVA: 0x0002DEC8 File Offset: 0x0002C0C8
        [Export(PropertyHint.None, "")]
        public bool Enabled {
            get {
                return this._enabled;
            }
            set {
                this._enabled = value;
                this.UpdateValues();
            }
        }

        // Token: 0x170001E8 RID: 488
        // (get) Token: 0x06000B90 RID: 2960 RVA: 0x0002DED7 File Offset: 0x0002C0D7
        // (set) Token: 0x06000B91 RID: 2961 RVA: 0x0002DEDF File Offset: 0x0002C0DF
        [Export(PropertyHint.None, "")]
        public Vector2 Area { get; set; } = new Vector2(32f, 32f);

        // Token: 0x170001E9 RID: 489
        // (get) Token: 0x06000B92 RID: 2962 RVA: 0x0002DEE8 File Offset: 0x0002C0E8
        // (set) Token: 0x06000B93 RID: 2963 RVA: 0x0002DEF0 File Offset: 0x0002C0F0
        [Export(PropertyHint.None, "")]
        public NodePath ActionEntered { get; set; } = new NodePath();

        // Token: 0x170001EA RID: 490
        // (get) Token: 0x06000B94 RID: 2964 RVA: 0x0002DEF9 File Offset: 0x0002C0F9
        // (set) Token: 0x06000B95 RID: 2965 RVA: 0x0002DF01 File Offset: 0x0002C101
        [Export(PropertyHint.None, "")]
        public NodePath ActionExited { get; set; } = new NodePath();

        // Token: 0x06000B96 RID: 2966 RVA: 0x0002DF0C File Offset: 0x0002C10C
        public override void _EnterTree()
        {
            if (Engine.EditorHint)
            {
                return;
            }
            this._collisionNode = new AreaActionTrigger.ActionTriggerArea(this);
            this._collisionNode.CollisionMask = 0U;
            this._collisionNode.AddChild(GDUtil.MakeCollisionRect(this.Area, this.GetPixelPerfectOffset()), false);
            base.AddChild(this._collisionNode, false);
            this.UpdateValues();
        }

        // Token: 0x06000B97 RID: 2967 RVA: 0x0002DF69 File Offset: 0x0002C169
        private void UpdateValues()
        {
            this._collisionNode.CollisionLayer = ((this._enabled) ? 1U : 0U);
        }

        // Token: 0x06000B99 RID: 2969 RVA: 0x0002DFB9 File Offset: 0x0002C1B9
        void IEditorArea.Update()
        {
            base.Update();
        }

        // Token: 0x0400099B RID: 2459
        private bool _enabled = true;

        // Token: 0x0400099C RID: 2460
        private AreaActionTrigger.ActionTriggerArea _collisionNode;

        // Token: 0x020002A3 RID: 675
        public class ActionTriggerArea : Area2D
        {
            // Token: 0x060012E5 RID: 4837 RVA: 0x0004414E File Offset: 0x0004234E
            public ActionTriggerArea(AreaActionTrigger teleport)
            {
                this._parent = teleport;
            }

            // Token: 0x060012E6 RID: 4838 RVA: 0x00044160 File Offset: 0x00042360
            public override void _Ready()
            {
                if (!this._parent.ActionEntered.IsNullOrEmpty())
                {
                    base.Connect("body_entered", this, "Entered", null, 0U);
                }
                if (!this._parent.ActionExited.IsNullOrEmpty())
                {
                    base.Connect("body_exited", this, "Exited", null, 0U);
                }
            }

            // Token: 0x060012E7 RID: 4839 RVA: 0x000441B9 File Offset: 0x000423B9
            public void Entered(object body)
            {
                if (body is IPlayer)
                {
                    this.ExecuteAction(this._parent.ActionEntered);
                }
            }

            // Token: 0x060012E8 RID: 4840 RVA: 0x000441D4 File Offset: 0x000423D4
            public void Exited(object body)
            {
                if (body is IPlayer)
                {
                    this.ExecuteAction(this._parent.ActionExited);
                }
            }

            // Token: 0x060012E9 RID: 4841 RVA: 0x000441F0 File Offset: 0x000423F0
            private void ExecuteAction(NodePath actionPath)
            {
                if (actionPath.IsNullOrEmpty())
                {
                    return;
                }
                IAction action = this._parent.GetNode(actionPath) as IAction;
                if (action == null)
                {
                    return;
                }
                action.Execute();
            }

            // Token: 0x04000E4A RID: 3658
            private AreaActionTrigger _parent;
        }
    }
}
