// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1PncLakesideDoor
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Minigames
{
  public class Ch1PncLakesideDoor : PointAndClick, IMinigame
  {
    [Export(PropertyHint.None, "")]
    private Texture texNoDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texExitDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texFacilityDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texGardenDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texLibraryDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texGraveDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texMothsDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texShopDoorknob;
    [Export(PropertyHint.None, "")]
    private Texture texGoldDoorknob;
    [LacieEngine.API.GetNode("Targets/Slot1")]
    private TextureRect nSlot1;
    [LacieEngine.API.GetNode("Targets/Slot2")]
    private TextureRect nSlot2;
    [LacieEngine.API.GetNode("Targets/Slot3")]
    private TextureRect nSlot3;
    [LacieEngine.API.GetNode("Targets/Slot4")]
    private TextureRect nSlot4;
    [LacieEngine.API.GetNode("Targets/Slot5")]
    private TextureRect nSlot5;
    [LacieEngine.API.GetNode("Targets/Slot6")]
    private TextureRect nSlot6;
    private PVar varDoorknob1 = (PVar) "ch1.forest_lakeside_doorknob_1";
    private PVar varDoorknob2 = (PVar) "ch1.forest_lakeside_doorknob_2";
    private PVar varDoorknob3 = (PVar) "ch1.forest_lakeside_doorknob_3";
    private PVar varDoorknob4 = (PVar) "ch1.forest_lakeside_doorknob_4";
    private PVar varDoorknob5 = (PVar) "ch1.forest_lakeside_doorknob_5";
    private PVar varDoorknob6 = (PVar) "ch1.forest_lakeside_doorknob_6";
    private PVar varDoorknobToPlace = (PVar) "ch1.temp_doorknob_to_place";
    private PEvent evtNoKnob = (PEvent) "pnc_door_slot";
    private PEvent evtFacilityKnob = (PEvent) "pnc_door_facility";
    private PEvent evtGardenKnob = (PEvent) "pnc_door_garden";
    private PEvent evtLibraryKnob = (PEvent) "pnc_door_library";
    private PEvent evtGraveKnob = (PEvent) "pnc_door_grave";
    private PEvent evtMothsKnob = (PEvent) "pnc_door_moths";
    private PEvent evtShopKnob = (PEvent) "pnc_door_shop";
    private PEvent evtGoldKnob = (PEvent) "pnc_door_gold";
    private PEvent evtExitKnob = (PEvent) "pnc_door_exit";
    private PEvent evtIntro = (PEvent) "pnc_door_intro";
    private PVar _lastDoorknobInteracted;

    public override void Init()
    {
      base.Init();
      this.RefreshDoorknobs();
    }

    public void Start()
    {
      if (Game.Events.SeenEvent(this.evtIntro.Id))
        return;
      this.evtIntro.Play();
    }

    public void Slot1() => this.ExecuteSlot(this.varDoorknob1);

    public void Slot2() => this.ExecuteSlot(this.varDoorknob2);

    public void Slot3() => this.ExecuteSlot(this.varDoorknob3);

    public void Slot4() => this.ExecuteSlot(this.varDoorknob4);

    public void Slot5() => this.ExecuteSlot(this.varDoorknob5);

    public void Slot6() => this.ExecuteSlot(this.varDoorknob6);

    public void PlaceDoorknob()
    {
      this._lastDoorknobInteracted.NewValue = (PVar.PVarSetProxy) this.varDoorknobToPlace.Value;
      this.RefreshDoorknobs();
    }

    private void RefreshDoorknobs()
    {
      this.UpdateTextureWithDoorknob(this.nSlot1, this.varDoorknob1);
      this.UpdateTextureWithDoorknob(this.nSlot2, this.varDoorknob2);
      this.UpdateTextureWithDoorknob(this.nSlot3, this.varDoorknob3);
      this.UpdateTextureWithDoorknob(this.nSlot4, this.varDoorknob4);
      this.UpdateTextureWithDoorknob(this.nSlot5, this.varDoorknob5);
      this.UpdateTextureWithDoorknob(this.nSlot6, this.varDoorknob6);
    }

    private void UpdateTextureWithDoorknob(TextureRect slot, PVar var)
    {
      TextureRect textureRect = slot;
      string str = (string) var.Value;
      Texture texture;
      if (str != null)
      {
        switch (str.Length)
        {
          case 4:
            switch (str[0])
            {
              case 'e':
                if (str == "exit")
                {
                  texture = this.texExitDoorknob;
                  goto label_21;
                }
                else
                  break;
              case 'g':
                if (str == "gold")
                {
                  texture = this.texGoldDoorknob;
                  goto label_21;
                }
                else
                  break;
              case 's':
                if (str == "shop")
                {
                  texture = this.texShopDoorknob;
                  goto label_21;
                }
                else
                  break;
            }
            break;
          case 5:
            switch (str[0])
            {
              case 'g':
                if (str == "grave")
                {
                  texture = this.texGraveDoorknob;
                  goto label_21;
                }
                else
                  break;
              case 'm':
                if (str == "moths")
                {
                  texture = this.texMothsDoorknob;
                  goto label_21;
                }
                else
                  break;
            }
            break;
          case 6:
            if (str == "garden")
            {
              texture = this.texGardenDoorknob;
              goto label_21;
            }
            else
              break;
          case 7:
            if (str == "library")
            {
              texture = this.texLibraryDoorknob;
              goto label_21;
            }
            else
              break;
          case 8:
            if (str == "facility")
            {
              texture = this.texFacilityDoorknob;
              goto label_21;
            }
            else
              break;
        }
      }
      texture = this.texNoDoorknob;
label_21:
      textureRect.Texture = texture;
    }

    private void ExecuteSlot(PVar var)
    {
      this._lastDoorknobInteracted = var;
      string str = (string) var.Value;
      PEvent pevent;
      if (str != null)
      {
        switch (str.Length)
        {
          case 4:
            switch (str[0])
            {
              case 'e':
                if (str == "exit")
                {
                  pevent = this.evtExitKnob;
                  goto label_21;
                }
                else
                  break;
              case 'g':
                if (str == "gold")
                {
                  pevent = this.evtGoldKnob;
                  goto label_21;
                }
                else
                  break;
              case 's':
                if (str == "shop")
                {
                  pevent = this.evtShopKnob;
                  goto label_21;
                }
                else
                  break;
            }
            break;
          case 5:
            switch (str[0])
            {
              case 'g':
                if (str == "grave")
                {
                  pevent = this.evtGraveKnob;
                  goto label_21;
                }
                else
                  break;
              case 'm':
                if (str == "moths")
                {
                  pevent = this.evtMothsKnob;
                  goto label_21;
                }
                else
                  break;
            }
            break;
          case 6:
            if (str == "garden")
            {
              pevent = this.evtGardenKnob;
              goto label_21;
            }
            else
              break;
          case 7:
            if (str == "library")
            {
              pevent = this.evtLibraryKnob;
              goto label_21;
            }
            else
              break;
          case 8:
            if (str == "facility")
            {
              pevent = this.evtFacilityKnob;
              goto label_21;
            }
            else
              break;
        }
      }
      pevent = this.evtNoKnob;
label_21:
      pevent.Play();
    }
  }
}
