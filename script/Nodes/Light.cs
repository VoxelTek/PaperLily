using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "light")]
	public class Light : Light2D, IInspectorCustomizer
	{
		public enum LightBlendMode
		{
			Auto,
			Add,
			Sub,
			Mix
		}

		public enum LightZPosition
		{
			Floor,
			Wall,
			Ceiling,
			Sky
		}

		public enum LightLayers
		{
			All,
			EnvironmentOnly,
			CharacterOnly,
			Custom
		}

		private const string DEFAULT_LIGHT_SHAPE = "res://assets/sprite/common/light01.png";

		private LightBlendMode _blendMode;

		private LightZPosition _height;

		private LightLayers _layers;

		[Export(PropertyHint.None, "")]
		public new LightBlendMode BlendMode
		{
			get
			{
				return _blendMode;
			}
			set
			{
				_blendMode = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.None, "")]
		public LightZPosition Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.None, "")]
		public LightLayers Layers
		{
			get
			{
				return _layers;
			}
			set
			{
				_layers = value;
				UpdateValues();
			}
		}

		public override void _EnterTree()
		{
			UpdateValues();
		}

		private void UpdateValues()
		{
			if (!IsInsideTree())
			{
				return;
			}
			if (base.Texture == null)
			{
				Texture texture2 = (base.Texture = GD.Load<Texture>("res://assets/sprite/common/light01.png"));
			}
			switch (BlendMode)
			{
			case LightBlendMode.Auto:
			{
				base.Mode = ModeEnum.Add;
				GameRoom room = (Engine.EditorHint ? (GetTree().EditedSceneRoot as GameRoom) : Game.CurrentRoom);
				if (room != null && room.Lighting != null)
				{
					base.Mode = room.Lighting.EnvironmentLightMode.ToGodotLight2DMode();
				}
				break;
			}
			case LightBlendMode.Add:
				base.Mode = ModeEnum.Add;
				break;
			case LightBlendMode.Sub:
				base.Mode = ModeEnum.Sub;
				break;
			case LightBlendMode.Mix:
				base.Mode = ModeEnum.Mix;
				break;
			}
			switch (Height)
			{
			case LightZPosition.Floor:
				base.RangeHeight = 0f;
				break;
			case LightZPosition.Wall:
				base.RangeHeight = 100f;
				break;
			case LightZPosition.Ceiling:
				base.RangeHeight = 300f;
				break;
			case LightZPosition.Sky:
				base.RangeHeight = 100000f;
				break;
			}
			switch (Layers)
			{
			case LightLayers.All:
				base.RangeItemCullMask = 3;
				break;
			case LightLayers.EnvironmentOnly:
				base.RangeItemCullMask = 1;
				break;
			case LightLayers.CharacterOnly:
				base.RangeItemCullMask = 2;
				break;
			}
			PropertyListChangedNotify();
		}
	}
}
