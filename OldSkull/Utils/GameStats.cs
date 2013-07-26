using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldSkull.Utils
{
    public class GameStats
    {
        static public GameStats Instance { get; private set; }

        private List<string> Triggers;

        public static GameStats Init()
        {
            GameStats instance = new GameStats();
            return instance;
        }

        public GameStats()
        {
            Instance = this;
            Triggers = new List<string>();
        }

        public void SetTrigger(string trigger)
        {
            if (!HasTrigger(trigger))
            {
                if (Triggers == null) Triggers = new List<string>();
                Triggers.Add(trigger);
            }
        }

        public bool HasTrigger(string trigger)
        {
            if (Triggers == null) return false;
            foreach (string t in Triggers)
            {
                if (t == trigger) return true;
            }

            return false;
        }

    }
}
