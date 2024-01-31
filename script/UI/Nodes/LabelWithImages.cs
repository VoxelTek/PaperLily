// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.LabelWithImages
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Text.RegularExpressions;

#nullable disable
namespace LacieEngine.UI
{
  public class LabelWithImages : HBoxContainer
  {
    private const string _pattern = "\\[img.*?\\](.*?)\\[\\/img\\]";
    private static readonly Vector2 _imageSize = new Vector2(14f, 14f);
    private string _bbcode;

    public LabelWithImages()
    {
      this.Name = "TextLine";
      this._bbcode = "";
      this.SetSeparation(1);
    }

    public string Text
    {
      get => this._bbcode;
      set
      {
        this._bbcode = value;
        this.UpdateContent();
      }
    }

    public void SetLeftAlign() => this.Alignment = BoxContainer.AlignMode.Begin;

    public void SetCenterAlign() => this.Alignment = BoxContainer.AlignMode.Center;

    public void SetRightAlign() => this.Alignment = BoxContainer.AlignMode.End;

    private void UpdateContent()
    {
      this.Clear();
      int num1 = 0;
      int num2 = 0;
      foreach (Match match in Regex.Matches(this._bbcode, "\\[img.*?\\](.*?)\\[\\/img\\]"))
      {
        if (match.Index - num1 > 0)
        {
          Label label1 = GDUtil.MakeNode<Label>((++num2).ToString() + "Text");
          Label label2 = label1;
          string bbcode = this._bbcode;
          int num3 = num1;
          int startIndex = num3;
          int length = match.Index - num3;
          string str = bbcode.Substring(startIndex, length);
          label2.Text = str;
          label1.SetDefaultFontAndColor();
          this.AddChild((Node) label1);
        }
        TextureRect textureRect = GDUtil.MakeNode<TextureRect>((++num2).ToString() + "Image");
        textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        textureRect.Expand = true;
        textureRect.RectMinSize = LabelWithImages._imageSize;
        textureRect.Texture = GD.Load<Texture>(match.Groups[1].Value);
        this.AddChild((Node) textureRect);
        num1 = match.Index + match.Value.Length;
      }
      if (num1 >= this._bbcode.Length)
        return;
      int num4;
      Label label3 = GDUtil.MakeNode<Label>((num4 = num2 + 1).ToString() + "Text");
      Label label4 = label3;
      string bbcode1 = this._bbcode;
      int startIndex1 = num1;
      string str1 = bbcode1.Substring(startIndex1, bbcode1.Length - startIndex1);
      label4.Text = str1;
      label3.SetDefaultFontAndColor();
      this.AddChild((Node) label3);
    }
  }
}
