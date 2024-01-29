using System;
using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public abstract class StoryPlayerCommand
	{
		[field: NonSerialized]
		public StoryPlayerEvent Event { get; set; }

		public int Index { get; set; }

		public string RawCommand { get; set; }

		public int IndentLv { get; set; } = -1;


		public abstract void Execute(StoryPlayer storyPlayer);

		public virtual void Load()
		{
			foreach (string res in GetDependencies())
			{
				Game.Memory.Cache(res);
			}
		}

		public virtual IList<string> GetDependencies()
		{
			return Array.Empty<string>();
		}

		public virtual bool Validate(out List<string> messages)
		{
			messages = null;
			return true;
		}

		public virtual void FindCaptions(TranslatableMessages captions)
		{
		}

		public virtual void OverrideCaptions(ICaptionSet captions)
		{
		}
	}
}
