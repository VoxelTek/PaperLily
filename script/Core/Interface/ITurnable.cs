namespace LacieEngine.Core
{
	public interface ITurnable
	{
		bool TurningEnabled { get; }

		void Turn(Direction direction);

		void TurnToDefault();
	}
}
