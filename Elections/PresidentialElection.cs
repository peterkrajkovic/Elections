using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Elections
{
    class PresidentialElection : Election
    {
        public Dictionary<string, int> Second { get; init; } = new Dictionary<string, int>();
        public PresidentialElection(DateTime date) : base(date)
        {
            Type = "presidential";
        }

        public PresidentialElection(DateTime date, string[] parties, int[] votes, string[] parties2, int[] votes2) : base(date, parties, votes)
        {
            if (parties2.Length == 2 && parties2.Length == votes2.Length)
            {
                for (int i = 0; i < parties2.Length; i++)
                {
                    Second.Add(parties2[i], votes2[i]);
                }
            }
            Type = "presidential";
        }

        public override string[] About()
        {
            var sortedResults = from var in Second orderby var.Value descending select var;
            List<string> r = new List<string>();
            r.Add("Results from parliamentary election " + Date.ToString("dd.MM.yyyy"));
            int count = Second.Values.Sum();
            foreach (var pair in sortedResults)
            {
                r.Add(pair.Key + " : " + Math.Round(((double)pair.Value * 100 / count), 2) + "%, " + pair.Value + " votes");
            }
            return r.ToArray();
        }
        
        public string[] FirstRound()
        {
            var sortedResults = from var in Results orderby var.Value descending select var;
            List<string> r = new List<string>(); 
            r.Add("Results from presidential election " + Date.ToString("dd.MM.yyyy") + "\nFirst round");
            int count = Results.Values.Sum();
            foreach (var pair in sortedResults)
            {
                r.Add(pair.Key + " : " + Math.Round(((double)pair.Value * 100 / count), 2) + "% , " + pair.Value + " votes");
            }
            return r.ToArray();
        }
       

        public override string ToString()
        {
            return "Presidential election " + Date.ToString("dd.MM.yyyy") + "\n";
        }
        public void Update()
        {
            if (Results.Count > 1)
            {
                int first = 0;
                string firstKey = String.Empty;
                string secondKey = String.Empty;
                int second = 0;
                foreach (var item in Results)
                {
                    if (item.Value > first)
                    {
                        second = first;
                        first = item.Value;
                        secondKey = firstKey;
                        firstKey = item.Key;
                    }
                    else if (item.Value > second)
                    {
                        second = item.Value;
                        secondKey = item.Key;
                    }
                }
                if (!(Second.ContainsKey(firstKey) && Second.ContainsKey(secondKey)))
                {
                    UpdateSecond dialog = new UpdateSecond(0, 0, firstKey, secondKey);
                    dialog.ShowDialog();
                    Second.Clear();
                    Second.Add(firstKey, dialog.Votes1);
                    Second.Add(secondKey, dialog.Votes2);
                }
            }
        }
    }
}
