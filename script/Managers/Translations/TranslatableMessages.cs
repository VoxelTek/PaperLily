using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LacieEngine.Core;

namespace LacieEngine.Translations
{
	public class TranslatableMessages : IEnumerable
	{
		private List<LanguageCaption> _entries = new List<LanguageCaption>();

		public string Name { get; set; }

		public int Count => _entries.Count;

		public bool IsEmpty()
		{
			return _entries.Count == 0;
		}

		public void Add(string msgid)
		{
			Add(new LanguageCaption(msgid));
		}

		public void Add(string msgid, string msgctxt)
		{
			Add(new LanguageCaption(msgid, string.Empty, msgctxt));
		}

		public void Add(LanguageCaption caption)
		{
			_entries.Add(caption);
		}

		public string Get(string msgid, string msgctxt = null)
		{
			if (msgctxt.IsNullOrEmpty())
			{
				foreach (LanguageCaption caption2 in _entries)
				{
					if (msgid == caption2.Id && msgctxt == caption2.Context)
					{
						return caption2.Str;
					}
				}
			}
			foreach (LanguageCaption caption in _entries)
			{
				if (msgid == caption.Id)
				{
					return caption.Str;
				}
			}
			return null;
		}

		public TranslatableMessages()
		{
		}

		public TranslatableMessages(string name)
		{
			Name = name;
		}

		public void Clean()
		{
			for (int j = 0; j < _entries.Count; j++)
			{
				if (!MsgIdAlreadyExists(_entries[j].Id, j) && !_entries[j].Context.IsNullOrEmpty())
				{
					_entries[j] = new LanguageCaption(_entries[j].Id, _entries[j].Str);
				}
			}
			for (int i = 0; i < _entries.Count; i++)
			{
				if (EntryDuplicateExists(_entries[i], i))
				{
					Log.Warn("Duplicate entry: ", _entries[i].Id, " in ", Name);
				}
			}
			int count = _entries.Count;
			_entries = _entries.Distinct().ToList();
			if (count > _entries.Count)
			{
				Log.Warn("Duplicate captions were consumed: ", count - _entries.Count, " in ", Name);
			}
			bool EntryDuplicateExists(LanguageCaption entry, int msgidx)
			{
				for (int k = 0; k < _entries.Count; k++)
				{
					if (k != msgidx && _entries[k].Equals(entry))
					{
						return true;
					}
				}
				return false;
			}
			bool MsgIdAlreadyExists(string msgid, int msgidx)
			{
				for (int l = 0; l < _entries.Count; l++)
				{
					if (l != msgidx && _entries[l].Id == msgid)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void RemoveContext()
		{
			for (int i = 0; i < _entries.Count; i++)
			{
				if (!_entries[i].Context.IsNullOrEmpty())
				{
					_entries[i] = new LanguageCaption(_entries[i].Id, _entries[i].Str);
				}
			}
			_entries = _entries.Distinct().ToList();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _entries.GetEnumerator();
		}
	}
}
