// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SimpleVerticalMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public abstract class SimpleVerticalMenu : IMenuEntryList
  {
    protected List<ColorRect> _selectBgs;

    public IMenuEntryContainer Container { get; set; }

    public IMenuEntryList Parent { get; set; }

    public List<IMenuEntry> Entries { get; protected set; }

    public int Selection { get; set; }

    public Action OnBack { get; set; }

    public virtual Control DrawContent()
    {
      return UIUtil.CreateVerticalEntryList(this.Entries, out this._selectBgs);
    }

    public virtual void HandleInput(InputEvent @event)
    {
      UIUtil.HandleVerticalNavigationInput((IMenuEntryList) this, @event);
    }

    public virtual void HighlightSelection()
    {
      foreach (ColorRect selectBg in this._selectBgs)
        selectBg.Color = Colors.Transparent;
      if (this.Selection <= -1)
        return;
      this._selectBgs[this.Selection].Color = UIUtil.SelectionColor;
    }

    public void Root()
    {
      this.Container.Clear();
      this.Container.AddToFrame(this.DrawContent());
      this.Container.Menu = (IMenuEntryList) this;
      this.ResetSelection();
    }

    public virtual void Back()
    {
      if (this.OnBack != null)
        this.OnBack();
      else
        this.Parent?.Root();
    }

    public void ResetSelection()
    {
      this.Selection = 0;
      this.HighlightSelection();
    }
  }
}
