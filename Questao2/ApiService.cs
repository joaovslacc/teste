using Newtonsoft.Json;

namespace Questao2;

public class ApiService
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

    public ApiService()
    {
        _client = new HttpClient();
    }

    public async Task<int> GetTotalScoredGoalsAsync(string team, int year)
    {
        int totalGoals = 0;

        totalGoals += await GetGoalsByTeamAsync(team, year, "team1");
        totalGoals += await GetGoalsByTeamAsync(team, year, "team2");

        return totalGoals;
    }

    private async Task<int> GetGoalsByTeamAsync(string team, int year, string teamParam)
    {
        int totalGoals = 0;
        int currentPage = 1;
        bool morePages = true;

        while (morePages)
        {
            string url = $"{BaseUrl}?year={year}&{teamParam}={team}&page={currentPage}";

            HttpResponseMessage response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao acessar a API: {response.ReasonPhrase}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (result?.data != null)
            {
                foreach (var match in result.data)
                {
                    totalGoals += int.Parse(teamParam == "team1" ? match.team1goals : match.team2goals);
                }
            }

            morePages = currentPage < result!.total_pages;
            currentPage++;
        }

        return totalGoals;
    }
}

public class ApiResponse
{
    public Match[] data { get; set; }
    public int total_pages { get; set; }
}

public class Match
{
    public string team1goals { get; set; }
    public string team2goals { get; set; }
}
