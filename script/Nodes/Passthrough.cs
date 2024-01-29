using Godot;
using LacieEngine.API;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "ghost")]
	public class Passthrough : Node2D, IEditorArea, IToggleable
	{
		public class PassthroughArea : Area2D
		{
			private Passthrough _parent;

			private PassthroughArea()
			{
			}

			public PassthroughArea(Passthrough parent)
			{
				base.Name = "PassthroughArea";
				_parent = parent;
			}

			public override void _Ready()
			{
				Connect("body_entered", this, "StartCollisionBypass");
				Connect("body_exited", this, "EndCollisionBypass");
			}

			public void StartCollisionBypass(object body)
			{
				if (_parent.Enabled && body is WalkingCharacter w)
				{
					w.CollisionMask |= 8u;
					w.CollisionMask &= 4294967292u;
				}
			}

			public void EndCollisionBypass(object body)
			{
				if (_parent.Enabled && body is WalkingCharacter w)
				{
					w.CollisionMask |= 3u;
					w.CollisionMask &= 4294967287u;
				}
			}
		}

		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		public override void _EnterTree()
		{
			if (!Engine.EditorHint)
			{
				PassthroughArea _collisionNode = new PassthroughArea(this);
				_collisionNode.CollisionLayer = 9u;
				_collisionNode.CollisionMask = 0u;
				AddChild(_collisionNode);
				CollisionShape2D _shapeNode = new CollisionShape2D();
				RectangleShape2D rect = new RectangleShape2D();
				rect.Extents = Area / 2f;
				_shapeNode.Shape = rect;
				_shapeNode.Position = this.GetPixelPerfectOffset();
				_collisionNode.AddChild(_shapeNode);
			}
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
