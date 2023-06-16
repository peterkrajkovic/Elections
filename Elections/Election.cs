using System;
using System.Collections.Generic;
using System.Linq;

namespace Elections
{
    abstract class Election
    {
        public DateTime Date { get; init; }
        public Dictionary<string, int> Results { get; init; }
        public string Type { get; init; } = string.Empty;
        public Election(DateTime date)
        {
            Date = date;
            Results = new Dictionary<string, int>();    
        }
        public Election(DateTime date, string[] parties, int[] votes)
        {
            Date = date;
            Results = new Dictionary<string, int>();
            if (parties.Length != 0 && parties.Length == votes.Length)
            {
                for (int i = 0; i < parties.Length; i++)
                {
                    Results.Add(parties[i], votes[i]);
                }
            }
        }
        public string[] Parties()
        {
            return Results.Keys.ToArray();
        }
        public bool AddVotes(int votes, string party)
        {
            if (Results.ContainsKey(party))
            {
                Results[party] += votes;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SubVotes(int votes, string party)
        {
            if (Results.ContainsKey(party))
            {
                if ((Results[party] - votes) < 0)
                {
                    return false;
                }
                else
                {
                    Results[party] -= votes;
                    return true;
                }
            } 
            else
            {
                return false;
            }
        }
        public abstract string[] About();
        public override abstract string ToString();
    }
}
