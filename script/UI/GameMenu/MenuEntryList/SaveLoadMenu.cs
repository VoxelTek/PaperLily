using System.Collections.Generic;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SaveLoadMenu : SimpleVerticalMenu
	{
		public enum Mode
		{
			Save,
			Load
		}

		private const int SLOTS_PER_PAGE = 3;

		private const int SLOTS_MAX = 30;

		private const string SLOT_PREFIX = "slot";

		private const float PAGINATION_ANIM_OFFSET = 60f;

		private const int LAST_PAGE = 9;

		private Mode _mode;

		private int _page;

		private Control _entriesCotainer;

		private Control _floatingContainer;

		private Control[] _drawnEntries;

		private Vector2[] _rowGlobalPositions;

		private bool _paginating;

		public SaveLoadMenu(Mode mode, IMenuEntryList parentMenu, IMenuEntryContainer container)
		{
			base.Parent = parentMenu;
			base.Container = container;
			_mode = mode;
			InitEntries();
		}

		public override Control DrawContent()
		{
			VBoxContainer container = GDUtil.MakeNode<VBoxContainer>("EntryList");
			container.SetSeparation(10);
			container.Alignment = BoxContainer.AlignMode.Center;
			container.SizeFlagsHorizontal = 4;
			_entriesCotainer = container;
			DrawEntries();
			_floatingContainer = GDUtil.MakeNode<Control>("FloatingContainer");
			_floatingContainer.SizeFlagsHorizontal = 3;
			_floatingContainer.SizeFlagsVertical = 3;
			TextureRect arrowL = GDUtil.MakeNode<TextureRect>("ArrowL");
			arrowL.Texture = GD.Load<Texture>("res://assets/img/ui/arrow_up.png");
			arrowL.RectRotation = -90f;
			arrowL.RectPosition = new Vector2(10f, 60f);
			arrowL.RectPivotOffset = arrowL.Texture.GetSize() / 2f;
			TextureRect arrowR = GDUtil.MakeNode<TextureRect>("ArrowR");
			arrowR.Texture = GD.Load<Texture>("res://assets/img/ui/arrow_up.png");
			arrowR.RectRotation = 90f;
			arrowR.RectPosition = new Vector2(412f, 60f);
			arrowR.RectPivotOffset = arrowL.Texture.GetSize() / 2f;
			Control floatingArrows = GDUtil.MakeNode<Control>("FloatingArrows");
			floatingArrows.AddChild(arrowL);
			floatingArrows.AddChild(arrowR);
			base.Container.AddToFrame(_floatingContainer);
			base.Container.AddToFrame(floatingArrows);
			Game.Animations.PlayDelayed(new BounceAnimation(arrowL, Direction.Left), 0.1f);
			Game.Animations.PlayDelayed(new BounceAnimation(arrowR, Direction.Right), 0.1f);
			return container;
		}

		public override void HighlightSelection()
		{
			foreach (SaveLoadSlotMenuEntry entry in base.Entries)
			{
				entry.Deselect();
			}
			if (base.Selection > -1)
			{
				((SaveLoadSlotMenuEntry)base.Entries[base.Selection]).Select();
			}
		}

		public override void HandleInput(InputEvent @event)
		{
			string input = Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, allowEcho: true);
			if (input == null || _paginating)
			{
				return;
			}
			switch (input)
			{
			case "input_up":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (--base.Selection <= -1)
				{
					base.Selection = base.Entries.Count - 1;
				}
				HighlightSelection();
				break;
			case "input_down":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (++base.Selection >= base.Entries.Count)
				{
					base.Selection = 0;
				}
				HighlightSelection();
				break;
			case "input_left":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				Paginate(right: false);
				break;
			case "input_right":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				Paginate(right: true);
				break;
			case "input_action":
				base.Entries[base.Selection].Accept();
				break;
			case "input_cancel":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
				Back();
				break;
			}
		}

		private void InitEntries()
		{
			base.Entries = new List<IMenuEntry>();
			base.Entries.Add(MakeEntrySlot(_mode, SaveFileInformation.GetSaveFileInformation("slot" + (_page * 3 + 1)), this));
			base.Entries.Add(MakeEntrySlot(_mode, SaveFileInformation.GetSaveFileInformation("slot" + (_page * 3 + 2)), this));
			base.Entries.Add(MakeEntrySlot(_mode, SaveFileInformation.GetSaveFileInformation("slot" + (_page * 3 + 3)), this));
		}

		private void RefreshPage()
		{
			InitEntries();
			DrawEntries();
			HighlightSelection();
		}

		private void DrawEntries()
		{
			_entriesCotainer.Clear();
			_drawnEntries = new Control[base.Entries.Count];
			int i = 0;
			foreach (IMenuEntry entry in base.Entries)
			{
				MarginContainer entryContainer = GDUtil.MakeNode<MarginContainer>("Option" + i);
				Control row = entry.DrawEntry();
				_drawnEntries[i] = row;
				entryContainer.AddChild(row);
				_entriesCotainer.AddChild(entryContainer);
				i++;
			}
		}

		private async void Paginate(bool right)
		{
			_paginating = true;
			_floatingContainer.Clear();
			if (right && ++_page > 9)
			{
				_page = 0;
			}
			if (!right && --_page < 0)
			{
				_page = 9;
			}
			if (_rowGlobalPositions == null)
			{
				_rowGlobalPositions = new Vector2[3];
				for (int i = 0; i < 3; i++)
				{
					_rowGlobalPositions[i] = _drawnEntries[i].RectGlobalPosition;
				}
			}
			for (int j = 0; j < 3; j++)
			{
				_drawnEntries[j].Reparent(_floatingContainer);
				_drawnEntries[j].RectGlobalPosition = _rowGlobalPositions[j];
			}
			int newSlotNumber = _page * 3 + 1;
			for (int k = 0; k < 3; k++)
			{
				if (right)
				{
					Game.Animations.Play(new SlideOutLeftAnimation(_drawnEntries[k], 60f));
				}
				else
				{
					Game.Animations.Play(new SlideOutRightAnimation(_drawnEntries[k], 60f));
				}
				Control newSlot = MakeEntrySlot(_mode, SaveFileInformation.GetSaveFileInformation("slot" + newSlotNumber++), this).DrawEntry();
				_floatingContainer.AddChild(newSlot);
				newSlot.RectGlobalPosition = _rowGlobalPositions[k];
				newSlot.Modulate = Colors.Transparent;
				await DrkieUtil.DelaySeconds(0.05);
				if (right)
				{
					Game.Animations.Play(new SlideInRightAnimation(newSlot, 60f));
				}
				else
				{
					Game.Animations.Play(new SlideInLeftAnimation(newSlot, 60f));
				}
			}
			await DrkieUtil.DelaySeconds(0.05);
			RefreshPage();
			_paginating = false;
		}

		private SaveLoadSlotMenuEntry MakeEntrySlot(Mode mode, SaveFileInformation saveInfo, IMenuEntryList parentMenu)
		{
			if (GameState.SaveExists(saveInfo.Id))
			{
				return new SaveLoadSlotUsedEntry(mode, saveInfo, parentMenu);
			}
			return new SaveLoadSlotEmptyEntry(mode, saveInfo, parentMenu);
		}
	}
}
