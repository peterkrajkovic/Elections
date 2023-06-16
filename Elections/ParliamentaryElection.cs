using System;
using System.Collections.Generic;
using System.Linq;

namespace Elections
{
    class ParliamentaryElection : Election
    {
        
        public ParliamentaryElection(DateTime date) : base(date)
        {
            Type = "parliamentary";
        }

        public ParliamentaryElection(DateTime date, string[] parties, int[] votes) : base(date, parties, votes)
        {
            Type = "parliamentary";
        }


        public override string[] About()
        {
            var sortedResults = from var in Results orderby var.Value descending select var;
            List<string> r = new List<string>();
            r.Add("Results from parliamentary election " + Date.ToString("dd.MM.yyyy"));
            int count = Results.Values.Sum();
            foreach (var pair in sortedResults)
            {
                r.Add(pair.Key + " : " + Math.Round(((double)pair.Value * 100 / count), 2) + "% , " + pair.Value + " votes");
            }
            return r.ToArray();
        }

        public override string ToString()
        {
            return "Parliamentary election " + Date.ToString("dd.MM.yyyy") + "\n";
        }
    }
}
