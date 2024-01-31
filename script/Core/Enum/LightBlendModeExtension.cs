// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.LightBlendModeExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Core
{
  public static class LightBlendModeExtension
  {
    public static Light2D.ModeEnum ToGodotLight2DMode(this LightBlendMode value)
    {
      Light2D.ModeEnum godotLight2Dmode;
      switch (value)
      {
        case LightBlendMode.Add:
          godotLight2Dmode = Light2D.ModeEnum.Add;
          break;
        case LightBlendMode.Sub:
          godotLight2Dmode = Light2D.ModeEnum.Sub;
          break;
        case LightBlendMode.Mix:
          godotLight2Dmode = Light2D.ModeEnum.Mix;
          break;
        default:
          godotLight2Dmode = Light2D.ModeEnum.Add;
          break;
      }
      return godotLight2Dmode;
    }
  }
}
