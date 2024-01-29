using System.Threading.Tasks;
using Godot;

namespace LacieEngine.Minigames
{
	public interface IMinigame
	{
		Node Node => (Node)this;

		bool CustomTransition => false;

		void Init();

		void Start()
		{
		}

		void AddUiElements(Control parent)
		{
		}

		Task TransitionIn()
		{
			return null;
		}

		Task TransitionOut()
		{
			return null;
		}
	}
}
