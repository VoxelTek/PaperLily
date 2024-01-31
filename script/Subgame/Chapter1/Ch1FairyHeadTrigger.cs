using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
    // Token: 0x020001CD RID: 461
    [Tool]
    [ExportType(icon = "skull")]
    public class Ch1FairyHeadTrigger : Node2D, IEditorArea
    {
        // Token: 0x170002F6 RID: 758
        // (get) Token: 0x06001056 RID: 4182 RVA: 0x0003BF8F File Offset: 0x0003A18F
        // (set) Token: 0x06001057 RID: 4183 RVA: 0x0003BF97 File Offset: 0x0003A197
        [Export(PropertyHint.None, "")]
        public string Event { get; set; } = "ch1_death_impact";

        // Token: 0x170002F7 RID: 759
        // (get) Token: 0x06001058 RID: 4184 RVA: 0x0003BFA0 File Offset: 0x0003A1A0
        // (set) Token: 0x06001059 RID: 4185 RVA: 0x0003BFA8 File Offset: 0x0003A1A8
        [Export(PropertyHint.None, "")]
        public bool Enabled { get; set; } = true;

        // Token: 0x170002F8 RID: 760
        // (get) Token: 0x0600105A RID: 4186 RVA: 0x0003BFB1 File Offset: 0x0003A1B1
        // (set) Token: 0x0600105B RID: 4187 RVA: 0x0003BFB9 File Offset: 0x0003A1B9
        [Export(PropertyHint.None, "")]
        public Ch1FairyHeadTrigger.EAttackType AttackType { get; set; }

        // Token: 0x170002F9 RID: 761
        // (get) Token: 0x0600105C RID: 4188 RVA: 0x0003BFC2 File Offset: 0x0003A1C2
        // (set) Token: 0x0600105D RID: 4189 RVA: 0x0003BFCA File Offset: 0x0003A1CA
        [Export(PropertyHint.None, "")]
        public NodePath HeadNode { get; private set; } = new NodePath();

        // Token: 0x170002FA RID: 762
        // (get) Token: 0x0600105E RID: 4190 RVA: 0x0003BFD3 File Offset: 0x0003A1D3
        // (set) Token: 0x0600105F RID: 4191 RVA: 0x0003BFDB File Offset: 0x0003A1DB
        [Export(PropertyHint.None, "")]
        public NodePath AnimationNode { get; private set; } = new NodePath();

        // Token: 0x170002FB RID: 763
        // (get) Token: 0x06001060 RID: 4192 RVA: 0x0003BFE4 File Offset: 0x0003A1E4
        // (set) Token: 0x06001061 RID: 4193 RVA: 0x0003BFEC File Offset: 0x0003A1EC
        [Export(PropertyHint.None, "")]
        public Vector2 Area { get; set; } = new Vector2(32f, 32f);

        // Token: 0x06001062 RID: 4194 RVA: 0x0003BFF8 File Offset: 0x0003A1F8
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
            this._collisionNode = new Area2D();
            this._collisionNode.Name = base.Name;
            this._collisionNode.CollisionLayer = ((this.Enabled) ? 1U : 0U);
            this._collisionNode.CollisionMask = 0U;
            base.AddChild(this._collisionNode, false);
            this._shapeNode = GDUtil.MakeCollisionRect(this.Area, this.GetPixelPerfectOffset());
            this._collisionNode.AddChild(this._shapeNode, false);
            this._collisionNode.Connect("body_entered", this, "TriggerAttack", null, 0U);
        }

        // Token: 0x06001063 RID: 4195 RVA: 0x0003C0A8 File Offset: 0x0003A2A8
        public override void _Process(float delta)
        {
            if (this._animLungeStarted)
            {
                Node2D animationNode = base.GetNode(this.AnimationNode) as Node2D;
                animationNode.Position += Vector2.Down * delta * 700f;
                if (animationNode.Position.y >= Game.Player.Node.Position.y)
                {
                    this._animLungeStarted = false;
                    animationNode.Position = animationNode.Position.ReplaceY(Game.Player.Node.Position.y - 1f);
                    Game.Audio.PlaySfx(this.sfxDeath);
                    Game.Events.PlayEvent(this.Event);
                }
            }
        }

        // Token: 0x06001064 RID: 4196 RVA: 0x0003C16B File Offset: 0x0003A36B
        public void UpdateValues()
        {
            if (this._collisionNode.IsValid())
            {
                this._collisionNode.CollisionLayer = ((this.Enabled) ? 1U : 0U);
            }
            if (Engine.EditorHint)
            {
                base.Update();
            }
        }

        // Token: 0x06001065 RID: 4197 RVA: 0x0003C19C File Offset: 0x0003A39C
        public async void TriggerAttack(Node _)
        {
            Game.InputProcessor = Inputs.Processor.None;
            if (this.AttackType == Ch1FairyHeadTrigger.EAttackType.Ground)
            {
                (base.GetNode(this.HeadNode) as IAnimation2D).Play();
                await DrkieUtil.DelaySeconds(0.4);
                Node2D node2D = base.GetNode(this.AnimationNode) as Node2D;
                node2D.Position = Game.Player.Node.Position;
                node2D.Visible = true;
                IAnimation2D animation = node2D as IAnimation2D;
                if (animation != null)
                {
                    animation.Play();
                }
                await DrkieUtil.DelaySeconds(0.5);
                Game.Audio.PlaySfx(this.sfxDeath);
                Game.Events.PlayEvent(this.Event);
            }
            else if (this.AttackType == Ch1FairyHeadTrigger.EAttackType.Lunge)
            {
                Node2D headNode = base.GetNode(this.HeadNode) as Node2D;
                headNode.Visible = false;
                Node2D node2D2 = base.GetNode(this.AnimationNode) as Node2D;
                node2D2.Position = headNode.Position;
                node2D2.Visible = true;
                this._animLungeStarted = true;
            }
        }

        // Token: 0x06001067 RID: 4199 RVA: 0x0003C224 File Offset: 0x0003A424
        void IEditorArea.Update()
        {
            base.Update();
        }

        // Token: 0x04000BBA RID: 3002
        [Export(PropertyHint.None, "")]
        public AudioStream sfxDeath;

        // Token: 0x04000BBB RID: 3003
        private CollisionObject2D _collisionNode;

        // Token: 0x04000BBC RID: 3004
        private CollisionShape2D _shapeNode;

        // Token: 0x04000BBD RID: 3005
        private bool _animLungeStarted;

        // Token: 0x020002E2 RID: 738
        public enum EAttackType
        {
            // Token: 0x04000FD0 RID: 4048
            Ground,
            // Token: 0x04000FD1 RID: 4049
            Lunge
        }
    }
}
