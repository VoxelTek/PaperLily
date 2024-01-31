// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ControlExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Core
{
  public static class ControlExtension
  {
    public static void SetAnchorsAndMarginsPreset(
      this Control control,
      Control.LayoutPreset layout,
      Vector2 size)
    {
      switch (layout)
      {
        case Control.LayoutPreset.TopLeft:
          control.AnchorLeft = 0.0f;
          control.AnchorTop = 0.0f;
          control.AnchorRight = 0.0f;
          control.AnchorBottom = 0.0f;
          control.MarginLeft = 0.0f;
          control.MarginTop = 0.0f;
          control.MarginRight = size.x;
          control.MarginBottom = size.y;
          break;
        case Control.LayoutPreset.TopRight:
          control.AnchorLeft = 1f;
          control.AnchorTop = 0.0f;
          control.AnchorRight = 1f;
          control.AnchorBottom = 0.0f;
          control.MarginLeft = -size.x;
          control.MarginTop = 0.0f;
          control.MarginRight = 0.0f;
          control.MarginBottom = size.y;
          break;
        case Control.LayoutPreset.BottomLeft:
          control.AnchorLeft = 0.0f;
          control.AnchorTop = 1f;
          control.AnchorRight = 0.0f;
          control.AnchorBottom = 1f;
          control.MarginLeft = 0.0f;
          control.MarginTop = -size.y;
          control.MarginRight = size.x;
          control.MarginBottom = 0.0f;
          break;
        case Control.LayoutPreset.BottomRight:
          control.AnchorLeft = 1f;
          control.AnchorTop = 1f;
          control.AnchorRight = 1f;
          control.AnchorBottom = 1f;
          control.MarginLeft = -size.x;
          control.MarginTop = -size.y;
          control.MarginRight = 0.0f;
          control.MarginBottom = 0.0f;
          break;
        case Control.LayoutPreset.CenterTop:
          control.AnchorLeft = 0.5f;
          control.AnchorTop = 0.0f;
          control.AnchorRight = 0.5f;
          control.AnchorBottom = 0.0f;
          control.MarginLeft = (float) (-(double) size.x / 2.0);
          control.MarginTop = 0.0f;
          control.MarginRight = size.x / 2f;
          control.MarginBottom = size.y;
          break;
        case Control.LayoutPreset.CenterBottom:
          control.AnchorLeft = 0.5f;
          control.AnchorTop = 1f;
          control.AnchorRight = 0.5f;
          control.AnchorBottom = 1f;
          control.MarginLeft = (float) (-(double) size.x / 2.0);
          control.MarginTop = -size.y;
          control.MarginRight = size.x / 2f;
          control.MarginBottom = 0.0f;
          break;
        case Control.LayoutPreset.LeftWide:
          control.AnchorLeft = 0.0f;
          control.AnchorTop = 0.0f;
          control.AnchorRight = 0.0f;
          control.AnchorBottom = 1f;
          control.MarginLeft = 0.0f;
          control.MarginTop = 0.0f;
          control.MarginRight = size.x;
          control.MarginBottom = 0.0f;
          break;
        case Control.LayoutPreset.TopWide:
          control.AnchorLeft = 0.0f;
          control.AnchorTop = 0.0f;
          control.AnchorRight = 1f;
          control.AnchorBottom = 0.0f;
          control.MarginLeft = 0.0f;
          control.MarginTop = 0.0f;
          control.MarginRight = 0.0f;
          control.MarginBottom = size.y;
          break;
        case Control.LayoutPreset.RightWide:
          control.AnchorLeft = 1f;
          control.AnchorTop = 0.0f;
          control.AnchorRight = 1f;
          control.AnchorBottom = 1f;
          control.MarginLeft = -size.x;
          control.MarginTop = 0.0f;
          control.MarginRight = 0.0f;
          control.MarginBottom = 0.0f;
          break;
        case Control.LayoutPreset.BottomWide:
          control.AnchorLeft = 0.0f;
          control.AnchorTop = 1f;
          control.AnchorRight = 1f;
          control.AnchorBottom = 1f;
          control.MarginLeft = 0.0f;
          control.MarginTop = -size.y;
          control.MarginRight = 0.0f;
          control.MarginBottom = 0.0f;
          break;
        default:
          Log.Warn((object) ("Layout type not supported, using default behavior: " + layout.ToString()));
          control.SetAnchorsAndMarginsPreset(layout);
          break;
      }
    }

    public static void SetAnchorsGrowFrom(
      this Control control,
      float pointX,
      float pointY,
      Direction growTowards)
    {
      control.AnchorLeft = control.AnchorRight = pointX;
      control.AnchorTop = control.AnchorBottom = pointY;
      Control control1 = control;
      Control.GrowDirection growDirection1;
      switch (growTowards.ToEnum())
      {
        case Direction.Enum.None:
          growDirection1 = Control.GrowDirection.Both;
          break;
        case Direction.Enum.Left:
          growDirection1 = Control.GrowDirection.Begin;
          break;
        case Direction.Enum.Up:
          growDirection1 = Control.GrowDirection.Both;
          break;
        case Direction.Enum.UpLeft:
          growDirection1 = Control.GrowDirection.Begin;
          break;
        case Direction.Enum.Right:
          growDirection1 = Control.GrowDirection.End;
          break;
        case Direction.Enum.UpRight:
          growDirection1 = Control.GrowDirection.End;
          break;
        case Direction.Enum.Down:
          growDirection1 = Control.GrowDirection.Both;
          break;
        case Direction.Enum.DownLeft:
          growDirection1 = Control.GrowDirection.Begin;
          break;
        case Direction.Enum.DownRight:
          growDirection1 = Control.GrowDirection.End;
          break;
        default:
          growDirection1 = Control.GrowDirection.Both;
          break;
      }
      control1.GrowHorizontal = growDirection1;
      Control control2 = control;
      Control.GrowDirection growDirection2;
      switch (growTowards.ToEnum())
      {
        case Direction.Enum.None:
          growDirection2 = Control.GrowDirection.Both;
          break;
        case Direction.Enum.Left:
          growDirection2 = Control.GrowDirection.Both;
          break;
        case Direction.Enum.Up:
          growDirection2 = Control.GrowDirection.Begin;
          break;
        case Direction.Enum.UpLeft:
          growDirection2 = Control.GrowDirection.Begin;
          break;
        case Direction.Enum.Right:
          growDirection2 = Control.GrowDirection.Both;
          break;
        case Direction.Enum.UpRight:
          growDirection2 = Control.GrowDirection.Begin;
          break;
        case Direction.Enum.Down:
          growDirection2 = Control.GrowDirection.End;
          break;
        case Direction.Enum.DownLeft:
          growDirection2 = Control.GrowDirection.End;
          break;
        case Direction.Enum.DownRight:
          growDirection2 = Control.GrowDirection.End;
          break;
        default:
          growDirection2 = Control.GrowDirection.Both;
          break;
      }
      control2.GrowVertical = growDirection2;
    }

    public static void SetAnchorsGrowFrom(this Control control, float pointX, float pointY)
    {
      control.SetAnchorsGrowFrom(pointX, pointY, Direction.None);
    }

    public static void SetAnchorsGrowFrom(
      this Control control,
      Vector2 point,
      Direction growTowards)
    {
      control.SetAnchorsGrowFrom(point.x, point.y, growTowards);
    }

    public static void SetAnchorsGrowFrom(this Control control, Vector2 point)
    {
      control.SetAnchorsGrowFrom(point, Direction.None);
    }

    public static void SetSeparation(this BoxContainer box, int value)
    {
      box.Set("custom_constants/separation", (object) value);
    }

    public static void SetSeparation(this GridContainer box, int xValue, int yValue)
    {
      box.Set("custom_constants/hseparation", (object) xValue);
      box.Set("custom_constants/vseparation", (object) yValue);
    }

    public static void SetContainerMarginLeft(this MarginContainer container, int value)
    {
      container.Set("custom_constants/margin_left", (object) value);
    }

    public static void SetContainerMarginTop(this MarginContainer container, int value)
    {
      container.Set("custom_constants/margin_top", (object) value);
    }

    public static void SetContainerMarginRight(this MarginContainer container, int value)
    {
      container.Set("custom_constants/margin_right", (object) value);
    }

    public static void SetContainerMarginBottom(this MarginContainer container, int value)
    {
      container.Set("custom_constants/margin_bottom", (object) value);
    }

    public static void SetContainerMargins(this MarginContainer container, int value)
    {
      container.SetContainerMarginLeft(value);
      container.SetContainerMarginTop(value);
      container.SetContainerMarginRight(value);
      container.SetContainerMarginBottom(value);
    }

    public static void SetFont(this Label label, string fontName)
    {
      string path = fontName.StartsWith("res://") ? fontName : "res://resources/font/" + fontName + ".tres";
      label.AddFontOverride("font", GD.Load<Font>(path));
    }

    public static void SetDefaultFontAndColor(this Label label)
    {
      label.SetFont("res://resources/font/default.tres");
      label.SetFontColor(Constants.Colors.DefaultTextColor);
    }

    public static void SetFontColor(this Label label, Color color)
    {
      label.AddColorOverride("font_color", color);
    }

    public static void SetFontShadowColor(this Label label, Color color)
    {
      label.AddColorOverride("font_color_shadow", color);
    }

    public static void SetFontOutlineColor(this Label label, Color color)
    {
      label.AddColorOverride("font_outline_modulate", color);
    }

    public static void SetFont(this RichTextLabel label, string fontName)
    {
      string path = fontName.StartsWith("res://") ? fontName : "res://resources/font/" + fontName + ".tres";
      label.AddFontOverride("normal_font", GD.Load<Font>(path));
    }

    public static void SetDefaultFontAndColor(this RichTextLabel label)
    {
      label.SetFont("res://resources/font/default.tres");
      label.SetFontColor(Constants.Colors.DefaultTextColor);
    }

    public static void SetFontColor(this RichTextLabel label, Color color)
    {
      label.AddColorOverride("default_color", color);
    }

    public static Color GetPixelAt(this TextureRect texture, Vector2 coords)
    {
      Image data = texture.Texture.GetData();
      data.Lock();
      Color pixel = data.GetPixel((int) coords.x, (int) coords.y);
      data.Unlock();
      return pixel;
    }

    public static void CollapseAll(this TreeItem treeItem)
    {
      treeItem.Collapsed = true;
      TreeItem children = treeItem.GetChildren();
      if (children != null)
        children.CollapseAll();
      TreeItem next = treeItem.GetNext();
      if (next == null)
        return;
      next.CollapseAll();
    }
  }
}
