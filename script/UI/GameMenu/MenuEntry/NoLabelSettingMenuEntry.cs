// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.NoLabelSettingMenuEntry`2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

#nullable disable
namespace LacieEngine.UI
{
  public class NoLabelSettingMenuEntry<T, U> : IMenuEntry where T : Setting<U>, new()
  {
    private Setting<U> setting;
    private Label valueLabel;

    public IMenuEntryList Parent { get; set; }

    public NoLabelSettingMenuEntry(IMenuEntryList menu)
    {
      this.setting = (Setting<U>) new T();
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
      this.valueLabel = GDUtil.MakeNode<Label>("Value");
      this.valueLabel.Text = this.setting.ValueLabel();
      this.valueLabel.Align = Label.AlignEnum.Center;
      this.valueLabel.SetDefaultFontAndColor();
      container.AddChild((Node) this.valueLabel);
      return (Control) container;
    }

    public void Update() => this.valueLabel.Text = this.setting.ValueLabel();
  }
}
