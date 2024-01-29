using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "water")]
	public class WaterSurfaceReflective : Sprite
	{
		private ShaderMaterial _shader;

		public override void _EnterTree()
		{
			if (Engine.EditorHint)
			{
				base.Centered = false;
				if (base.Texture == null)
				{
					base.Texture = GD.Load<Texture>("res://assets/sprite/common/tiles_white.png");
				}
				if (base.Material == null)
				{
					_shader = GDUtil.MakeResource<ShaderMaterial>();
					_shader.Shader = GD.Load<Shader>("res://resources/material/water_reflective.gdshader");
					_shader.SetShaderParam("noise", GD.Load<Texture>("res://resources/shader/water_reflective_noisetexture.tres"));
					base.Material = _shader;
				}
				_shader = (ShaderMaterial)base.Material;
				PropertyListChangedNotify();
			}
			_shader = (ShaderMaterial)base.Material;
			AdjustParams();
		}

		public void AdjustParams()
		{
			_shader.SetShaderParam("scale", base.Scale);
			_shader.SetShaderParam("y_zoom", GetViewportTransform().y.y);
		}
	}
}
