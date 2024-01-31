using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
    // Token: 0x02000167 RID: 359
    [Tool]
    [ExportType(icon = "portal")]
    public class TeleportTrigger : Node2D, IEditorArea, IToggleable
    {
        // Token: 0x17000214 RID: 532
        // (get) Token: 0x06000C74 RID: 3188 RVA: 0x00030EDA File Offset: 0x0002F0DA
        // (set) Token: 0x06000C75 RID: 3189 RVA: 0x00030EE2 File Offset: 0x0002F0E2
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

        // Token: 0x17000215 RID: 533
        // (get) Token: 0x06000C76 RID: 3190 RVA: 0x00030EF1 File Offset: 0x0002F0F1
        // (set) Token: 0x06000C77 RID: 3191 RVA: 0x00030EF9 File Offset: 0x0002F0F9
        [Export(PropertyHint.None, "")]
        public Vector2 Area { get; set; } = new Vector2(32f, 32f);

        // Token: 0x17000216 RID: 534
        // (get) Token: 0x06000C78 RID: 3192 RVA: 0x00030F02 File Offset: 0x0002F102
        // (set) Token: 0x06000C79 RID: 3193 RVA: 0x00030F0A File Offset: 0x0002F10A
        [Export(PropertyHint.None, "")]
        public Vector2 Target { get; set; } = Vector2.Zero;

        // Token: 0x17000217 RID: 535
        // (get) Token: 0x06000C7A RID: 3194 RVA: 0x00030F13 File Offset: 0x0002F113
        // (set) Token: 0x06000C7B RID: 3195 RVA: 0x00030F1B File Offset: 0x0002F11B
        [Export(PropertyHint.None, "")]
        public bool Relative { get; set; }

        // Token: 0x06000C7C RID: 3196 RVA: 0x00030F24 File Offset: 0x0002F124
        public override void _EnterTree()
        {
            if (Engine.EditorHint)
            {
                return;
            }
            this._collisionNode = new TeleportTrigger.TeleportArea(this);
            this._collisionNode.CollisionMask = 0U;
            this._collisionNode.AddChild(GDUtil.MakeCollisionRect(this.Area, this.GetPixelPerfectOffset()), false);
            base.AddChild(this._collisionNode, false);
            this.UpdateValues();
        }

        // Token: 0x06000C7D RID: 3197 RVA: 0x00030F81 File Offset: 0x0002F181
        private void UpdateValues()
        {
            this._collisionNode.CollisionLayer = ((this._enabled) ? 1U : 0U);
        }

        // Token: 0x06000C7F RID: 3199 RVA: 0x00030FC6 File Offset: 0x0002F1C6
        void IEditorArea.Update()
        {
            base.Update();
        }

        // Token: 0x04000A09 RID: 2569
        private bool _enabled = true;

        // Token: 0x04000A0A RID: 2570
        private TeleportTrigger.TeleportArea _collisionNode;

        // Token: 0x020002AD RID: 685
        public class TeleportArea : Area2D
        {
            // Token: 0x06001303 RID: 4867 RVA: 0x00045106 File Offset: 0x00043306
            public TeleportArea(TeleportTrigger teleport)
            {
                this._teleport = teleport;
            }

            // Token: 0x06001304 RID: 4868 RVA: 0x00045115 File Offset: 0x00043315
            public override void _Ready()
            {
                base.Connect("body_entered", this, "Teleport", null, 0U);
            }

            // Token: 0x06001305 RID: 4869 RVA: 0x0004512C File Offset: 0x0004332C
            public void Teleport(object body)
            {
                IPlayer player = body as IPlayer;
                if (player != null)
                {
                    if (this._teleport.Relative)
                    {
                        player.Node.Position += this._teleport.Target;
                    }
                    else
                    {
                        player.Node.Position = this._teleport.Target;
                    }
                    player.Teleport();
                }
            }

            // Token: 0x04000E6E RID: 3694
            private TeleportTrigger _teleport;
        }
    }
}
