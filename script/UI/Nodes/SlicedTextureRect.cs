using System;
using Godot;
using LacieEngine.API;

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
			get
			{
				return _texture;
			}
			set
			{
				_texture = value;
				UpdateTextureSlices();
			}
		}

		[Export(PropertyHint.Range, "1,256")]
		public int Hframes
		{
			get
			{
				return _hframes;
			}
			set
			{
				_hframes = Math.Max(1, value);
				UpdateTextureSlices();
			}
		}

		[Export(PropertyHint.Range, "1,256")]
		public int Vframes
		{
			get
			{
				return _vframes;
			}
			set
			{
				_vframes = Math.Max(1, value);
				UpdateTextureSlices();
			}
		}

		[Export(PropertyHint.Range, "0,65535")]
		public int Frame
		{
			get
			{
				return _frame;
			}
			set
			{
				_frame = Math.Max(0, value);
				UpdateFrame();
			}
		}

		public override void _EnterTree()
		{
			UpdateTextureSlices();
		}

		protected virtual void UpdateTextureSlices()
		{
			ready = false;
			if (IsInsideTree() && Texture != null)
			{
				frameTextures = new AtlasTexture[Hframes * Vframes];
				float frameSizeX = Texture.GetSize().x / (float)Hframes;
				float frameSizeY = Texture.GetSize().y / (float)Vframes;
				for (int i = 0; i < frameTextures.Length; i++)
				{
					float frameXPos = (float)(i % Hframes) * frameSizeX;
					float frameYPos = (float)(i / Hframes) * frameSizeY;
					frameTextures[i] = new AtlasTexture();
					frameTextures[i].Atlas = Texture;
					frameTextures[i].Region = new Rect2(frameXPos, frameYPos, frameSizeX, frameSizeY);
				}
				ready = true;
				UpdateFrame();
			}
		}

		protected virtual void UpdateFrame()
		{
			if (!ready)
			{
				UpdateTextureSlices();
			}
			if (ready)
			{
				if (_frame + 1 > Hframes * Vframes)
				{
					_frame = 0;
				}
				base.Texture = frameTextures[_frame];
				PropertyListChangedNotify();
			}
		}
	}
}
