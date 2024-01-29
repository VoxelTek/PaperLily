using System.Collections.Generic;

namespace LacieEngine.Characters
{
	internal class CharacterDTO
	{
		public class CharacterSpriteDTO
		{
			public string State;

			public string TextureSuffix;

			public int Hframes;

			public int Vframes;

			public string LeftFrames;

			public string UpFrames;

			public string RightFrames;

			public string DownFrames;

			public string Offset;

			public float Fps;

			public bool Loop;
		}

		public string Name;

		public List<string> Costumes = new List<string>();

		public List<CharacterSpriteDTO> Sprites = new List<CharacterSpriteDTO>();

		public bool NoDefaultSprites;
	}
}
