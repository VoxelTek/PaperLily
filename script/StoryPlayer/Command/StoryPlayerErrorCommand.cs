namespace LacieEngine.StoryPlayer
{
	public class StoryPlayerErrorCommand : StoryPlayerNoopCommand
	{
		public string Message { get; set; }

		public StoryPlayerErrorCommand(string message)
		{
			Message = message;
		}

		public StoryPlayerErrorCommand(StoryPlayerCommand block, string message)
		{
			base.IndentLv = block.IndentLv;
			base.RawCommand = block.RawCommand;
			base.Index = block.Index;
			base.Event = block.Event;
			Message = message;
		}
	}
}
