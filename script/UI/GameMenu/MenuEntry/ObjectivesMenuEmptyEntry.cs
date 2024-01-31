// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.ObjectivesMenuEmptyEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.UI
{
  public class ObjectivesMenuEmptyEntry : IMenuEntry
  {
    public IMenuEntryList Parent { get; set; }

    public ObjectivesMenuEmptyEntry(IMenuEntryList parentMenu) => this.Parent = parentMenu;

    public Control DrawEntry() => UIUtil.CreateSimpleEntry("system.menu.objectives.empty");
  }
}
