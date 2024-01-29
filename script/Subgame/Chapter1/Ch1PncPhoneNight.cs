using System.Collections.Generic;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1PncPhoneNight : Ch1PncPhone
	{
		static Ch1PncPhoneNight()
		{
			Ch1PncPhone.CorrectCombinations = new Dictionary<string, string>();
			Ch1PncPhone.CorrectCombinations.Add("195224#1", "ch1_event_phone_mother_night");
			Ch1PncPhone.CorrectCombinations.Add("999281#0", "ch1_event_phone_ritual");
		}
	}
}
