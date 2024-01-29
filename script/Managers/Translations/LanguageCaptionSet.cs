using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Translations
{
	public class LanguageCaptionSet : ICaptionSet
	{
		private class LanguageCaptionEntry
		{
			public string MessageStr { get; private set; }

			public Dictionary<string, string> Contexts { get; private set; }

			public LanguageCaptionEntry(string str)
			{
				MessageStr = str;
			}

			public void AddContext(string context, string str)
			{
				if (Contexts == null)
				{
					Dictionary<string, string> dictionary2 = (Contexts = new Dictionary<string, string>());
				}
				Contexts[context] = str;
			}

			public void UpdateMessage(string message)
			{
				MessageStr = message;
			}

			public bool HasContext(string context)
			{
				if (Contexts != null)
				{
					return Contexts.ContainsKey(context);
				}
				return false;
			}
		}

		private Dictionary<string, LanguageCaptionEntry> _entries = new Dictionary<string, LanguageCaptionEntry>();

		public bool ContainsKey(string id)
		{
			return _entries.ContainsKey(id);
		}

		public string Get(string id)
		{
			if (id.IsNullOrEmpty() || !_entries.ContainsKey(id))
			{
				return id;
			}
			return _entries[id].MessageStr;
		}

		public string Get(string id, string context)
		{
			if (context.IsNullOrEmpty())
			{
				return Get(id);
			}
			if (id.IsNullOrEmpty() || !_entries.ContainsKey(id))
			{
				return id;
			}
			if (!_entries[id].HasContext(context))
			{
				return _entries[id].MessageStr;
			}
			return _entries[id].Contexts[context];
		}

		public void AddCaption(LanguageCaption caption)
		{
			if (!_entries.ContainsKey(caption.Id))
			{
				_entries[caption.Id] = new LanguageCaptionEntry(caption.Str);
			}
			else if (caption.Context.IsNullOrEmpty())
			{
				_entries[caption.Id].UpdateMessage(caption.Str);
			}
			if (!caption.Context.IsNullOrEmpty())
			{
				_entries[caption.Id].AddContext(caption.Context, caption.Str);
			}
		}
	}
}
