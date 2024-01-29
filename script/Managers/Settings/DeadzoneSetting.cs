using System;

namespace LacieEngine.Settings
{
	internal class DeadzoneSetting : Setting<float>
	{
		private static readonly Lazy<DeadzoneSetting> _lazyInstance = new Lazy<DeadzoneSetting>(() => new DeadzoneSetting());

		private const decimal Max = 0.95m;

		private const decimal Min = 0m;

		private const decimal Step = 0.05m;

		private decimal _value;

		public static DeadzoneSetting Instance => _lazyInstance.Value;

		public DeadzoneSetting()
		{
			base.Name = "system.settings.input.deadzone";
			base.Path = "lacie_engine/input/joystick_deadzone";
			base.Value = ReadValue();
			_value = (decimal)base.Value;
		}

		public override string ValueLabel()
		{
			return (int)(base.Value * 100f) + "%";
		}

		public override void Decrement()
		{
			_value -= 0.05m;
			if (_value < 0m)
			{
				_value = default(decimal);
			}
			base.Value = (float)_value;
		}

		public override void Increment()
		{
			_value += 0.05m;
			if (_value > 0.95m)
			{
				_value = 0.95m;
			}
			base.Value = (float)_value;
		}

		public override void Apply()
		{
		}
	}
}
