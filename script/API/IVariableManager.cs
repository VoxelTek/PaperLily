namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IVariableManager : IModule
	{
		string GetVariable(string variableName);

		bool GetFlag(string flagName);

		int GetIntVariable(string variableName);

		void SetVariable(string variableName, string value);

		void SetFlag(string variableName, bool value);

		void ToggleFlag(string variableName);

		void SetInitialValue(string variableName, string initialValue);

		bool IsValid(string variableId);

		void Validate(string variableId);
	}
}
