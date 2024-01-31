// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SlicedTextureRect
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.UI
{
  [Tool]
  [ExportType(icon = "image")]
  public class SlicedTextureRect : TextureRect
  {
    protected AtlasTexture[] frameTextures;
    protected bool ready;
    private Texture _texture;
    private int _hframes = 1;
    private int _vframes = 1;
    private int _frame;

    [Export(PropertyHint.None, "")]
    public new Texture Texture
    {
      get => this._texture;
      set
      {
        this._texture = value;
        this.UpdateTextureSlices();
      }
    }

    [Export(PropertyHint.Range, "1,256")]
    public int Hframes
    {
      get => this._hframes;
      set
      {
        this._hframes = Math.Max(1, value);
        this.UpdateTextureSlices();
      }
    }

    [Export(PropertyHint.Range, "1,256")]
    public int Vframes
    {
      get => this._vframes;
      set
      {
        this._vframes = Math.Max(1, value);
        this.UpdateTextureSlices();
      }
    }

    [Export(PropertyHint.Range, "0,65535")]
    public int Frame
    {
      get => this._frame;
      set
      {
        this._frame = Math.Max(0, value);
        this.UpdateFrame();
      }
    }

    public override void _EnterTree() => this.UpdateTextureSlices();

    protected virtual void UpdateTextureSlices()
    {
      this.ready = false;
      if (!this.IsInsideTree() || this.Texture == null)
        return;
      this.frameTextures = new AtlasTexture[this.Hframes * this.Vframes];
      float width = this.Texture.GetSize().x / (float) this.Hframes;
      float height = this.Texture.GetSize().y / (float) this.Vframes;
      for (int index = 0; index < this.frameTextures.Length; ++index)
      {
        float x = (float) (index % this.Hframes) * width;
        float y = (float) (index / this.Hframes) * height;
        this.frameTextures[index] = new AtlasTexture();
        this.frameTextures[index].Atlas = this.Texture;
        this.frameTextures[index].Region = new Rect2(x, y, width, height);
      }
      this.ready = true;
      this.UpdateFrame();
    }

    protected virtual void UpdateFrame()
    {
      if (!this.ready)
        this.UpdateTextureSlices();
      if (!this.ready)
        return;
      if (this._frame + 1 > this.Hframes * this.Vframes)
        this._frame = 0;
      base.Texture = (Texture) this.frameTextures[this._frame];
      this.PropertyListChangedNotify();
    }
  }
}
