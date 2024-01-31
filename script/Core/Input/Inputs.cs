// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Inputs
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  public class Inputs
  {
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
    public static readonly string[] AllPlayer = new string[7]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right",
      "input_action",
      "input_run",
      "input_special"
    };
    public static readonly string[] AllMovement = new string[4]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right"
    };
    public static readonly string[] AllUi = new string[7]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right",
      "input_action",
      "input_cancel",
      "input_menu"
    };
    public static readonly string[] AllGame = new string[9]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right",
      "input_action",
      "input_cancel",
      "input_run",
      "input_special",
      "input_menu"
    };
    public static readonly string[] AllSystem = new string[3]
    {
      "system_full_screen",
      "system_screenshot",
      "system_mute_audio"
    };
    public static readonly string[] AllKeys = new string[19]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right",
      "input_action",
      "input_cancel",
      "input_run",
      "input_special",
      "input_menu",
      "system_full_screen",
      "system_screenshot",
      "system_mute_audio",
      "debug_key",
      "debug_logger",
      "debug_fps",
      "debug_openevent",
      "debug_console",
      "debug_save",
      "debug_load"
    };
    public static Dictionary<string, InputProfile> Profiles;
    public static InputProfile SystemKeys;
    public static InputProfile DebugKeys;

    public static void Init()
    {
      foreach (string action in InputMap.GetActions())
      {
        if (!action.StartsWith("ui_"))
          InputMap.EraseAction(action);
      }
      foreach (string allKey in Inputs.AllKeys)
        InputMap.AddAction(allKey);
      Inputs.Profiles = new Dictionary<string, InputProfile>();
      foreach (string str in GDUtil.ListFilesInPath("res://definitions/inputprofile/", suffix: ".json", fullPath: false))
      {
        string key = str.Substring(0, str.LastIndexOf(".json"));
        InputProfileDTO inputProfileDto = GDUtil.ReadJsonFile<InputProfileDTO>("res://definitions/inputprofile/" + str);
        Inputs.Profiles[key] = inputProfileDto.ToInputProfile();
        Inputs.Profiles[key].Name = key;
      }
      if (GDUtil.FileExists("user://bindings.json"))
      {
        try
        {
          InputProfileDTO inputProfileDto = GDUtil.ReadJsonFile<InputProfileDTO>("user://bindings.json");
          Inputs.Profiles["custom"] = inputProfileDto.ToInputProfile();
          Inputs.Profiles["custom"].Name = "custom";
        }
        catch
        {
          Log.Error((object) "Failed to load custom input profile. Is the bindings file corrupted?");
        }
      }
      Inputs.SystemKeys = new InputProfile(InputProfile.InputType.Keyboard);
      Inputs.SystemKeys.AddKeyMapping("system_mute_audio", 16777253U);
      Inputs.SystemKeys.AddKeyMapping("system_full_screen", 16777254U);
      Inputs.SystemKeys.AddKeyMapping("system_screenshot", 16777255U);
      Inputs.DebugKeys = new InputProfile(InputProfile.InputType.Keyboard);
      Inputs.DebugKeys.AddKeyMapping("debug_logger", 16777244U);
      Inputs.DebugKeys.AddKeyMapping("debug_fps", 16777245U);
      Inputs.DebugKeys.AddKeyMapping("debug_key", 16777244U, true);
      Inputs.DebugKeys.AddKeyMapping("debug_openevent", 16777247U, true);
      Inputs.DebugKeys.AddKeyMapping("debug_console", 96U);
      Inputs.DebugKeys.AddKeyMapping("debug_save", 16777248U, true);
      Inputs.DebugKeys.AddKeyMapping("debug_load", 16777249U, true);
    }

    public static string Handle(
      InputEvent @event,
      Inputs.Processor processor,
      string[] validInputs,
      bool allowEcho = false)
    {
      if (Game.InputProcessor == Inputs.Processor.None)
      {
        Game.Tree.SetInputAsHandled();
        return (string) null;
      }
      if (processor != Inputs.Processor.Any && processor != Game.InputProcessor)
        return (string) null;
      foreach (string validInput in validInputs)
      {
        if (@event.IsActionPressed(validInput, allowEcho) && (!(@event is InputEventJoypadMotion) || Inputs.IsActionJustPressed(validInput)))
        {
          Game.Tree.SetInputAsHandled();
          return validInput;
        }
      }
      return (string) null;
    }

    public static string Handle(Inputs.Processor processor, string[] validInputs)
    {
      if (Game.InputProcessor == Inputs.Processor.None)
      {
        Game.Tree.SetInputAsHandled();
        return (string) null;
      }
      if (processor != Inputs.Processor.Any && processor != Game.InputProcessor)
        return (string) null;
      foreach (string validInput in validInputs)
      {
        if (Inputs.IsActionJustPressed(validInput))
        {
          Game.Tree.SetInputAsHandled();
          return validInput;
        }
      }
      return (string) null;
    }

    public static bool HandleSingle(
      InputEvent @event,
      Inputs.Processor processor,
      string validInput,
      bool allowEcho = false)
    {
      if (Game.InputProcessor == Inputs.Processor.None)
      {
        Game.Tree.SetInputAsHandled();
        return false;
      }
      if (processor != Inputs.Processor.Any && processor != Game.InputProcessor || !@event.IsActionPressed(validInput, allowEcho))
        return false;
      Game.Tree.SetInputAsHandled();
      return true;
    }

    public static bool IsActionJustPressed(string action)
    {
      return Input.IsActionJustPressed(action) && Inputs.IsActionPressed(action);
    }

    public static bool IsActionPressed(string action)
    {
      foreach (InputEvent action1 in InputMap.GetActionList(action))
      {
        switch (action1)
        {
          case InputEventKey inputEventKey when Input.IsKeyPressed((int) inputEventKey.Scancode):
            return true;
          case InputEventJoypadButton eventJoypadButton when Input.IsJoyButtonPressed(eventJoypadButton.Device, eventJoypadButton.ButtonIndex):
            return true;
          case InputEventJoypadMotion eventJoypadMotion when Math.Sign(eventJoypadMotion.AxisValue) == Math.Sign(Input.GetJoyAxis(eventJoypadMotion.Device, eventJoypadMotion.Axis)) && (double) Math.Abs(Input.GetJoyAxis(eventJoypadMotion.Device, eventJoypadMotion.Axis)) > (double) Game.Settings.JoystickDeadzone:
            return true;
          default:
            continue;
        }
      }
      return false;
    }

    public static Direction GetDirectionFromInput(
      Direction currentDirection,
      Inputs.Processor processor)
    {
      if (Game.InputProcessor == Inputs.Processor.None || processor != Inputs.Processor.Any && processor != Game.InputProcessor)
        return Direction.None;
      string str = Inputs.Handle(processor, Inputs.AllMovement);
      Direction direction1;
      switch (str)
      {
        case "input_left":
          direction1 = Direction.Left;
          break;
        case "input_right":
          direction1 = Direction.Right;
          break;
        default:
          direction1 = !Inputs.IsActionPressed("input_left") || !(currentDirection.FlattenX() == Direction.Left) ? (!Inputs.IsActionPressed("input_right") || !(currentDirection.FlattenX() == Direction.Right) ? (!Inputs.IsActionPressed("input_left") ? (!Inputs.IsActionPressed("input_right") ? Direction.None : Direction.Right) : Direction.Left) : Direction.Right) : Direction.Left;
          break;
      }
      Direction direction2;
      switch (str)
      {
        case "input_up":
          direction2 = Direction.Up;
          break;
        case "input_down":
          direction2 = Direction.Down;
          break;
        default:
          direction2 = !Inputs.IsActionPressed("input_up") || !(currentDirection.FlattenY() == Direction.Up) ? (!Inputs.IsActionPressed("input_down") || !(currentDirection.FlattenY() == Direction.Down) ? (!Inputs.IsActionPressed("input_up") ? (!Inputs.IsActionPressed("input_down") ? Direction.None : Direction.Down) : Direction.Up) : Direction.Down) : Direction.Up;
          break;
      }
      return Direction.FromVectorAnalog(direction1.ToVector() + direction2.ToVector());
    }

    public static string Caption(string action)
    {
      return InputMapper.CurrentProfile.GetCaptionForAction(action);
    }

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
      Manual,
    }
  }
}
