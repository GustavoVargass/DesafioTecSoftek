using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        string apiUrlTeam1 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}";
        string apiUrlTeam2 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}";

        using (HttpClient client = new HttpClient())
        {
            int totalGoals = 0;
            int pageTeam1 = 1;
            int pageTeam2 = 1;
            int totalPagesTeam1 = 1;
            int totalPagesTeam2 = 1;

            while (pageTeam1 <= totalPagesTeam1)
            {
                string url = $"{apiUrlTeam1}&page={pageTeam1}";

                HttpResponseMessage response = client.GetAsync(url).Result;
                string json = response.Content.ReadAsStringAsync().Result;

                dynamic data = JsonConvert.DeserializeObject(json);

                totalPagesTeam1 = data.total_pages;

                foreach (dynamic matchData in data.data)
                {
                    if (matchData.team1 == team)
                    {
                        totalGoals += (int)matchData.team1goals;
                    }
                }

                pageTeam1++;
            }

            while (pageTeam2 <= totalPagesTeam2)
            {
                string url = $"{apiUrlTeam2}&page={pageTeam2}";

                HttpResponseMessage response = client.GetAsync(url).Result;
                string json = response.Content.ReadAsStringAsync().Result;

                dynamic data = JsonConvert.DeserializeObject(json);

                totalPagesTeam2 = data.total_pages;

                foreach (dynamic matchData in data.data)
                {
                    if (matchData.team2 == team)
                    {
                        totalGoals += (int)matchData.team2goals;
                    }
                }
                pageTeam2++;
            }

            return totalGoals;
        }

    }
}