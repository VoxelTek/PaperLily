// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1MkLivingroom
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
  public class Ch1MkLivingroom : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private AudioStream bgmLongplay;
    [LacieEngine.API.GetNode("Background/WallFireplace/Fire")]
    private Node2D nFire;
    [LacieEngine.API.GetNode("Main/plants_left/MiscPlant4")]
    private Sprite nPlant4;
    private PVar varFireOn = (PVar) "ch1.mk_fireplace_on";
    private PVar varLongplay = (PVar) "ch1.mk_gramophone_playing";
    private PVar varPlantsDead = (PVar) "ch1.mk_plants_dead";
    private PVar varUsedFlower1 = (PVar) "ch1.mk_took_flowers_livingroom";
    private PVar varUsedFlower2 = (PVar) "ch1.mk_took_flowers_hallway";
    private PVar varUsedFlower3 = (PVar) "ch1.mk_took_flowers_bathroom";

    public override void _UpdateRoom()
    {
      this.nFire.Visible = (bool) this.varFireOn.Value;
      if ((bool) this.varLongplay.Value)
        Game.Audio.PlayBgm(this.bgmLongplay, 1f, true);
      if ((bool) this.varPlantsDead.Value)
        this.nPlant4.Frame = 3;
      else if ((bool) this.varUsedFlower1.Value || (bool) this.varUsedFlower2.Value)
        this.nPlant4.Frame = 1;
      else if ((bool) this.varUsedFlower3.Value)
        this.nPlant4.Frame = 2;
      else
        this.nPlant4.Frame = 0;
    }
  }
}
