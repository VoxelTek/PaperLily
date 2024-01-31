// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1MkB1fHallway
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1MkB1fHallway : GameRoom
  {
    [LacieEngine.API.GetNode("TilesFgTrapped")]
    private TileMap nTilesTrapped;
    [LacieEngine.API.GetNode("Other/Events/misc_wall_trapped")]
    private EventTrigger nWallTrapped;

    public override void _UpdateRoom()
    {
      if (!Game.Variables.GetFlag("ch1.mk_trapped"))
        return;
      this.nTilesTrapped.Visible = true;
      this.nTilesTrapped.CollisionLayer = 2U;
      this.nWallTrapped.Enabled = true;
    }
  }
}
