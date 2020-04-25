namespace LeetCode.Csharp.Solutions2
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Search
    {
        // 332 - https://leetcode.com/problems/reconstruct-itinerary/
        public IList<string> FindItinerary(IList<IList<string>> tickets)
        {
            for (int i = 0; i < tickets.Count; i++)
            {
                tickets[i].Add(i.ToString());
            }

            Dictionary<string, List<IList<string>>> nodes = tickets
                .GroupBy(t => t[0])
                .ToDictionary(g => g.Key, g => g.OrderBy(t => t[1]).ToList());

            HashSet<string> visited = new HashSet<string>();

            List<string> route = new List<string>();
            this.FindItinerarySearch(nodes, visited, tickets.Count, "JFK", route);
            route.Reverse();
            return route;
        }

        private bool FindItinerarySearch(
            Dictionary<string, List<IList<string>>> nodes,
            HashSet<string> visited,
            int edgeToVisit,
            string currNode,
            List<string> route)
        {
            if (visited.Count == edgeToVisit)
            {
                // Have visited all edges.
                route.Add(currNode);
                return true;
            }

            foreach (IList<string> ticket in nodes[currNode])
            {
                string target = ticket[1];
                string tktKey = ticket[2];
                if (!visited.Contains(tktKey))
                {
                    visited.Add(tktKey);
                    
                    if (this.FindItinerarySearch(nodes, visited, edgeToVisit, target, route))
                    {
                        route.Add(currNode);
                        return true;
                    }

                    visited.Remove(tktKey);
                }
            }

            return false;
        }
    }
}