// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestPath3
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestPath3 : GameRoom
  {
    [LacieEngine.API.GetNode("Ground/Carpet")]
    private Sprite nCarpet;
    [LacieEngine.API.GetNode("Ground/Carpet/Shiny")]
    private Sprite nKeyShiny;
    private PVar varRevealedKey = (PVar) "ch1.forest_lockedsite_revealed_key_3";
    private PVar varTookKey = (PVar) "ch1.forest_lockedsite_took_key_3";

    public override void _UpdateRoom()
    {
      this.nCarpet.Frame = (bool) this.varRevealedKey.Value ? 1 : 0;
      this.nKeyShiny.Visible = (bool) this.varRevealedKey.Value && !(bool) this.varTookKey.Value;
    }
  }
}
