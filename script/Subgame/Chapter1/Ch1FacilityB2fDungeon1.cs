using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;
using LacieEngine.Subgame.PaperLily;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1FacilityB2fDungeon1 : Ch1FacilityRoom
	{
		[GetNode("Background/WallDoor1")]
		private SimpleAnimatedSprite nDoor1;

		[GetNode("Background/WallDoor2")]
		private SimpleAnimatedSprite nDoor2;

		[GetNode("Background/WallDoor3")]
		private SimpleAnimatedSprite nDoor3;

		[GetNode("Background/WallDoor4")]
		private SimpleAnimatedSprite nDoor4;

		[GetNode("Background/WallDoor5")]
		private SimpleAnimatedSprite nDoor5;

		[GetNode("Background/WallDoor6")]
		private SimpleAnimatedSprite nDoor6;

		[GetNode("Other/Events/misc_cell_1")]
		private EventTrigger nInteractDoor1;

		[GetNode("Other/Events/misc_cell_2")]
		private EventTrigger nInteractDoor2;

		[GetNode("Other/Events/misc_cell_3")]
		private EventTrigger nInteractDoor3;

		[GetNode("Other/Events/misc_cell_4")]
		private EventTrigger nInteractDoor4;

		[GetNode("Other/Events/misc_cell_5")]
		private EventTrigger nInteractDoor5;

		[GetNode("Other/Events/misc_cell_6")]
		private EventTrigger nInteractDoor6;

		[GetNode("Other/Events/misc_cell_1_close")]
		private EventTrigger nInteractDoor1Close;

		[GetNode("Other/Events/misc_cell_2_close")]
		private EventTrigger nInteractDoor2Close;

		[GetNode("Other/Events/misc_cell_3_close")]
		private EventTrigger nInteractDoor3Close;

		[GetNode("Other/Events/misc_cell_4_close")]
		private EventTrigger nInteractDoor4Close;

		[GetNode("Other/Events/misc_cell_5_close")]
		private EventTrigger nInteractDoor5Close;

		[GetNode("Other/Events/misc_cell_6_close")]
		private EventTrigger nInteractDoor6Close;

		[GetNode("Other/Events/move_cell_1")]
		private EventTrigger nMoveDoor1;

		[GetNode("Other/Events/move_cell_2")]
		private EventTrigger nMoveDoor2;

		[GetNode("Other/Events/move_cell_3")]
		private EventTrigger nMoveDoor3;

		[GetNode("Other/Events/move_cell_4")]
		private EventTrigger nMoveDoor4;

		[GetNode("Other/Events/move_cell_5")]
		private EventTrigger nMoveDoor5;

		[GetNode("Other/Events/move_cell_6")]
		private EventTrigger nMoveDoor6;

		[GetNode("Other/Events/misc_cell_2_interior")]
		private EventTrigger nEscapeCellInteriorEvent;

		[GetNode("Other/Events/move_exit_cell_2")]
		private EventTrigger nEscapeCellTouchEvent;

		[GetNode("Main/misc_dungeon_desk/ch1_lacie_items_shiny")]
		private Node2D nLacieItemsShiny;

		[GetNode("Background/CutsceneLacieRise")]
		private SimpleAnimatedSpriteV2 nCutsceneLacieRise;

		[GetNode("Main/PushBox")]
		private Node2D nPushBox;

		[GetNode("Background/ClimbBox")]
		private Node2D nClimbBox;

		[GetNode("Other/Events/move_climb")]
		private EventTrigger nClimbBoxEvt;

		[GetNode("Lighting/Cell2")]
		private Node2D nLightsCell2Open;

		[GetNode("Lighting/Cell2Closed")]
		private Node2D nLightsCell2Closed;

		[GetNode("Background/VentCover")]
		private Node2D nVentCover;

		private PVar varDoor1Open = "ch1.facility_dungeon_1_door_1_open";

		private PVar varDoor2Open = "ch1.facility_dungeon_1_door_2_open";

		private PVar varDoor3Open = "ch1.facility_dungeon_1_door_3_open";

		private PVar varDoor4Open = "ch1.facility_dungeon_1_door_4_open";

		private PVar varDoor5Open = "ch1.facility_dungeon_1_door_5_open";

		private PVar varDoor6Open = "ch1.facility_dungeon_1_door_6_open";

		private PVar varTookBackItems = "ch1.facility_took_back_items";

		private PVar varBoxLocation = "ch1.facility_dungeon_box_pos";

		private PVar varPlacedBox = "ch1.facility_dungeon_placed_box";

		private PVar varTookVentCover = "ch1.facility_dungeon_1_took_vent_cover";

		private PVar varItemStorage = "ch1.temp_blackpass_item_storage";

		private PVar varTalkedToSai = "ch1.facility_seen_cutscene_todolist";

		private PVar varSeenVentMan = "ch1.facility_seen_ventman";

		public override void _BeforeFadeIn()
		{
			if ((bool)varPlacedBox.Value)
			{
				nPushBox.DeleteIfValid();
			}
			if ((bool)varTalkedToSai.Value)
			{
				varDoor5Open.NewValue = false;
			}
			else if ((bool)varSeenVentMan.Value)
			{
				varDoor1Open.NewValue = false;
			}
		}

		public override void _UpdateRoom()
		{
			SetDoorState(nDoor1, nInteractDoor1, nInteractDoor1Close, nMoveDoor1, varDoor1Open.Value);
			SetDoorState(nDoor2, nInteractDoor2, nInteractDoor2Close, nMoveDoor2, varDoor2Open.Value);
			SetDoorState(nDoor3, nInteractDoor3, nInteractDoor3Close, nMoveDoor3, varDoor3Open.Value);
			SetDoorState(nDoor4, nInteractDoor4, nInteractDoor4Close, nMoveDoor4, varDoor4Open.Value);
			SetDoorState(nDoor5, nInteractDoor5, nInteractDoor5Close, nMoveDoor5, varDoor5Open.Value);
			SetDoorState(nDoor6, nInteractDoor6, nInteractDoor6Close, nMoveDoor6, varDoor6Open.Value);
			base._UpdateRoom();
			nLacieItemsShiny.Visible = !varTookBackItems.Value;
			nEscapeCellInteriorEvent.Enabled = !varDoor2Open.Value;
			nEscapeCellTouchEvent.Enabled = varDoor2Open.Value;
			Node2D node2D = nClimbBox;
			bool visible = (nClimbBoxEvt.Enabled = varPlacedBox.Value);
			node2D.Visible = visible;
			nLightsCell2Open.Visible = varDoor2Open.Value;
			nLightsCell2Closed.Visible = !nLightsCell2Open.Visible;
			nVentCover.Visible = !varTookVentCover.Value;
		}

		private void SetDoorState(Sprite door, EventTrigger interactEvent, EventTrigger closeEvent, EventTrigger moveEvent, bool isOpen)
		{
			door.Frame = (isOpen ? 3 : 0);
			interactEvent.Enabled = !isOpen;
			closeEvent.Enabled = isOpen;
			moveEvent.Enabled = isOpen;
		}

		public void OpenDoor1()
		{
			AnimateDoorOpening(nDoor1);
		}

		public void OpenDoor2()
		{
			AnimateDoorOpening(nDoor2);
		}

		public void OpenDoor3()
		{
			AnimateDoorOpening(nDoor3);
		}

		public void OpenDoor4()
		{
			AnimateDoorOpening(nDoor4);
		}

		public void OpenDoor5()
		{
			AnimateDoorOpening(nDoor5);
		}

		public void OpenDoor6()
		{
			AnimateDoorOpening(nDoor6);
		}

		public void CloseDoor1()
		{
			AnimateDoorClosing(nDoor1);
		}

		public void CloseDoor2()
		{
			AnimateDoorClosing(nDoor2);
		}

		public void CloseDoor3()
		{
			AnimateDoorClosing(nDoor3);
		}

		public void CloseDoor4()
		{
			AnimateDoorClosing(nDoor4);
		}

		public void CloseDoor5()
		{
			AnimateDoorClosing(nDoor5);
		}

		public void CloseDoor6()
		{
			AnimateDoorClosing(nDoor6);
		}

		public void MoveBoxIntoDoor1()
		{
			MoveBoxIntoDoor(1);
		}

		public void MoveBoxIntoDoor2()
		{
			MoveBoxIntoDoor(2);
		}

		public void MoveBoxIntoDoor3()
		{
			MoveBoxIntoDoor(3);
		}

		public void MoveBoxIntoDoor4()
		{
			MoveBoxIntoDoor(4);
		}

		public void MoveBoxIntoDoor5()
		{
			MoveBoxIntoDoor(5);
		}

		public void MoveBoxIntoDoor6()
		{
			MoveBoxIntoDoor(6);
		}

		public void MoveBoxOutOfDoor1()
		{
			MoveBoxOutOfDoor(1);
		}

		public void MoveBoxOutOfDoor2()
		{
			MoveBoxOutOfDoor(2);
		}

		public void MoveBoxOutOfDoor3()
		{
			MoveBoxOutOfDoor(3);
		}

		public void MoveBoxOutOfDoor4()
		{
			MoveBoxOutOfDoor(4);
		}

		public void MoveBoxOutOfDoor5()
		{
			MoveBoxOutOfDoor(5);
		}

		public void MoveBoxOutOfDoor6()
		{
			MoveBoxOutOfDoor(6);
		}

		public void LacieRise()
		{
			nCutsceneLacieRise.Visible = true;
			nCutsceneLacieRise.Playing = true;
		}

		public void LacieRiseHide()
		{
			nCutsceneLacieRise.Visible = false;
		}

		private void AnimateDoorOpening(SimpleAnimatedSprite door)
		{
			door.Playing = false;
			door.AnimationFrames = "";
			door.Playing = true;
		}

		private void AnimateDoorClosing(SimpleAnimatedSprite door)
		{
			door.Playing = false;
			door.AnimationFrames = "3,2,1,0";
			door.Playing = true;
		}

		private void MoveBoxIntoDoor(int door)
		{
			varBoxLocation.NewValue = GetPoint("cell_box_" + door);
		}

		private void MoveBoxOutOfDoor(int door)
		{
			varBoxLocation.NewValue = GetPoint("box_from_" + door);
		}

		public void RestoreInventory()
		{
			PaperLilyFunctions.RestoreInventory(varItemStorage);
		}
	}
}
