using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	public class EventTriggerPushable : EventTrigger, IEditorArea
	{
		[Export(PropertyHint.None, "")]
		public string[] Nodes;

		private CollisionObject2D _collisionNode;

		private CollisionShape2D _shapeNode;

		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		public override void _EnterTree()
		{
			UpdateValues();
			if (!Engine.EditorHint && !Game.Room.Cutscene)
			{
				_collisionNode = new Area2D();
				_collisionNode.Name = base.Name;
				_collisionNode.CollisionLayer = 0u;
				_collisionNode.CollisionMask = (base.Enabled ? 16u : 0u);
				AddChild(_collisionNode);
				_shapeNode = new CollisionShape2D();
				RectangleShape2D rect = new RectangleShape2D();
				rect.Extents = Area / 2f;
				_shapeNode.Shape = rect;
				_collisionNode.AddChild(_shapeNode);
				_collisionNode.Connect("body_entered", this, "TriggerEvent");
				Game.Events.PreloadEvent(base.EffectiveEventName);
			}
		}

		public override void UpdateValues()
		{
			if (_collisionNode.IsValid())
			{
				_collisionNode.CollisionLayer = (base.Enabled ? 1u : 0u);
			}
			if (Engine.EditorHint)
			{
				Update();
			}
		}

		public void TriggerEvent(Node node)
		{
			if (Nodes.IsNullOrEmpty() || Nodes.Contains(node.Name))
			{
				Game.Events.PlayEvent(base.EffectiveEventName);
			}
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
