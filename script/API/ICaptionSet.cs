namespace LacieEngine.API
{
	public interface ICaptionSet
	{
		string this[string i] => Get(i);

		bool ContainsKey(string id);

		string Get(string id);

		string Get(string id, string context);
	}
}
