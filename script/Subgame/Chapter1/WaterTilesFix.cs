// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.WaterTilesFix
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class WaterTilesFix : TileMap
  {
    public override void _Process(float _)
    {
      (this.Material as ShaderMaterial).SetShaderParam("camera_matrix", (object) this.GetViewport().CanvasTransform.AffineInverse());
    }
  }
}
