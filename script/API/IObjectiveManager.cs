using System.Collections.Generic;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IObjectiveManager : IModule
	{
		void Add(string objectiveId);

		void Remove(string objectiveId);

		void Complete(string objectiveId);

		void Fail(string objectiveId);

		List<IObjective> GetCurrentObjectives();

		List<IObjective> GetAllObjectives();

		void ClearObjectives();

		void SilenceNotifications();

		void ShowNotification();

		bool IsObjectiveInProgress(string objectiveId);

		bool IsObjectiveCompleted(string objectiveId);

		bool IsObjectiveFailed(string objectiveId);

		bool IsObjectiveValid(string objectiveId);

		bool ObjectiveHasPendingChildren(IObjective objective);
	}
}
