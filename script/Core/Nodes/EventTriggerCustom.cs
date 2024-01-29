using Godot;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
	[Tool]
	public class EventTriggerCustom : EventTrigger
	{
		private static readonly Color SolidModulate = new Color(0f, 0f, 1f);

		private static readonly Color NonSolidModulate = new Color(1f, 1f, 0f);

		private CollisionObject2D _collisionNode;

		public override void _EnterTree()
		{
			UpdateValues();
			if (Engine.EditorHint || Game.Room.Cutscene)
			{
				return;
			}
			if (base.Solid)
			{
				_collisionNode = new StaticBody2D();
			}
			else
			{
				_collisionNode = new Area2D();
			}
			_collisionNode.Name = base.Name;
			_collisionNode.CollisionLayer = (base.Enabled ? 1u : 0u);
			_collisionNode.CollisionMask = 0u;
			AddChild(_collisionNode);
			foreach (Node node in GetChildren())
			{
				if (node is CollisionShape2D || node is CollisionPolygon2D)
				{
					node.Reparent(_collisionNode);
				}
			}
			Direction direction = Direction.FromByte(base.Directions);
			StoryPlayerEventTrigger trigger = ((base.Trigger != 0) ? StoryPlayerEventTrigger.Touch : StoryPlayerEventTrigger.Action);
			Game.Events.AddTrigger(base.Name, base.EffectiveEventName, trigger, direction);
		}

		public override void UpdateValues()
		{
			if (_collisionNode.IsValid())
			{
				_collisionNode.CollisionLayer = (base.Enabled ? 1u : 0u);
			}
		}
	}
}
