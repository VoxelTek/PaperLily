// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB3fMk
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Subgame.PaperLily;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB3fMk : GameRoom
  {
    private PVar varItemStorage = (PVar) "ch1.temp_mk_item_storage";

    public void RemoveInventory() => PaperLilyFunctions.RemoveInventory(this.varItemStorage);

    public void RestoreInventory() => PaperLilyFunctions.RestoreInventory(this.varItemStorage);
  }
}
