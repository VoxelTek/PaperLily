// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.FullScreenMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

#nullable disable
namespace LacieEngine.UI
{
  public class FullScreenMenuEntry : IMenuEntry
  {
    private FullScreenSetting setting;
    private Label captionLabel;
    private Label valueLabel;

    public IMenuEntryList Parent { get; set; }

    public FullScreenMenuEntry(IMenuEntryList menu)
    {
      this.setting = new FullScreenSetting();
      this.Parent = menu;
    }

    public void Left()
    {
      if (!this.setting.OwnSound)
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
      this.setting.Decrement();
      this.setting.Apply();
      this.Parent.Update();
    }

    public void Right()
    {
      if (!this.setting.OwnSound)
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
      this.setting.Increment();
      this.setting.Apply();
      this.Parent.Update();
    }

    public Control DrawEntry()
    {
      MarginContainer container = new MarginContainer();
      container.SetContainerMarginLeft(10);
      container.SetContainerMarginRight(10);
      this.captionLabel = new Label();
      this.captionLabel.Name = "Label";
      this.captionLabel.Text = this.setting.CaptionLabel();
      this.captionLabel.Align = Label.AlignEnum.Left;
      this.captionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
      this.captionLabel.SetDefaultFontAndColor();
      this.valueLabel = (Label) new FullScreenMenuEntry.FullScreenLabel(this.setting);
      this.valueLabel.Name = "Value";
      this.valueLabel.Text = this.setting.ValueLabel();
      this.valueLabel.Align = Label.AlignEnum.Right;
      this.valueLabel.SetDefaultFontAndColor();
      container.AddChild((Node) this.captionLabel);
      container.AddChild((Node) this.valueLabel);
      return (Control) container;
    }

    public class FullScreenLabel : Label
    {
      private FullScreenSetting setting;

      public FullScreenLabel(FullScreenSetting setting) => this.setting = setting;

      public override void _Process(float delta) => this.Text = this.setting.ValueLabel();
    }
  }
}
