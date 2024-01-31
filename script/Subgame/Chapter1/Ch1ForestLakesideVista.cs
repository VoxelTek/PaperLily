// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakesideVista
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakesideVista : GameRoom
  {
    [LacieEngine.API.GetNode("Foreground/Paddle")]
    private Node2D nPaddle;
    private PVar varTookPaddle = (PVar) "ch1.forest_lakeside_took_paddle_vista";

    public override void _UpdateRoom() => this.nPaddle.Visible = !(bool) this.varTookPaddle.Value;
  }
}
