using System;
using System.Collections.Generic;
using Godot;

namespace LacieEngine.Core
{
	public class InputProfile : IComparable<InputProfile>
	{
		public enum InputType
		{
			Keyboard,
			Controller
		}

		public class Mapping
		{
			public string Action;

			public InputEvent Event;
		}

		public string Name;

		public string Caption;

		public int Order;

		public InputType Type;

		public List<Mapping> Mappings = new List<Mapping>();

		public string CaptionBase;

		public Dictionary<uint, string> KeyCaptions;

		public Dictionary<int, string> JoystickButtonCaptions;

		public Dictionary<int, string> JoystickAxisPlusCaptions;

		public Dictionary<int, string> JoystickAxisMinusCaptions;

		public InputProfile(InputType type)
		{
			Type = type;
			Mappings = new List<Mapping>();
			KeyCaptions = new Dictionary<uint, string>();
			JoystickButtonCaptions = new Dictionary<int, string>();
			JoystickAxisPlusCaptions = new Dictionary<int, string>();
			JoystickAxisMinusCaptions = new Dictionary<int, string>();
		}

		public InputProfile(InputProfile baseProfile)
		{
			Type = baseProfile.Type;
			Mappings = new List<Mapping>(baseProfile.Mappings);
			CaptionBase = baseProfile.CaptionBase;
			KeyCaptions = new Dictionary<uint, string>(baseProfile.KeyCaptions);
			JoystickButtonCaptions = new Dictionary<int, string>(baseProfile.JoystickButtonCaptions);
			JoystickAxisPlusCaptions = new Dictionary<int, string>(baseProfile.JoystickAxisPlusCaptions);
			JoystickAxisMinusCaptions = new Dictionary<int, string>(baseProfile.JoystickAxisMinusCaptions);
		}

		public void AddKeyMapping(string action, uint key, bool shift = false, bool ctrl = false, bool alt = false)
		{
			InputEventKey evt = new InputEventKey();
			evt.Scancode = key;
			evt.Shift = shift;
			evt.Control = ctrl;
			evt.Alt = alt;
			Mapping mapping = new Mapping();
			mapping.Action = action;
			mapping.Event = evt;
			Mappings.Add(mapping);
		}

		public void AddButtonMapping(string action, int button)
		{
			InputEventJoypadButton evt = new InputEventJoypadButton();
			evt.ButtonIndex = button;
			Mapping mapping = new Mapping();
			mapping.Action = action;
			mapping.Event = evt;
			Mappings.Add(mapping);
		}

		public void AddAxisMapping(string action, int axis, int value)
		{
			InputEventJoypadMotion evt = new InputEventJoypadMotion();
			evt.Axis = axis;
			evt.AxisValue = ((value > 0) ? 1f : (-1f));
			Mapping mapping = new Mapping();
			mapping.Action = action;
			mapping.Event = evt;
			Mappings.Add(mapping);
		}

		public bool IsComplete()
		{
			string[] allGame = Inputs.AllGame;
			foreach (string action in allGame)
			{
				if (!IsActionMapped(action))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsActionMapped(string action)
		{
			foreach (Mapping mapping in Mappings)
			{
				if (mapping.Action == action)
				{
					return true;
				}
			}
			return false;
		}

		public void UnassignMapping(string action)
		{
			Mappings.RemoveAll((Mapping mapping) => mapping.Action == action);
		}

		public void UnassignEvent(InputEvent @event)
		{
			List<Mapping> toRemove = new List<Mapping>();
			foreach (Mapping mapping2 in Mappings)
			{
				if (mapping2.Event is InputEventKey mappingKey && @event is InputEventKey eventKey)
				{
					if (mappingKey.Scancode == eventKey.Scancode)
					{
						toRemove.Add(mapping2);
					}
				}
				else if (mapping2.Event is InputEventJoypadButton mappingButton && @event is InputEventJoypadButton eventButton)
				{
					if (mappingButton.ButtonIndex == eventButton.ButtonIndex)
					{
						toRemove.Add(mapping2);
					}
				}
				else if (mapping2.Event is InputEventJoypadMotion mappingMotion && @event is InputEventJoypadMotion eventMotion && mappingMotion.Axis == eventMotion.Axis && mappingMotion.AxisValue == eventMotion.AxisValue)
				{
					toRemove.Add(mapping2);
				}
			}
			Mappings.RemoveAll((Mapping mapping) => toRemove.Contains(mapping));
		}

		public void AddKeyCaption(uint key, string caption)
		{
			KeyCaptions[key] = caption;
		}

		public void AddButtonCaption(int button, string caption)
		{
			JoystickButtonCaptions[button] = caption;
		}

		public void AddAxisCaption(int axis, int value, string caption)
		{
			if (value > 0)
			{
				JoystickAxisPlusCaptions[axis] = caption;
			}
			else
			{
				JoystickAxisMinusCaptions[axis] = caption;
			}
		}

		public string GetCaptionForAction(string action)
		{
			foreach (Mapping mapping in Mappings)
			{
				if (mapping.Action == action)
				{
					return GetCaptionForEvent(mapping.Event);
				}
			}
			return "-";
		}

		public string GetAllCaptionsForAction(string action)
		{
			List<string> captions = new List<string>();
			foreach (Mapping mapping in Mappings)
			{
				if (mapping.Action == action)
				{
					captions.Add(GetCaptionForEvent(mapping.Event));
				}
			}
			if (captions.Count > 0)
			{
				return string.Join("/", captions);
			}
			return "-";
		}

		public string GetCaptionForEvent(InputEvent evt)
		{
			if (CaptionBase != null && Inputs.Profiles.ContainsKey(CaptionBase))
			{
				return Inputs.Profiles[CaptionBase].GetCaptionForEvent(evt);
			}
			if (evt is InputEventKey eventKey)
			{
				uint code2 = eventKey.Scancode;
				if (KeyCaptions.ContainsKey(code2))
				{
					return ProcessCaption(KeyCaptions[code2]);
				}
				return OS.GetScancodeString(code2);
			}
			if (evt is InputEventJoypadButton eventButton)
			{
				int code3 = eventButton.ButtonIndex;
				if (JoystickButtonCaptions.ContainsKey(code3))
				{
					return ProcessCaption(JoystickButtonCaptions[code3]);
				}
				return Game.Language.GetCaption("system.common.button") + " " + (code3 + 1);
			}
			if (evt is InputEventJoypadMotion eventMotion)
			{
				int code = eventMotion.Axis;
				float value = eventMotion.AxisValue;
				if (value > 0f && JoystickAxisPlusCaptions.ContainsKey(code))
				{
					return ProcessCaption(JoystickAxisPlusCaptions[code]);
				}
				if (JoystickAxisMinusCaptions.ContainsKey(code))
				{
					return ProcessCaption(JoystickAxisMinusCaptions[code]);
				}
				return Game.Language.GetCaption("system.common.axis") + " " + (code + 1) + ((value > 0f) ? " +" : " -");
			}
			return "-";
		}

		private string ProcessCaption(string caption)
		{
			if (caption.StartsWith("img:"))
			{
				string[] obj = new string[5] { "[img]", "res://assets/img/ui/input/", null, null, null };
				obj[2] = caption.Substring(4, caption.Length - 4);
				obj[3] = ".png";
				obj[4] = "[/img]";
				return string.Concat(obj);
			}
			return Game.Language.GetCaption(caption);
		}

		public int CompareTo(InputProfile profile)
		{
			return Order.CompareTo(profile.Order);
		}
	}
}
