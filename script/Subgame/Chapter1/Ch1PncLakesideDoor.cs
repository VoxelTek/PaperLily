using Godot;
using LacieEngine.API;
using LacieEngine.Core;

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

		[GetNode("Targets/Slot1")]
		private TextureRect nSlot1;

		[GetNode("Targets/Slot2")]
		private TextureRect nSlot2;

		[GetNode("Targets/Slot3")]
		private TextureRect nSlot3;

		[GetNode("Targets/Slot4")]
		private TextureRect nSlot4;

		[GetNode("Targets/Slot5")]
		private TextureRect nSlot5;

		[GetNode("Targets/Slot6")]
		private TextureRect nSlot6;

		private PVar varDoorknob1 = "ch1.forest_lakeside_doorknob_1";

		private PVar varDoorknob2 = "ch1.forest_lakeside_doorknob_2";

		private PVar varDoorknob3 = "ch1.forest_lakeside_doorknob_3";

		private PVar varDoorknob4 = "ch1.forest_lakeside_doorknob_4";

		private PVar varDoorknob5 = "ch1.forest_lakeside_doorknob_5";

		private PVar varDoorknob6 = "ch1.forest_lakeside_doorknob_6";

		private PVar varDoorknobToPlace = "ch1.temp_doorknob_to_place";

		private PEvent evtNoKnob = "pnc_door_slot";

		private PEvent evtFacilityKnob = "pnc_door_facility";

		private PEvent evtGardenKnob = "pnc_door_garden";

		private PEvent evtLibraryKnob = "pnc_door_library";

		private PEvent evtGraveKnob = "pnc_door_grave";

		private PEvent evtMothsKnob = "pnc_door_moths";

		private PEvent evtShopKnob = "pnc_door_shop";

		private PEvent evtGoldKnob = "pnc_door_gold";

		private PEvent evtExitKnob = "pnc_door_exit";

		private PEvent evtIntro = "pnc_door_intro";

		private PVar _lastDoorknobInteracted;

		public override void Init()
		{
			base.Init();
			RefreshDoorknobs();
		}

		public void Start()
		{
			if (!Game.Events.SeenEvent(evtIntro.Id))
			{
				evtIntro.Play();
			}
		}

		public void Slot1()
		{
			ExecuteSlot(varDoorknob1);
		}

		public void Slot2()
		{
			ExecuteSlot(varDoorknob2);
		}

		public void Slot3()
		{
			ExecuteSlot(varDoorknob3);
		}

		public void Slot4()
		{
			ExecuteSlot(varDoorknob4);
		}

		public void Slot5()
		{
			ExecuteSlot(varDoorknob5);
		}

		public void Slot6()
		{
			ExecuteSlot(varDoorknob6);
		}

		public void PlaceDoorknob()
		{
			_lastDoorknobInteracted.NewValue = varDoorknobToPlace.Value;
			RefreshDoorknobs();
		}

		private void RefreshDoorknobs()
		{
			UpdateTextureWithDoorknob(nSlot1, varDoorknob1);
			UpdateTextureWithDoorknob(nSlot2, varDoorknob2);
			UpdateTextureWithDoorknob(nSlot3, varDoorknob3);
			UpdateTextureWithDoorknob(nSlot4, varDoorknob4);
			UpdateTextureWithDoorknob(nSlot5, varDoorknob5);
			UpdateTextureWithDoorknob(nSlot6, varDoorknob6);
		}

		private void UpdateTextureWithDoorknob(TextureRect slot, PVar var)
		{
			string text = var.Value;
			if (text == null)
			{
				goto IL_014b;
			}
			switch (text.Length)
			{
			case 4:
				break;
			case 5:
				goto IL_005c;
			case 8:
				goto IL_00b5;
			case 6:
				goto IL_00c7;
			case 7:
				goto IL_00d6;
			default:
				goto IL_014b;
			}
			char c = text[0];
			Texture texture;
			if (c != 'e')
			{
				if (c != 'g')
				{
					if (c != 's' || !(text == "shop"))
					{
						goto IL_014b;
					}
					texture = texShopDoorknob;
				}
				else
				{
					if (!(text == "gold"))
					{
						goto IL_014b;
					}
					texture = texGoldDoorknob;
				}
			}
			else
			{
				if (!(text == "exit"))
				{
					goto IL_014b;
				}
				texture = texExitDoorknob;
			}
			goto IL_0152;
			IL_005c:
			c = text[0];
			if (c != 'g')
			{
				if (c != 'm' || !(text == "moths"))
				{
					goto IL_014b;
				}
				texture = texMothsDoorknob;
			}
			else
			{
				if (!(text == "grave"))
				{
					goto IL_014b;
				}
				texture = texGraveDoorknob;
			}
			goto IL_0152;
			IL_00b5:
			if (!(text == "facility"))
			{
				goto IL_014b;
			}
			texture = texFacilityDoorknob;
			goto IL_0152;
			IL_014b:
			texture = texNoDoorknob;
			goto IL_0152;
			IL_00d6:
			if (!(text == "library"))
			{
				goto IL_014b;
			}
			texture = texLibraryDoorknob;
			goto IL_0152;
			IL_0152:
			slot.Texture = texture;
			return;
			IL_00c7:
			if (!(text == "garden"))
			{
				goto IL_014b;
			}
			texture = texGardenDoorknob;
			goto IL_0152;
		}

		private void ExecuteSlot(PVar var)
		{
			_lastDoorknobInteracted = var;
			string text = var.Value;
			if (text == null)
			{
				goto IL_0149;
			}
			switch (text.Length)
			{
			case 4:
				break;
			case 5:
				goto IL_005d;
			case 8:
				goto IL_00b3;
			case 6:
				goto IL_00c5;
			case 7:
				goto IL_00d4;
			default:
				goto IL_0149;
			}
			char c = text[0];
			PEvent pEvent;
			if (c != 'e')
			{
				if (c != 'g')
				{
					if (c != 's' || !(text == "shop"))
					{
						goto IL_0149;
					}
					pEvent = evtShopKnob;
				}
				else
				{
					if (!(text == "gold"))
					{
						goto IL_0149;
					}
					pEvent = evtGoldKnob;
				}
			}
			else
			{
				if (!(text == "exit"))
				{
					goto IL_0149;
				}
				pEvent = evtExitKnob;
			}
			goto IL_0150;
			IL_005d:
			c = text[0];
			if (c != 'g')
			{
				if (c != 'm' || !(text == "moths"))
				{
					goto IL_0149;
				}
				pEvent = evtMothsKnob;
			}
			else
			{
				if (!(text == "grave"))
				{
					goto IL_0149;
				}
				pEvent = evtGraveKnob;
			}
			goto IL_0150;
			IL_00b3:
			if (!(text == "facility"))
			{
				goto IL_0149;
			}
			pEvent = evtFacilityKnob;
			goto IL_0150;
			IL_0149:
			pEvent = evtNoKnob;
			goto IL_0150;
			IL_00d4:
			if (!(text == "library"))
			{
				goto IL_0149;
			}
			pEvent = evtLibraryKnob;
			goto IL_0150;
			IL_0150:
			pEvent.Play();
			return;
			IL_00c5:
			if (!(text == "garden"))
			{
				goto IL_0149;
			}
			pEvent = evtGardenKnob;
			goto IL_0150;
		}
	}
}
