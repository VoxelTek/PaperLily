// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InputBindingMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class InputBindingMenuEntry : IMenuEntry
  {
    private string caption;
    private string action;
    private Label captionLabel;
    private LabelWithImages valueLabel;

    public IMenuEntryList Parent { get; set; }

    public InputBindingMenuEntry(IMenuEntryList parentMenu, string caption, string action)
    {
      this.caption = caption;
      this.action = action;
      this.Parent = parentMenu;
    }

    public void Accept()
    {
      Game.InputProcessor = Inputs.Processor.Binding;
      if (!Game.Settings.InputProfileCustom)
      {
        InputMapper.CloneIntoCustomProfile(Game.Settings.InputProfile);
        Game.Settings.SetInputProfile("custom");
      }
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) new InputBindingScreen(this.action, this.caption, (Action) (() => this.Parent.Update())));
    }

    public Control DrawEntry()
    {
      MarginContainer container = new MarginContainer();
      container.SetContainerMarginLeft(10);
      container.SetContainerMarginRight(10);
      this.captionLabel = new Label();
      this.captionLabel.Name = "Label";
      this.captionLabel.Text = this.caption;
      this.captionLabel.Align = Label.AlignEnum.Left;
      this.captionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
      this.captionLabel.SetDefaultFontAndColor();
      this.valueLabel = GDUtil.MakeNode<LabelWithImages>("VolumeLabel");
      this.valueLabel.SetRightAlign();
      container.AddChild((Node) this.captionLabel);
      container.AddChild((Node) this.valueLabel);
      this.Update();
      return (Control) container;
    }

    public void Update()
    {
      this.valueLabel.Text = Inputs.Profiles[Game.Settings.InputProfile].GetAllCaptionsForAction(this.action);
    }
  }
}
