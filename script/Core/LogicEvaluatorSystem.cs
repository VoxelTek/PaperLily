using System;

namespace LacieEngine.Core
{
    public static class LogicEvaluatorSystem
    {
        public static bool Evaluate(string flag)
        {
            if (flag == "show_skip_option")
            {
                return Game.Settings.ShowSkipOnDeath;
            }
            Log.Error(new object[] { "Invalid system flag: ", flag });
            return false;
        }

        public const string SKIP_OPTION_ENABLED = "show_skip_option";
    }
}