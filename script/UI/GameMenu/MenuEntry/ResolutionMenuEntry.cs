// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.ResolutionMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Settings;

#nullable disable
namespace LacieEngine.UI
{
  public class ResolutionMenuEntry : SettingMenuEntry<ResolutionSetting, Vector2>
  {
    public ResolutionMenuEntry(IMenuEntryList menu)
      : base(menu)
    {
    }

    public override Control DrawEntry()
    {
      MarginContainer container = new MarginContainer();
      container.SetContainerMarginLeft(10);
      container.SetContainerMarginRight(10);
      this.nCaptionLabel = GDUtil.MakeNode<Label>("Label");
      this.nCaptionLabel.Text = this.setting.CaptionLabel();
      this.nCaptionLabel.Align = Label.AlignEnum.Left;
      this.nCaptionLabel.SetAnchorsPreset(Control.LayoutPreset.LeftWide);
      this.nCaptionLabel.SetDefaultFontAndColor();
      this.nValueLabel = (Label) new ResolutionMenuEntry.ResolutionLabel((ResolutionSetting) this.setting);
      this.nValueLabel.Name = "Value";
      this.nValueLabel.Text = this.setting.ValueLabel();
      this.nValueLabel.Align = Label.AlignEnum.Right;
      this.nValueLabel.SetDefaultFontAndColor();
      container.AddChild((Node) this.nCaptionLabel);
      container.AddChild((Node) this.nValueLabel);
      return (Control) container;
    }

    public class ResolutionLabel : Label
    {
      private ResolutionSetting setting;

      public ResolutionLabel(ResolutionSetting setting) => this.setting = setting;

      public override void _Ready()
      {
        this.ApplyCurrentResolution();
        int num = (int) this.GetTree().Root.Connect("size_changed", (Object) this, "ApplyCurrentResolution");
      }

      public override void _Process(float delta) => this.Text = this.setting.ValueLabel();

      public void ApplyCurrentResolution() => this.setting.ApplyCurrentResolution();
    }
  }
}
