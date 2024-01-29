using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "bolt")]
	public abstract class EventTrigger : Node2D, IToggleable, IEventTriggerNode
	{
		public enum ETrigger
		{
			Action,
			Touch,
			Item
		}

		public static readonly Color SolidEventColor = new Color(0f, 0f, 1f, 0.42f);

		public static readonly Color NonSolidEventColor = new Color(1f, 1f, 0f, 0.42f);

		public static readonly Color ContactEventColor = new Color(1f, 0f, 0f, 0.4f);

		private bool _enabled = true;

		private bool _solid = true;

		private ETrigger _trigger;

		[Export(PropertyHint.None, "")]
		public string Event { get; set; } = "";


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
		public bool Solid
		{
			get
			{
				return _solid;
			}
			set
			{
				_solid = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.None, "")]
		public ETrigger Trigger
		{
			get
			{
				return _trigger;
			}
			set
			{
				_trigger = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.Flags, "Left,Up,Right,Down")]
		public byte Directions { get; set; }

		[Export(PropertyHint.None, "")]
		public NodePath RelatedNode { get; private set; } = new NodePath();


		public string EffectiveEventName
		{
			get
			{
				if (!Event.IsNullOrEmpty())
				{
					return Event;
				}
				return base.Name;
			}
		}

		public int Priority { get; set; }

		public abstract void UpdateValues();
	}
}
