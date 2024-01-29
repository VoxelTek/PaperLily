using System.Collections.Generic;
using LacieEngine.Translations;

namespace LacieEngine.API
{
	[InjectableInterface]
	public interface ITranslatable
	{
		void ApplyTranslationOverrides();

		List<TranslatableMessages> GetTranslatableMessages()
		{
			return null;
		}
	}
}
