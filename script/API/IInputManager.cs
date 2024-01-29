using System;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IInputManager : IModule
	{
		event Action InputTypeChanged;
	}
}
