using Godot;
using LacieEngine.API;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
	[Tool]
	public class EventTriggerCircular : EventTrigger, IEditorArea
	{
		private Vector2 _area = new Vector2(32f, 32f);

		private CollisionObject2D _collisionNode;

		[Export(PropertyHint.None, "")]
		public Vector2 Area
		{
			get
			{
				return _area;
			}
			set
			{
				_area = value;
				UpdateValues();
			}
		}

		public override void _EnterTree()
		{
			UpdateValues();
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
				CollisionShape2D collisionShape = GDUtil.MakeCollisionCapsule(Area, GetPixelPerfectOffset());
				_collisionNode.AddChild(collisionShape);
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

		private Vector2 GetPixelPerfectOffset()
		{
			return new Vector2((Area.x % 2f != 0f) ? 0.5f : 0f, (Area.y % 2f != 0f) ? 0.5f : 0f);
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
