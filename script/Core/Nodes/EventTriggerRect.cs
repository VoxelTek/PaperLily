using Godot;
using LacieEngine.API;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
	[Tool]
	public class EventTriggerRect : EventTrigger, IEditorArea
	{
		private CollisionObject2D _collisionNode;

		private CollisionShape2D _shapeNode;

		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		public override void _EnterTree()
		{
			if (!Engine.EditorHint && !Game.Room.Cutscene)
			{
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
				_shapeNode = GDUtil.MakeCollisionRect(Area, this.GetPixelPerfectOffset());
				_collisionNode.AddChild(_shapeNode);
				if (base.Trigger != ETrigger.Item)
				{
					Direction direction = Direction.FromByte(base.Directions);
					StoryPlayerEventTrigger trigger = ((base.Trigger != 0) ? StoryPlayerEventTrigger.Touch : StoryPlayerEventTrigger.Action);
					Game.Events.AddTrigger(base.Name, base.EffectiveEventName, trigger, direction);
				}
			}
		}

		public override void _Ready()
		{
			base.Priority = GetIndex();
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

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
