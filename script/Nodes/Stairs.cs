using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	// Token: 0x02000166 RID: 358
	[Tool]
	[ExportType(icon = "stairs")]
	public class Stairs : Node2D, IEditorArea, IToggleable
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00030D2C File Offset: 0x0002EF2C
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x00030D34 File Offset: 0x0002EF34
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00030D43 File Offset: 0x0002EF43
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x00030D4B File Offset: 0x0002EF4B
		[Export(PropertyHint.None, "")]
		public Stairs.EType Type { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00030D54 File Offset: 0x0002EF54
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x00030D5C File Offset: 0x0002EF5C
		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00030D65 File Offset: 0x0002EF65
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x00030D6D File Offset: 0x0002EF6D
		[Export(PropertyHint.None, "")]
		public float Degrees { get; set; } = 45f;

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00030D76 File Offset: 0x0002EF76
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00030D7E File Offset: 0x0002EF7E
		[Export(PropertyHint.None, "")]
		public bool InvertX { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00030D87 File Offset: 0x0002EF87
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x00030D8F File Offset: 0x0002EF8F
		[Export(PropertyHint.None, "")]
		public bool Passthrough { get; set; }

		// Token: 0x06000C6F RID: 3183 RVA: 0x00030D98 File Offset: 0x0002EF98
		public override void _EnterTree()
		{
			if (Engine.EditorHint)
			{
				return;
			}
			this._collisionNode = new Stairs.StairsArea(this);
			this._collisionNode.CollisionLayer = ((this.Enabled) ? 1U : 0U);
			if (this.Passthrough && this.Enabled)
			{
				this._collisionNode.CollisionLayer += 8U;
			}
			this._collisionNode.CollisionMask = 0U;
			base.AddChild(this._collisionNode, false);
			CollisionShape2D _shapeNode = GDUtil.MakeCollisionRect(this.Area, this.GetPixelPerfectOffset());
			this._collisionNode.AddChild(_shapeNode, false);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00030E28 File Offset: 0x0002F028
		public void UpdateValues()
		{
			if (this._collisionNode.IsValid())
			{
				this._collisionNode.CollisionLayer = ((this.Enabled) ? 1U : 0U);
				if (this.Passthrough && this.Enabled)
				{
					this._collisionNode.CollisionLayer += 8U;
				}
			}
			if (Engine.EditorHint)
			{
				base.Update();
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00030E86 File Offset: 0x0002F086
		public bool OverlapsBody(Node body)
		{
			return this._collisionNode.IsValid() && this._collisionNode.OverlapsBody(body);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00030ED2 File Offset: 0x0002F0D2
		void IEditorArea.Update()
		{
			base.Update();
		}

		// Token: 0x04000A04 RID: 2564
		private bool _enabled = true;

		// Token: 0x04000A05 RID: 2565
		private Stairs.StairsArea _collisionNode;

		// Token: 0x020002AB RID: 683
		public enum EType
		{
			// Token: 0x04000E6A RID: 3690
			Horizontal,
			// Token: 0x04000E6B RID: 3691
			Vertical
		}

		// Token: 0x020002AC RID: 684
		public class StairsArea : Area2D, IFloorMovementModifier
		{
			// Token: 0x060012FD RID: 4861 RVA: 0x00044FBE File Offset: 0x000431BE
			public StairsArea(Stairs stairs)
			{
				this._stairs = stairs;
			}

			// Token: 0x060012FE RID: 4862 RVA: 0x00044FCD File Offset: 0x000431CD
			public override void _Ready()
			{
				if (this._stairs.Passthrough)
				{
					base.Connect("body_entered", this, "StartCollisionBypass", null, 0U);
					base.Connect("body_exited", this, "EndCollisionBypass", null, 0U);
				}
			}

			// Token: 0x060012FF RID: 4863 RVA: 0x00045004 File Offset: 0x00043204
			public Vector2 GetModifiedMovement(Vector2 movement)
			{
				if (this._stairs.Type == Stairs.EType.Horizontal)
				{
					if (movement.x == 0f)
					{
						return movement;
					}
					return movement.FlattenX().Rotated(Mathf.Deg2Rad(this._stairs.Degrees) * (float)(this._stairs.InvertX ? 1 : (-1))) + movement.FlattenY();
				}
				else
				{
					if (this._stairs.Type == Stairs.EType.Vertical)
					{
						return movement * Stairs.StairsArea.YSpeedMultiplier;
					}
					return movement;
				}
			}

			// Token: 0x06001300 RID: 4864 RVA: 0x00045088 File Offset: 0x00043288
			public void StartCollisionBypass(object body)
			{
				WalkingCharacter w = body as WalkingCharacter;
				if (w != null)
				{
					w.CollisionMask |= 8U;
					w.CollisionMask &= 4294967292U;
				}
			}

			// Token: 0x06001301 RID: 4865 RVA: 0x000450BC File Offset: 0x000432BC
			public void EndCollisionBypass(object body)
			{
				WalkingCharacter w = body as WalkingCharacter;
				if (w != null)
				{
					w.CollisionMask |= 3U;
					w.CollisionMask &= 4294967287U;
				}
			}

			// Token: 0x04000E6C RID: 3692
			private Stairs _stairs;

			// Token: 0x04000E6D RID: 3693
			private static readonly Vector2 YSpeedMultiplier = new Vector2(1f, 0.6f);
		}
	}
}
