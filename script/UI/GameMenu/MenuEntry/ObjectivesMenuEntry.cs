// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.ObjectivesMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class ObjectivesMenuEntry : IMenuEntry
  {
    private IObjective _objective;

    public IMenuEntryList Parent { get; set; }

    public ObjectivesMenuEntry(IObjective objective, IMenuEntryList parentMenu)
    {
      this.Parent = parentMenu;
      this._objective = objective;
    }

    public Control DrawEntry()
    {
      MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("LabelContainer");
      RichTextLabel label = GDUtil.MakeNode<RichTextLabel>("ObjectiveName");
      label.ScrollActive = false;
      label.BbcodeEnabled = true;
      label.FitContentHeight = true;
      label.SetDefaultFontAndColor();
      label.BbcodeText = "[img=11]res://assets/img/ui/bullet_point.png[/img] " + this._objective.Name;
      label.SetAnchorsPreset(Control.LayoutPreset.Wide);
      marginContainer.AddChild((Node) label);
      return (Control) marginContainer;
    }
  }
}
