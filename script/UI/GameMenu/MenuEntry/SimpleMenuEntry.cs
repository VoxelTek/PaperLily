// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SimpleMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class SimpleMenuEntry : IMenuEntry
  {
    private string _caption;
    private Action _action;

    public IMenuEntryList Parent { get; set; }

    public SimpleMenuEntry(string caption, Action action, IMenuEntryList parent)
    {
      this.Parent = parent;
      this._caption = caption;
      this._action = action;
    }

    public Control DrawEntry() => UIUtil.CreateSimpleEntry(this._caption);

    public void Accept() => this._action();
  }
}
