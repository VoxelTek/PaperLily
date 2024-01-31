// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.InputProfile
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  public class InputProfile : IComparable<InputProfile>
  {
    public string Name;
    public string Caption;
    public int Order;
    public InputProfile.InputType Type;
    public List<InputProfile.Mapping> Mappings = new List<InputProfile.Mapping>();
    public string CaptionBase;
    public Dictionary<uint, string> KeyCaptions;
    public Dictionary<int, string> JoystickButtonCaptions;
    public Dictionary<int, string> JoystickAxisPlusCaptions;
    public Dictionary<int, string> JoystickAxisMinusCaptions;

    public InputProfile(InputProfile.InputType type)
    {
      this.Type = type;
      this.Mappings = new List<InputProfile.Mapping>();
      this.KeyCaptions = new Dictionary<uint, string>();
      this.JoystickButtonCaptions = new Dictionary<int, string>();
      this.JoystickAxisPlusCaptions = new Dictionary<int, string>();
      this.JoystickAxisMinusCaptions = new Dictionary<int, string>();
    }

    public InputProfile(InputProfile baseProfile)
    {
      this.Type = baseProfile.Type;
      this.Mappings = new List<InputProfile.Mapping>((IEnumerable<InputProfile.Mapping>) baseProfile.Mappings);
      this.CaptionBase = baseProfile.CaptionBase;
      this.KeyCaptions = new Dictionary<uint, string>((IDictionary<uint, string>) baseProfile.KeyCaptions);
      this.JoystickButtonCaptions = new Dictionary<int, string>((IDictionary<int, string>) baseProfile.JoystickButtonCaptions);
      this.JoystickAxisPlusCaptions = new Dictionary<int, string>((IDictionary<int, string>) baseProfile.JoystickAxisPlusCaptions);
      this.JoystickAxisMinusCaptions = new Dictionary<int, string>((IDictionary<int, string>) baseProfile.JoystickAxisMinusCaptions);
    }

    public void AddKeyMapping(string action, uint key, bool shift = false, bool ctrl = false, bool alt = false)
    {
      InputEventKey inputEventKey = new InputEventKey();
      inputEventKey.Scancode = key;
      inputEventKey.Shift = shift;
      inputEventKey.Control = ctrl;
      inputEventKey.Alt = alt;
      this.Mappings.Add(new InputProfile.Mapping()
      {
        Action = action,
        Event = (InputEvent) inputEventKey
      });
    }

    public void AddButtonMapping(string action, int button)
    {
      this.Mappings.Add(new InputProfile.Mapping()
      {
        Action = action,
        Event = (InputEvent) new InputEventJoypadButton()
        {
          ButtonIndex = button
        }
      });
    }

    public void AddAxisMapping(string action, int axis, int value)
    {
      this.Mappings.Add(new InputProfile.Mapping()
      {
        Action = action,
        Event = (InputEvent) new InputEventJoypadMotion()
        {
          Axis = axis,
          AxisValue = (value > 0 ? 1f : -1f)
        }
      });
    }

    public bool IsComplete()
    {
      foreach (string action in Inputs.AllGame)
      {
        if (!this.IsActionMapped(action))
          return false;
      }
      return true;
    }

    public bool IsActionMapped(string action)
    {
      foreach (InputProfile.Mapping mapping in this.Mappings)
      {
        if (mapping.Action == action)
          return true;
      }
      return false;
    }

    public void UnassignMapping(string action)
    {
      this.Mappings.RemoveAll((Predicate<InputProfile.Mapping>) (mapping => mapping.Action == action));
    }

    public void UnassignEvent(InputEvent @event)
    {
      List<InputProfile.Mapping> toRemove = new List<InputProfile.Mapping>();
      foreach (InputProfile.Mapping mapping in this.Mappings)
      {
        if (mapping.Event is InputEventKey inputEventKey1 && @event is InputEventKey inputEventKey2)
        {
          if ((int) inputEventKey1.Scancode == (int) inputEventKey2.Scancode)
            toRemove.Add(mapping);
        }
        else if (mapping.Event is InputEventJoypadButton eventJoypadButton1 && @event is InputEventJoypadButton eventJoypadButton2)
        {
          if (eventJoypadButton1.ButtonIndex == eventJoypadButton2.ButtonIndex)
            toRemove.Add(mapping);
        }
        else if (mapping.Event is InputEventJoypadMotion eventJoypadMotion1 && @event is InputEventJoypadMotion eventJoypadMotion2 && eventJoypadMotion1.Axis == eventJoypadMotion2.Axis && (double) eventJoypadMotion1.AxisValue == (double) eventJoypadMotion2.AxisValue)
          toRemove.Add(mapping);
      }
      this.Mappings.RemoveAll((Predicate<InputProfile.Mapping>) (mapping => toRemove.Contains(mapping)));
    }

    public void AddKeyCaption(uint key, string caption) => this.KeyCaptions[key] = caption;

    public void AddButtonCaption(int button, string caption)
    {
      this.JoystickButtonCaptions[button] = caption;
    }

    public void AddAxisCaption(int axis, int value, string caption)
    {
      if (value > 0)
        this.JoystickAxisPlusCaptions[axis] = caption;
      else
        this.JoystickAxisMinusCaptions[axis] = caption;
    }

    public string GetCaptionForAction(string action)
    {
      foreach (InputProfile.Mapping mapping in this.Mappings)
      {
        if (mapping.Action == action)
          return this.GetCaptionForEvent(mapping.Event);
      }
      return "-";
    }

    public string GetAllCaptionsForAction(string action)
    {
      List<string> values = new List<string>();
      foreach (InputProfile.Mapping mapping in this.Mappings)
      {
        if (mapping.Action == action)
          values.Add(this.GetCaptionForEvent(mapping.Event));
      }
      return values.Count > 0 ? string.Join("/", (IEnumerable<string>) values) : "-";
    }

    public string GetCaptionForEvent(InputEvent evt)
    {
      if (this.CaptionBase != null && Inputs.Profiles.ContainsKey(this.CaptionBase))
        return Inputs.Profiles[this.CaptionBase].GetCaptionForEvent(evt);
      switch (evt)
      {
        case InputEventKey inputEventKey:
          uint scancode = inputEventKey.Scancode;
          return this.KeyCaptions.ContainsKey(scancode) ? this.ProcessCaption(this.KeyCaptions[scancode]) : OS.GetScancodeString(scancode);
        case InputEventJoypadButton eventJoypadButton:
          int buttonIndex = eventJoypadButton.ButtonIndex;
          return this.JoystickButtonCaptions.ContainsKey(buttonIndex) ? this.ProcessCaption(this.JoystickButtonCaptions[buttonIndex]) : Game.Language.GetCaption("system.common.button") + " " + (buttonIndex + 1).ToString();
        case InputEventJoypadMotion eventJoypadMotion:
          int axis = eventJoypadMotion.Axis;
          float axisValue = eventJoypadMotion.AxisValue;
          if ((double) axisValue > 0.0 && this.JoystickAxisPlusCaptions.ContainsKey(axis))
            return this.ProcessCaption(this.JoystickAxisPlusCaptions[axis]);
          return this.JoystickAxisMinusCaptions.ContainsKey(axis) ? this.ProcessCaption(this.JoystickAxisMinusCaptions[axis]) : Game.Language.GetCaption("system.common.axis") + " " + (axis + 1).ToString() + ((double) axisValue > 0.0 ? " +" : " -");
        default:
          return "-";
      }
    }

    private string ProcessCaption(string caption)
    {
      if (!caption.StartsWith("img:"))
        return Game.Language.GetCaption(caption);
      string[] strArray = new string[5]
      {
        "[img]",
        "res://assets/img/ui/input/",
        null,
        null,
        null
      };
      string str = caption;
      strArray[2] = str.Substring(4, str.Length - 4);
      strArray[3] = ".png";
      strArray[4] = "[/img]";
      return string.Concat(strArray);
    }

    public int CompareTo(InputProfile profile) => this.Order.CompareTo(profile.Order);

    public enum InputType
    {
      Keyboard,
      Controller,
    }

    public class Mapping
    {
      public string Action;
      public InputEvent Event;
    }
  }
}
