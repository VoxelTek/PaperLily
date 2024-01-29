using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class InputBindingScreen : Control
	{
		private string _action;

		private Action _refreshCallback;

		private InputBindingScreen()
		{
		}

		public InputBindingScreen(string action, string actionCaption, Action refreshCallback)
		{
			base.Name = "InputBindingScreen";
			_action = action;
			_refreshCallback = refreshCallback;
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			string caption = ((Inputs.Profiles[Game.Settings.InputProfile].Type != 0) ? "system.settings.input.pressbutton" : "system.settings.input.presskey");
			caption = Game.Language.GetFormattedCaption(caption, Game.Language.GetCaption(actionCaption));
			ColorRect darkener = new ColorRect();
			darkener.Color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			darkener.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control infoBox = UIUtil.CreateInfoBox(caption);
			CenterContainer centerContainer = new CenterContainer();
			centerContainer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			centerContainer.AddChild(infoBox);
			AddChild(darkener);
			AddChild(centerContainer);
		}

		public override void _Input(InputEvent @event)
		{
			if (Game.InputProcessor != Inputs.Processor.Binding || !@event.IsPressed())
			{
				return;
			}
			Game.Tree.SetInputAsHandled();
			InputProfile selectedProfile = Inputs.Profiles[Game.Settings.InputProfile];
			if (selectedProfile.Type == InputProfile.InputType.Keyboard)
			{
				if (!(@event is InputEventKey key2))
				{
					return;
				}
				uint[] blacklist = InputMapper.Blacklist;
				for (int i = 0; i < blacklist.Length; i++)
				{
					KeyList blacklistedKey = (KeyList)blacklist[i];
					if (key2.Scancode == (uint)blacklistedKey)
					{
						return;
					}
				}
				Inputs.Profiles["custom"].UnassignMapping(_action);
				Inputs.Profiles["custom"].UnassignEvent(key2);
				Inputs.Profiles["custom"].AddKeyMapping(_action, key2.Scancode);
			}
			else if (selectedProfile.Type == InputProfile.InputType.Controller)
			{
				if (@event is InputEventJoypadButton button)
				{
					Inputs.Profiles["custom"].UnassignMapping(_action);
					Inputs.Profiles["custom"].UnassignEvent(button);
					Inputs.Profiles["custom"].AddButtonMapping(_action, button.ButtonIndex);
				}
				else if (@event is InputEventJoypadMotion axis)
				{
					Inputs.Profiles["custom"].UnassignMapping(_action);
					Inputs.Profiles["custom"].UnassignEvent(axis);
					Inputs.Profiles["custom"].AddAxisMapping(_action, axis.Axis, (axis.AxisValue > 0f) ? 1 : (-1));
				}
				else
				{
					if (!(@event is InputEventKey key) || key.Scancode != 16777217)
					{
						return;
					}
					ReturnToMenu();
				}
			}
			ReturnToMenu();
		}

		private void ReturnToMenu()
		{
			Game.InputProcessor = Inputs.Processor.Menu;
			this.Delete();
			_refreshCallback();
		}
	}
}
