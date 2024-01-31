// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.Light
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  [ExportType(icon = "light")]
  public class Light : Light2D, IInspectorCustomizer
  {
    private const string DEFAULT_LIGHT_SHAPE = "res://assets/sprite/common/light01.png";
    private Light.LightBlendMode _blendMode;
    private Light.LightZPosition _height;
    private Light.LightLayers _layers;

    [Export(PropertyHint.None, "")]
    public Light.LightBlendMode BlendMode
    {
      get => this._blendMode;
      set
      {
        this._blendMode = value;
        this.UpdateValues();
      }
    }

    [Export(PropertyHint.None, "")]
    public Light.LightZPosition Height
    {
      get => this._height;
      set
      {
        this._height = value;
        this.UpdateValues();
      }
    }

    [Export(PropertyHint.None, "")]
    public Light.LightLayers Layers
    {
      get => this._layers;
      set
      {
        this._layers = value;
        this.UpdateValues();
      }
    }

    public override void _EnterTree() => this.UpdateValues();

    private void UpdateValues()
    {
      if (!this.IsInsideTree())
        return;
      if (this.Texture == null)
        this.Texture = GD.Load<Texture>("res://assets/sprite/common/light01.png");
      switch (this.BlendMode)
      {
        case Light.LightBlendMode.Auto:
          this.Mode = Light2D.ModeEnum.Add;
          GameRoom gameRoom = Engine.EditorHint ? this.GetTree().EditedSceneRoot as GameRoom : Game.CurrentRoom;
          if (gameRoom != null && gameRoom.Lighting != null)
          {
            this.Mode = gameRoom.Lighting.EnvironmentLightMode.ToGodotLight2DMode();
            break;
          }
          break;
        case Light.LightBlendMode.Add:
          this.Mode = Light2D.ModeEnum.Add;
          break;
        case Light.LightBlendMode.Sub:
          this.Mode = Light2D.ModeEnum.Sub;
          break;
        case Light.LightBlendMode.Mix:
          this.Mode = Light2D.ModeEnum.Mix;
          break;
      }
      switch (this.Height)
      {
        case Light.LightZPosition.Floor:
          this.RangeHeight = 0.0f;
          break;
        case Light.LightZPosition.Wall:
          this.RangeHeight = 100f;
          break;
        case Light.LightZPosition.Ceiling:
          this.RangeHeight = 300f;
          break;
        case Light.LightZPosition.Sky:
          this.RangeHeight = 100000f;
          break;
      }
      switch (this.Layers)
      {
        case Light.LightLayers.All:
          this.RangeItemCullMask = 3;
          break;
        case Light.LightLayers.EnvironmentOnly:
          this.RangeItemCullMask = 1;
          break;
        case Light.LightLayers.CharacterOnly:
          this.RangeItemCullMask = 2;
          break;
      }
      this.PropertyListChangedNotify();
    }

    public enum LightBlendMode
    {
      Auto,
      Add,
      Sub,
      Mix,
    }

    public enum LightZPosition
    {
      Floor,
      Wall,
      Ceiling,
      Sky,
    }

    public enum LightLayers
    {
      All,
      EnvironmentOnly,
      CharacterOnly,
      Custom,
    }
  }
}
