using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "stairs")]
	public class Stairs : Node2D, IEditorArea, IToggleable
	{
		public enum EType
		{
			Horizontal,
			Vertical
		}

		public class StairsArea : Area2D, IFloorMovementModifier
		{
			private Stairs _stairs;

			private static readonly Vector2 YSpeedMultiplier = new Vector2(1f, 0.6f);

			public StairsArea(Stairs stairs)
			{
				_stairs = stairs;
			}

			public override void _Ready()
			{
				if (_stairs.Passthrough)
				{
					Connect("body_entered", this, "StartCollisionBypass");
					Connect("body_exited", this, "EndCollisionBypass");
				}
			}

			public Vector2 GetModifiedMovement(Vector2 movement)
			{
				if (_stairs.Type == EType.Horizontal)
				{
					if (movement.x == 0f)
					{
						return movement;
					}
					return movement.FlattenX().Rotated(Mathf.Deg2Rad(_stairs.Degrees) * (float)(_stairs.InvertX ? 1 : (-1))) + movement.FlattenY();
				}
				if (_stairs.Type == EType.Vertical)
				{
					return movement * YSpeedMultiplier;
				}
				return movement;
			}

			public void StartCollisionBypass(object body)
			{
				if (body is WalkingCharacter w)
				{
					w.CollisionMask |= 8u;
					w.CollisionMask &= 4294967292u;
				}
			}

			public void EndCollisionBypass(object body)
			{
				if (body is WalkingCharacter w)
				{
					w.CollisionMask |= 3u;
					w.CollisionMask &= 4294967287u;
				}
			}
		}

		private bool _enabled = true;

		private StairsArea _collisionNode;

		[Export(PropertyHint.None, "")]
		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.None, "")]
		public EType Type { get; set; }

		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		[Export(PropertyHint.None, "")]
		public float Degrees { get; set; } = 45f;


		[Export(PropertyHint.None, "")]
		public bool InvertX { get; set; }

		[Export(PropertyHint.None, "")]
		public bool Passthrough { get; set; }

		public override void _EnterTree()
		{
			if (!Engine.EditorHint)
			{
				_collisionNode = new StairsArea(this);
				_collisionNode.CollisionLayer = (Enabled ? 1u : 0u);
				if (Passthrough && Enabled)
				{
					_collisionNode.CollisionLayer += 8u;
				}
				_collisionNode.CollisionMask = 0u;
				AddChild(_collisionNode);
				CollisionShape2D _shapeNode = GDUtil.MakeCollisionRect(Area, this.GetPixelPerfectOffset());
				_collisionNode.AddChild(_shapeNode);
			}
		}

		public void UpdateValues()
		{
			if (_collisionNode.IsValid())
			{
				_collisionNode.CollisionLayer = (Enabled ? 1u : 0u);
				if (Passthrough && Enabled)
				{
					_collisionNode.CollisionLayer += 8u;
				}
			}
			if (Engine.EditorHint)
			{
				Update();
			}
		}

		public bool OverlapsBody(Node body)
		{
			if (_collisionNode.IsValid())
			{
				return _collisionNode.OverlapsBody(body);
			}
			return false;
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
