// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.GameOnlyVisibility
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "visible")]
  public class GameOnlyVisibility : Node2D
  {
    public override void _EnterTree() => this.Visible = !Engine.EditorHint;
  }
}
