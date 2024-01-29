using LacieEngine.Core;

namespace LacieEngine.API
{
	public interface IActionObject
	{
		void Activate();

		void Deactivate();

		bool IsValidForDirection(Direction direction)
		{
			return true;
		}
	}
}
