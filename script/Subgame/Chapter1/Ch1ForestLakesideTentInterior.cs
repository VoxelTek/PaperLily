// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakesideTentInterior
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakesideTentInterior : GameRoom
  {
    [LacieEngine.API.GetNode("Main/Terrarium")]
    private Node2D nTerrarium;
    [LacieEngine.API.GetNode("Other/Events/misc_terrarium")]
    private EventTrigger nTerrariumEvt;
    private PVar varTookCaterpillar = (PVar) "ch1.forest_moths_took_caterpillar";

    public override void _UpdateRoom()
    {
      this.nTerrarium.Visible = !(bool) this.varTookCaterpillar.Value;
      this.nTerrariumEvt.Enabled = !(bool) this.varTookCaterpillar.Value;
    }
  }
}
