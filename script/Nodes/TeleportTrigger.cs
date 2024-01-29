using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "portal")]
	public class TeleportTrigger : Node2D, IEditorArea, IToggleable
	{
		public class TeleportArea : Area2D
		{
			private TeleportTrigger _teleport;

			public TeleportArea(TeleportTrigger teleport)
			{
				_teleport = teleport;
			}

			public override void _Ready()
			{
				Connect("body_entered", this, "Teleport");
			}

			public void Teleport(object body)
			{
				if (body is IPlayer player)
				{
					if (_teleport.Relative)
					{
						player.Node.Position += _teleport.Target;
					}
					else
					{
						player.Node.Position = _teleport.Target;
					}
					player.Teleport();
				}
			}
		}

		private bool _enabled = true;

		private TeleportArea _collisionNode;

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
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		[Export(PropertyHint.None, "")]
		public Vector2 Target { get; set; } = Vector2.Zero;


		[Export(PropertyHint.None, "")]
		public bool Relative { get; set; }

		public override void _EnterTree()
		{
			if (!Engine.EditorHint)
			{
				_collisionNode = new TeleportArea(this);
				_collisionNode.CollisionMask = 0u;
				_collisionNode.AddChild(GDUtil.MakeCollisionRect(Area, this.GetPixelPerfectOffset()));
				AddChild(_collisionNode);
				UpdateValues();
			}
		}

		private void UpdateValues()
		{
			_collisionNode.CollisionLayer = (_enabled ? 1u : 0u);
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
