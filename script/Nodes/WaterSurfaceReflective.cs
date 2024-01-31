// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.WaterSurfaceReflective
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
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
        this.Centered = false;
        if (this.Texture == null)
          this.Texture = GD.Load<Texture>("res://assets/sprite/common/tiles_white.png");
        if (this.Material == null)
        {
          this._shader = GDUtil.MakeResource<ShaderMaterial>();
          this._shader.Shader = GD.Load<Shader>("res://resources/material/water_reflective.gdshader");
          this._shader.SetShaderParam("noise", (object) GD.Load<Texture>("res://resources/shader/water_reflective_noisetexture.tres"));
          this.Material = (Material) this._shader;
        }
        this._shader = (ShaderMaterial) this.Material;
        this.PropertyListChangedNotify();
      }
      this._shader = (ShaderMaterial) this.Material;
      this.AdjustParams();
    }

    public void AdjustParams()
    {
      this._shader.SetShaderParam("scale", (object) this.Scale);
      this._shader.SetShaderParam("y_zoom", (object) this.GetViewportTransform().y.y);
    }
  }
}
