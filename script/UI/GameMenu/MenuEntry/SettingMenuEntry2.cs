// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SettingMenuEntry2`2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

#nullable disable
namespace LacieEngine.UI
{
  public class SettingMenuEntry2<T, U> : IMenuEntry where T : Setting<U>
  {
    protected Setting<U> setting;
    protected Label nCaptionLabel;
    protected Label nValueLabel;

    public IMenuEntryList Parent { get; set; }

    public SettingMenuEntry2(IMenuEntryList menu)
    {
      this.setting = Game.Settings.GetSetting<U, T>();
      this.Parent = menu;
    }

    public virtual void Left()
    {
      if (!this.setting.OwnSound)
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
      this.setting.Decrement();
      this.setting.Apply();
      this.Parent.Update();
    }

    public virtual void Right()
    {
      if (!this.setting.OwnSound)
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
      this.setting.Increment();
      this.setting.Apply();
      this.Parent.Update();
    }

    public virtual Control DrawEntry()
    {
      MarginContainer container = new MarginContainer();
      container.SetContainerMarginLeft(10);
      container.SetContainerMarginRight(10);
      this.nCaptionLabel = GDUtil.MakeNode<Label>("Label");
      this.nCaptionLabel.Text = this.setting.CaptionLabel();
      this.nCaptionLabel.Align = Label.AlignEnum.Left;
      this.nCaptionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
      this.nCaptionLabel.SetDefaultFontAndColor();
      this.nValueLabel = GDUtil.MakeNode<Label>("Value");
      this.nValueLabel.Text = this.setting.ValueLabel();
      this.nValueLabel.Align = Label.AlignEnum.Right;
      this.nValueLabel.SetDefaultFontAndColor();
      container.AddChild((Node) this.nCaptionLabel);
      container.AddChild((Node) this.nValueLabel);
      return (Control) container;
    }

    public void Update()
    {
      this.nCaptionLabel.Text = this.setting.CaptionLabel();
      this.nValueLabel.Text = this.setting.ValueLabel();
    }
  }
}
