using Godot;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class WaterTilesFix : TileMap
	{
		public override void _Process(float _)
		{
			(base.Material as ShaderMaterial).SetShaderParam("camera_matrix", GetViewport().CanvasTransform.AffineInverse());
		}
	}
}
