using System;

namespace LacieEngine.Translations
{
	public class LanguageCaption
	{
		public string Id { get; private set; }

		public string Str { get; private set; } = string.Empty;


		public string Context { get; private set; }

		public LanguageCaption(string id)
		{
			Id = id;
		}

		public LanguageCaption(string id, string str)
		{
			Id = id;
			Str = str;
		}

		public LanguageCaption(string id, string str, string context)
		{
			Id = id;
			Str = str;
			Context = context;
		}

		public override bool Equals(object obj)
		{
			if (obj is LanguageCaption caption && Id == caption.Id && Str == caption.Str)
			{
				return Context == caption.Context;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Str, Context);
		}
	}
}
