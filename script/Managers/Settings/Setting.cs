using Godot;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	public abstract class Setting<T> : SettingParent
	{
		public string Name { get; protected set; }

		public string Path { get; protected set; }

		public T Value { get; set; }

		public bool OwnSound { get; protected set; }

		public virtual T ReadValue()
		{
			return GDUtil.ReadSetting<T>(Path);
		}

		public override void WriteValue(ConfigFile configFile)
		{
			if (!ReadValue().Equals(Value))
			{
				ProjectSettings.SetSetting(Path, Value);
				string section = Path.Substring(0, Path.IndexOf("/"));
				string path = Path;
				int num = Path.IndexOf("/") + 1;
				string key = path.Substring(num, path.Length - num);
				configFile.SetValue(section, key, Value);
			}
		}

		public virtual string CaptionLabel()
		{
			return Name;
		}

		public virtual string ValueLabel()
		{
			return Value.ToString();
		}

		public abstract void Increment();

		public abstract void Decrement();
	}
}
