using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public static class UIUtil
	{
		public static readonly Color Transparent = new Color(1f, 1f, 1f, 0f);

		public static readonly Color SelectionColor = new Color(0.93f, 0.87f, 0.87f, 0.2f);

		public static readonly Color ScreenDarkenerColor = new Color(0f, 0f, 0f, 0.7f);

		public static readonly Color MenuBgColor = new Color("#11080D");

		public const string BulletPoint = "[img=11]res://assets/img/ui/bullet_point.png[/img]";

		public static ColorRect CreateOverlay(Color color)
		{
			ColorRect colorRect = GDUtil.MakeNode<ColorRect>("Overlay");
			colorRect.Color = color;
			colorRect.RectMinSize = Game.Settings.BaseResolution;
			return colorRect;
		}

		public static ColorRect CreateDarkenerOverlay()
		{
			return CreateOverlay(ScreenDarkenerColor);
		}

		public static Frame CreateInfoBox(string text)
		{
			InfoBoxFrame infoBoxFrame = new InfoBoxFrame();
			Label label = GDUtil.MakeNode<Label>("Info");
			label.Text = text;
			label.Align = Label.AlignEnum.Center;
			label.SetDefaultFontAndColor();
			infoBoxFrame.AddToFrame(label);
			return infoBoxFrame;
		}

		public static Frame CreateInfoBoxWithImages(string bbcode)
		{
			InfoBoxFrame infoBoxFrame = new InfoBoxFrame();
			LabelWithImages text = GDUtil.MakeNode<LabelWithImages>("Text");
			text.Text = bbcode;
			infoBoxFrame.AddToFrame(text);
			return infoBoxFrame;
		}

		public static Frame CreateInfoBoxWithInputIcons(string captionKey, params string[] inputs)
		{
			InfoBoxFrame infoBoxFrame = new InfoBoxFrame();
			LabelWithInputIcons text = GDUtil.MakeNode<LabelWithInputIcons>("Text");
			text.SetContent(captionKey, inputs);
			infoBoxFrame.AddToFrame(text);
			return infoBoxFrame;
		}

		public static Frame CreateInfoBoxWithIcon(string text, string iconPath)
		{
			InfoBoxFrame infoBoxFrame = new InfoBoxFrame();
			MarginContainer margin = new MarginContainer();
			margin.SetContainerMarginRight(5);
			HBoxContainer container = new HBoxContainer();
			container.SetSeparation(3);
			TextureRect icon = GDUtil.MakeNode<TextureRect>("Icon");
			icon.RectMinSize = new Vector2(40f, 40f);
			icon.Expand = true;
			icon.Texture = GD.Load("res://assets/sprite/common/item/" + iconPath + ".png") as Texture;
			Label label = GDUtil.MakeNode<Label>("Info");
			label.Text = text;
			label.Align = Label.AlignEnum.Center;
			label.SetDefaultFontAndColor();
			container.AddChild(icon);
			container.AddChild(label);
			margin.AddChild(container);
			infoBoxFrame.AddToFrame(margin);
			return infoBoxFrame;
		}

		public static Control CreateOptionList(List<string> options)
		{
			VBoxContainer container = GDUtil.MakeNode<VBoxContainer>("List");
			container.SetSeparation(0);
			int counter = 0;
			foreach (string option in options)
			{
				MarginContainer opContainer = GDUtil.MakeNode<MarginContainer>("Option" + counter++);
				ColorRect opBg = GDUtil.MakeNode<ColorRect>("Bg");
				opBg.Color = Transparent;
				MarginContainer labelContainer = new MarginContainer();
				labelContainer.SetContainerMarginLeft(10);
				labelContainer.SetContainerMarginRight(10);
				Label opLabel = GDUtil.MakeNode<Label>("Label");
				opLabel.Text = option;
				opLabel.Align = Label.AlignEnum.Center;
				opLabel.SetDefaultFontAndColor();
				labelContainer.AddChild(opLabel);
				opContainer.AddChild(opBg);
				opContainer.AddChild(labelContainer);
				container.AddChild(opContainer);
			}
			return container;
		}

		public static Control CreateVerticalEntryList(List<IMenuEntry> entries, out List<ColorRect> selectBgs, int separation = 5)
		{
			VBoxContainer container = GDUtil.MakeNode<VBoxContainer>("EntryList");
			container.SetSeparation(separation);
			selectBgs = new List<ColorRect>();
			int i = 0;
			foreach (IMenuEntry entry in entries)
			{
				MarginContainer entryContainer = GDUtil.MakeNode<MarginContainer>("Option" + i++);
				ColorRect bg = GDUtil.MakeNode<ColorRect>("Bg");
				bg.Color = Transparent;
				selectBgs.Add(bg);
				Control row = entry.DrawEntry();
				entryContainer.AddChild(bg);
				entryContainer.AddChild(row);
				container.AddChild(entryContainer);
			}
			return container;
		}

		public static Control CreateSimpleEntry(string caption)
		{
			MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("LabelContainer");
			Label label = GDUtil.MakeNode<Label>("Label");
			label.Text = caption;
			label.Align = Label.AlignEnum.Center;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			label.SetDefaultFontAndColor();
			marginContainer.AddChild(label);
			return marginContainer;
		}

		public static void HighlightItem(Control list, int selection)
		{
			if (list.Name == "List")
			{
				foreach (Node child in list.GetChildren())
				{
					((ColorRect)child.GetNode("Bg")).Color = Transparent;
				}
				if (selection != -1)
				{
					((ColorRect)list.GetNode("Option" + selection + "/Bg")).Color = SelectionColor;
				}
			}
			else
			{
				if (!(list.Name == "LargeList"))
				{
					return;
				}
				foreach (Node child2 in list.GetChildren())
				{
					((ColorRect)child2.GetNode("Bg")).Color = Transparent;
					((Control)child2.GetNode("Border")).Visible = false;
				}
				if (selection != -1)
				{
					((ColorRect)list.GetNode("Option" + selection + "/Bg")).Color = SelectionColor;
					((Control)list.GetNode("Option" + selection + "/Border")).Visible = true;
				}
			}
		}

		public static void HandleVerticalNavigationInput(IMenuEntryList menu, InputEvent @event)
		{
			string input = Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, allowEcho: true);
			if (input == null)
			{
				return;
			}
			switch (input)
			{
			case "input_up":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (--menu.Selection <= -1)
				{
					menu.Selection = menu.Entries.Count - 1;
				}
				menu.HighlightSelection();
				break;
			case "input_down":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (++menu.Selection >= menu.Entries.Count)
				{
					menu.Selection = 0;
				}
				menu.HighlightSelection();
				break;
			case "input_left":
				menu.Entries[menu.Selection].Left();
				break;
			case "input_right":
				menu.Entries[menu.Selection].Right();
				break;
			case "input_action":
				menu.Entries[menu.Selection].Accept();
				break;
			case "input_cancel":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
				menu.Back();
				break;
			}
		}

		public static float GetScaleFactor()
		{
			return Game.Root.Size.x / Game.Root.GetSizeOverride().x;
		}

		public static float GetScaled(float pixels)
		{
			return pixels;
		}

		public static int GetScaled(int pixels)
		{
			return pixels;
		}

		public static Vector2 GetScaled(Vector2 size)
		{
			return size;
		}
	}
}
