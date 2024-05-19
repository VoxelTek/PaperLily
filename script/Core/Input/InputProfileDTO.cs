// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.InputProfileDTO
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  internal class InputProfileDTO
  {
	public string Caption;
	public string Type;
	public string Order;
	public string CaptionBase;
	public List<InputMappingDTO> Mappings;
	public List<InputCaptionDTO> Captions;

	public InputProfileDTO()
	{
	  this.Mappings = new List<InputMappingDTO>();
	  this.Captions = new List<InputCaptionDTO>();
	}

	public InputProfileDTO(InputProfile profile)
	{
	  this.Caption = profile.Caption;
	  this.Type = profile.Type == InputProfile.InputType.Keyboard ? "keyboard" : "controller";
	  this.Order = profile.Order.ToString();
	  this.CaptionBase = profile.CaptionBase;
	  this.Mappings = new List<InputMappingDTO>();
	  foreach (InputProfile.Mapping mapping in profile.Mappings)
		this.Mappings.Add(this.MappingToDTO(mapping));
	  foreach (uint key in profile.KeyCaptions.Keys)
	  {
		InputCaptionDTO inputCaptionDto = new InputCaptionDTO()
		{
		  Key = new InputMappingKeyDTO()
		};
		inputCaptionDto.Key.Code = key;
		inputCaptionDto.Caption = profile.KeyCaptions[key];
		this.Captions.Add(inputCaptionDto);
	  }
	  foreach (int key in profile.JoystickButtonCaptions.Keys)
	  {
		InputCaptionDTO inputCaptionDto = new InputCaptionDTO()
		{
		  Button = new InputMappingButtonDTO()
		};
		inputCaptionDto.Button.Code = key;
		inputCaptionDto.Caption = profile.JoystickButtonCaptions[key];
		this.Captions.Add(inputCaptionDto);
	  }
	  foreach (int key in profile.JoystickAxisMinusCaptions.Keys)
	  {
		InputCaptionDTO inputCaptionDto = new InputCaptionDTO()
		{
		  Axis = new InputMappingAxisDTO()
		};
		inputCaptionDto.Axis.Code = key;
		inputCaptionDto.Axis.Value = -1;
		inputCaptionDto.Caption = profile.JoystickAxisMinusCaptions[key];
		this.Captions.Add(inputCaptionDto);
	  }
	  foreach (int key in profile.JoystickAxisPlusCaptions.Keys)
	  {
		InputCaptionDTO inputCaptionDto = new InputCaptionDTO()
		{
		  Axis = new InputMappingAxisDTO()
		};
		inputCaptionDto.Axis.Code = key;
		inputCaptionDto.Axis.Value = 1;
		inputCaptionDto.Caption = profile.JoystickAxisPlusCaptions[key];
		this.Captions.Add(inputCaptionDto);
	  }
	}

	private InputMappingDTO MappingToDTO(InputProfile.Mapping mapping)
	{
	  InputMappingDTO dto = new InputMappingDTO();
	  dto.Action = mapping.Action;
	  if (mapping.Event is InputEventKey inputEventKey)
	  {
		dto.Key = new InputMappingKeyDTO();
		dto.Key.Code = inputEventKey.Scancode;
	  }
	  else if (mapping.Event is InputEventJoypadButton eventJoypadButton)
	  {
		dto.Button = new InputMappingButtonDTO();
		dto.Button.Code = eventJoypadButton.ButtonIndex;
	  }
	  else if (mapping.Event is InputEventJoypadMotion eventJoypadMotion)
	  {
		dto.Axis = new InputMappingAxisDTO();
		dto.Axis.Code = eventJoypadMotion.Axis;
		dto.Axis.Value = (double) eventJoypadMotion.AxisValue > 0.0 ? 1 : 0;
	  }
	  return dto;
	}

	public InputProfile ToInputProfile()
	{
	  InputProfile inputProfile = new InputProfile(this.Type == "keyboard" ? InputProfile.InputType.Keyboard : InputProfile.InputType.Controller);
	  inputProfile.Caption = this.Caption;
	  inputProfile.CaptionBase = this.CaptionBase;
	  if (!this.Order.IsNullOrEmpty())
		inputProfile.Order = int.Parse(this.Order);
	  foreach (InputMappingDTO mapping in this.Mappings)
	  {
		if (mapping.Key != null)
		  inputProfile.AddKeyMapping(mapping.Action, mapping.Key.Code);
		else if (mapping.Button != null)
		  inputProfile.AddButtonMapping(mapping.Action, mapping.Button.Code);
		else if (mapping.Axis != null)
		  inputProfile.AddAxisMapping(mapping.Action, mapping.Axis.Code, mapping.Axis.Value);
	  }
	  foreach (InputCaptionDTO caption in this.Captions)
	  {
		if (caption.Key != null)
		  inputProfile.AddKeyCaption(caption.Key.Code, caption.Caption);
		else if (caption.Button != null)
		  inputProfile.AddButtonCaption(caption.Button.Code, caption.Caption);
		else if (caption.Axis != null)
		  inputProfile.AddAxisCaption(caption.Axis.Code, caption.Axis.Value, caption.Caption);
	  }
	  return inputProfile;
	}
  }
}
