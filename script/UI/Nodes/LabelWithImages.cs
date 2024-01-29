using System.Text.RegularExpressions;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class LabelWithImages : HBoxContainer
	{
		private const string _pattern = "\\[img.*?\\](.*?)\\[\\/img\\]";

		private static readonly Vector2 _imageSize = new Vector2(14f, 14f);

		private string _bbcode;

		public string Text
		{
			get
			{
				return _bbcode;
			}
			set
			{
				_bbcode = value;
				UpdateContent();
			}
		}

		public LabelWithImages()
		{
			base.Name = "TextLine";
			_bbcode = "";
			this.SetSeparation(1);
		}

		public void SetLeftAlign()
		{
			base.Alignment = AlignMode.Begin;
		}

		public void SetCenterAlign()
		{
			base.Alignment = AlignMode.Center;
		}

		public void SetRightAlign()
		{
			base.Alignment = AlignMode.End;
		}

		private void UpdateContent()
		{
			this.Clear();
			int pointer = 0;
			int index = 0;
			foreach (Match i in Regex.Matches(_bbcode, "\\[img.*?\\](.*?)\\[\\/img\\]"))
			{
				int num;
				if (i.Index - pointer > 0)
				{
					num = ++index;
					Label text2 = GDUtil.MakeNode<Label>(num + "Text");
					string bbcode = _bbcode;
					num = pointer;
					text2.Text = bbcode.Substring(num, i.Index - num);
					text2.SetDefaultFontAndColor();
					AddChild(text2);
				}
				num = ++index;
				TextureRect image = GDUtil.MakeNode<TextureRect>(num + "Image");
				image.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
				image.Expand = true;
				image.RectMinSize = _imageSize;
				image.Texture = GD.Load<Texture>(i.Groups[1].Value);
				AddChild(image);
				pointer = i.Index + i.Value.Length;
			}
			if (pointer < _bbcode.Length)
			{
				int num = ++index;
				Label text = GDUtil.MakeNode<Label>(num + "Text");
				string bbcode2 = _bbcode;
				num = pointer;
				text.Text = bbcode2.Substring(num, bbcode2.Length - num);
				text.SetDefaultFontAndColor();
				AddChild(text);
			}
		}
	}
}
