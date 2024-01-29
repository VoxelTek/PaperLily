using System;
using System.Collections.Generic;
using Godot;

namespace LacieEngine.Core
{
	public class Inputs
	{
		public enum Processor
		{
			None,
			Player,
			PlayerInObject,
			StoryPlayer,
			Menu,
			Minigame,
			Tutorial,
			Binding,
			Any,
			Manual
		}

		public const string Up = "input_up";

		public const string Down = "input_down";

		public const string Left = "input_left";

		public const string Right = "input_right";

		public const string Action = "input_action";

		public const string Cancel = "input_cancel";

		public const string Run = "input_run";

		public const string Special = "input_special";

		public const string Menu = "input_menu";

		public const string MuteAudio = "system_mute_audio";

		public const string FullScreen = "system_full_screen";

		public const string Screenshot = "system_screenshot";

		public const string DebugKey = "debug_key";

		public const string Logger = "debug_logger";

		public const string Fps = "debug_fps";

		public const string OpenEvent = "debug_openevent";

		public const string Console = "debug_console";

		public const string Save = "debug_save";

		public const string Load = "debug_load";

		public static readonly string[] AllPlayer = new string[7] { "input_up", "input_down", "input_left", "input_right", "input_action", "input_run", "input_special" };

		public static readonly string[] AllMovement = new string[4] { "input_up", "input_down", "input_left", "input_right" };

		public static readonly string[] AllUi = new string[7] { "input_up", "input_down", "input_left", "input_right", "input_action", "input_cancel", "input_menu" };

		public static readonly string[] AllGame = new string[9] { "input_up", "input_down", "input_left", "input_right", "input_action", "input_cancel", "input_run", "input_special", "input_menu" };

		public static readonly string[] AllSystem = new string[3] { "system_full_screen", "system_screenshot", "system_mute_audio" };

		public static readonly string[] AllKeys = new string[19]
		{
			"input_up", "input_down", "input_left", "input_right", "input_action", "input_cancel", "input_run", "input_special", "input_menu", "system_full_screen",
			"system_screenshot", "system_mute_audio", "debug_key", "debug_logger", "debug_fps", "debug_openevent", "debug_console", "debug_save", "debug_load"
		};

		public static Dictionary<string, InputProfile> Profiles;

		public static InputProfile SystemKeys;

		public static InputProfile DebugKeys;

		public static void Init()
		{
			foreach (string action in InputMap.GetActions())
			{
				if (!action.StartsWith("ui_"))
				{
					InputMap.EraseAction(action);
				}
			}
			string[] allKeys = AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				InputMap.AddAction(allKeys[i]);
			}
			Profiles = new Dictionary<string, InputProfile>();
			foreach (string filename in GDUtil.ListFilesInPath("res://definitions/inputprofile/", null, ".json", fullPath: false))
			{
				string name = filename.Substring(0, filename.LastIndexOf(".json"));
				InputProfileDTO dto2 = GDUtil.ReadJsonFile<InputProfileDTO>("res://definitions/inputprofile/" + filename);
				Profiles[name] = dto2.ToInputProfile();
				Profiles[name].Name = name;
			}
			if (GDUtil.FileExists("user://bindings.json"))
			{
				try
				{
					InputProfileDTO dto = GDUtil.ReadJsonFile<InputProfileDTO>("user://bindings.json");
					Profiles["custom"] = dto.ToInputProfile();
					Profiles["custom"].Name = "custom";
				}
				catch
				{
					Log.Error("Failed to load custom input profile. Is the bindings file corrupted?");
				}
			}
			SystemKeys = new InputProfile(InputProfile.InputType.Keyboard);
			SystemKeys.AddKeyMapping("system_mute_audio", 16777253u);
			SystemKeys.AddKeyMapping("system_full_screen", 16777254u);
			SystemKeys.AddKeyMapping("system_screenshot", 16777255u);
			DebugKeys = new InputProfile(InputProfile.InputType.Keyboard);
			DebugKeys.AddKeyMapping("debug_logger", 16777244u);
			DebugKeys.AddKeyMapping("debug_fps", 16777245u);
			DebugKeys.AddKeyMapping("debug_key", 16777244u, shift: true);
			DebugKeys.AddKeyMapping("debug_openevent", 16777247u, shift: true);
			DebugKeys.AddKeyMapping("debug_console", 96u);
			DebugKeys.AddKeyMapping("debug_save", 16777248u, shift: true);
			DebugKeys.AddKeyMapping("debug_load", 16777249u, shift: true);
		}

		public static string Handle(InputEvent @event, Processor processor, string[] validInputs, bool allowEcho = false)
		{
			if (Game.InputProcessor == Processor.None)
			{
				Game.Tree.SetInputAsHandled();
				return null;
			}
			if (processor != Processor.Any && processor != Game.InputProcessor)
			{
				return null;
			}
			foreach (string validInput in validInputs)
			{
				if (@event.IsActionPressed(validInput, allowEcho) && (!(@event is InputEventJoypadMotion) || IsActionJustPressed(validInput)))
				{
					Game.Tree.SetInputAsHandled();
					return validInput;
				}
			}
			return null;
		}

		public static string Handle(Processor processor, string[] validInputs)
		{
			if (Game.InputProcessor == Processor.None)
			{
				Game.Tree.SetInputAsHandled();
				return null;
			}
			if (processor != Processor.Any && processor != Game.InputProcessor)
			{
				return null;
			}
			foreach (string validInput in validInputs)
			{
				if (IsActionJustPressed(validInput))
				{
					Game.Tree.SetInputAsHandled();
					return validInput;
				}
			}
			return null;
		}

		public static bool HandleSingle(InputEvent @event, Processor processor, string validInput, bool allowEcho = false)
		{
			if (Game.InputProcessor == Processor.None)
			{
				Game.Tree.SetInputAsHandled();
				return false;
			}
			if (processor != Processor.Any && processor != Game.InputProcessor)
			{
				return false;
			}
			if (@event.IsActionPressed(validInput, allowEcho))
			{
				Game.Tree.SetInputAsHandled();
				return true;
			}
			return false;
		}

		public static bool IsActionJustPressed(string action)
		{
			if (!Input.IsActionJustPressed(action))
			{
				return false;
			}
			return IsActionPressed(action);
		}

		public static bool IsActionPressed(string action)
		{
			foreach (InputEvent @event in InputMap.GetActionList(action))
			{
				if (@event is InputEventKey key && Input.IsKeyPressed((int)key.Scancode))
				{
					return true;
				}
				if (@event is InputEventJoypadButton button && Input.IsJoyButtonPressed(button.Device, button.ButtonIndex))
				{
					return true;
				}
				if (@event is InputEventJoypadMotion axis && Math.Sign(axis.AxisValue) == Math.Sign(Input.GetJoyAxis(axis.Device, axis.Axis)) && Math.Abs(Input.GetJoyAxis(axis.Device, axis.Axis)) > Game.Settings.JoystickDeadzone)
				{
					return true;
				}
			}
			return false;
		}

		public static Direction GetDirectionFromInput(Direction currentDirection, Processor processor)
		{
			if (Game.InputProcessor == Processor.None || (processor != Processor.Any && processor != Game.InputProcessor))
			{
				return Direction.None;
			}
			string justPressed = Handle(processor, AllMovement);
			Direction xMov = ((justPressed == "input_left") ? Direction.Left : ((justPressed == "input_right") ? Direction.Right : ((IsActionPressed("input_left") && currentDirection.FlattenX() == Direction.Left) ? Direction.Left : ((IsActionPressed("input_right") && currentDirection.FlattenX() == Direction.Right) ? Direction.Right : (IsActionPressed("input_left") ? Direction.Left : ((!IsActionPressed("input_right")) ? Direction.None : Direction.Right))))));
			Direction yMov = ((justPressed == "input_up") ? Direction.Up : ((justPressed == "input_down") ? Direction.Down : ((IsActionPressed("input_up") && currentDirection.FlattenY() == Direction.Up) ? Direction.Up : ((IsActionPressed("input_down") && currentDirection.FlattenY() == Direction.Down) ? Direction.Down : (IsActionPressed("input_up") ? Direction.Up : ((!IsActionPressed("input_down")) ? Direction.None : Direction.Down))))));
			return Direction.FromVectorAnalog(xMov.ToVector() + yMov.ToVector());
		}

		public static string Caption(string action)
		{
			return InputMapper.CurrentProfile.GetCaptionForAction(action);
		}
	}
}
