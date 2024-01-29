using System.Collections.Generic;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Minigames;
using LacieEngine.UI;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1PncWardrobe : PointAndClick
	{
		[Export(PropertyHint.None, "")]
		public Texture texCostumeDefault;

		[Export(PropertyHint.None, "")]
		public Texture texCostumeIndoors;

		[Export(PropertyHint.None, "")]
		public AnimatedTexture texCostumeDefaultShine;

		[Export(PropertyHint.None, "")]
		public AnimatedTexture texCostumeIndoorsShine;

		[Export(PropertyHint.None, "")]
		public Material matDiamondTransition;

		[GetNode("Targets/snowglobe/anim/Animation")]
		private AnimationPlayer nSnowGlobeAnimation;

		[GetNode("Costume_Overlay")]
		private TextureRect nCostumeOverlay;

		[GetNode("Targets/window_day")]
		private Control nWindowDay;

		[GetNode("Targets/window_evening")]
		private Control nWindowEvening;

		[GetNode("Targets/window_night")]
		private Control nWindowNight;

		[GetNode("Targets/rune")]
		private Control nRune;

		[GetNode("Targets/salt")]
		private Control nSalt;

		[GetNode("Targets/notebook")]
		private Control nNotebook;

		[GetNode("Targets/doorknob")]
		private Control nDoorknob;

		private SimpleChoicesMenuContainer nCostumeMenu;

		private PVar varTookCh1RitualItems = "ch1.home_took_wardrobe_items";

		private PEvent evtNoChange = "event_nochange";

		private PEvent evtChangeBounce = "ch1_event_change_indoors";

		public override void _Ready()
		{
			base._Ready();
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				nWindowDay.Visible = true;
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				nWindowEvening.Visible = true;
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "night")
			{
				nWindowNight.Visible = true;
			}
			nRune.Visible = !varTookCh1RitualItems.Value;
			nSalt.Visible = !varTookCh1RitualItems.Value;
			nNotebook.Visible = !varTookCh1RitualItems.Value;
			nDoorknob.Visible = !varTookCh1RitualItems.Value;
		}

		public void CurtainsOpen()
		{
		}

		public void CurtainsClose()
		{
		}

		public void ShakeSnowglobe()
		{
			nSnowGlobeAnimation.PlayFirstAnimation();
		}

		public void ChangeCostumeMenu()
		{
			List<IMenuEntry> options = new List<IMenuEntry>();
			options.Add(new SimpleMenuEntry(Game.Language.GetCaption("system.menu.costume.default"), ChangeToDefaultCostume, null));
			options.Add(new SimpleMenuEntry(Game.Language.GetCaption("system.menu.costume.indoors"), ChangeToIndoorsCostume, null));
			options.Add(new SimpleMenuEntry(Game.Language.GetCaption("system.menu.costume.cancel"), CloseCostumeMenu, null));
			nCostumeMenu = new SimpleChoicesMenuContainer(options);
			nCostumeMenu.OnClose = CloseCostumeMenu;
			nCostumeMenu.DarkenBackground = true;
			Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, nCostumeMenu);
			Game.InputProcessor = Inputs.Processor.Menu;
		}

		public void CloseCostumeMenu()
		{
			nCostumeMenu.DeleteIfValid();
			Game.InputProcessor = Inputs.Processor.Minigame;
		}

		public void ChangeToDefaultCostume()
		{
			nCostumeMenu.DeleteIfValid();
			ChangeCostume(texCostumeDefault, texCostumeDefaultShine, "pnc_event_respawn");
		}

		public void ChangeToIndoorsCostume()
		{
			nCostumeMenu.DeleteIfValid();
			ChangeCostume(texCostumeIndoors, texCostumeIndoorsShine, evtChangeBounce.Id);
		}

		private async void ChangeCostume(Texture textureCard, Texture textureShine, string evtEnd)
		{
			nCursor.Visible = false;
			nCostumeOverlay.Visible = true;
			nCostumeOverlay.Texture = textureCard;
			nCostumeOverlay.Material = matDiamondTransition.Duplicate() as Material;
			Game.Animations.Play(new ShaderProgressAnimation(nCostumeOverlay, 1.2f));
			await DrkieUtil.DelaySeconds(1.3);
			TextureRect shine = GDUtil.MakeNode<TextureRect>("Shine");
			shine.Texture = textureShine;
			shine.SetAnchorsGrowFrom(0.5f, 0.3f);
			AddChild(shine);
			await DrkieUtil.DelaySeconds(3.0);
			await Game.Screen.FadeToBlack();
			Game.Minigames.End(evtEnd);
		}
	}
}
