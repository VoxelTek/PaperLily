using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "square")]
	public class AreaActionTrigger : Node2D, IEditorArea, IToggleable
	{
		public class ActionTriggerArea : Area2D
		{
			private AreaActionTrigger _parent;

			public ActionTriggerArea(AreaActionTrigger teleport)
			{
				_parent = teleport;
			}

			public override void _Ready()
			{
				if (!_parent.ActionEntered.IsNullOrEmpty())
				{
					Connect("body_entered", this, "Entered");
				}
				if (!_parent.ActionExited.IsNullOrEmpty())
				{
					Connect("body_exited", this, "Exited");
				}
			}

			public void Entered(object body)
			{
				if (body is IPlayer)
				{
					ExecuteAction(_parent.ActionEntered);
				}
			}

			public void Exited(object body)
			{
				if (body is IPlayer)
				{
					ExecuteAction(_parent.ActionExited);
				}
			}

			private void ExecuteAction(NodePath actionPath)
			{
				if (!actionPath.IsNullOrEmpty() && _parent.GetNode(actionPath) is IAction action)
				{
					action.Execute();
				}
			}
		}

		private bool _enabled = true;

		private ActionTriggerArea _collisionNode;

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
		public NodePath ActionEntered { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public NodePath ActionExited { get; set; } = new NodePath();


		public override void _EnterTree()
		{
			if (!Engine.EditorHint)
			{
				_collisionNode = new ActionTriggerArea(this);
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
