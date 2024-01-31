// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.LabelWithInputIcons
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class LabelWithInputIcons : LabelWithImages
  {
    private string _captionKey;
    private string[] _inputs;

    public bool LookupCaption { get; set; } = true;

    public LabelWithInputIcons()
    {
    }

    public LabelWithInputIcons(string captionKey, params string[] inputs)
    {
      this.SetContent(captionKey, inputs);
    }

    public override void _Ready()
    {
      base._Ready();
      Game.Input.InputTypeChanged += new Action(this.RefreshCaptionAndIcons);
    }

    public override void _ExitTree()
    {
      Game.Input.InputTypeChanged -= new Action(this.RefreshCaptionAndIcons);
    }

    public void SetContent(string captionKey, params string[] inputs)
    {
      this._captionKey = captionKey;
      this._inputs = inputs ?? new string[0];
      this.RefreshCaptionAndIcons();
    }

    protected void RefreshCaptionAndIcons()
    {
      string[] strArray = new string[this._inputs.Length];
      for (int index = 0; index < this._inputs.Length; ++index)
        strArray[index] = Inputs.Caption(this._inputs[index]);
      if (this.LookupCaption)
        this.Text = Game.Language.GetFormattedCaption(this._captionKey, strArray);
      else
        this.Text = DrkieUtil.FormatString(this._captionKey, strArray);
    }
  }
}
