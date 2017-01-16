﻿using Steam.Models.DOTA2;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamWebAPI2.Interfaces
{
    public interface IDOTA2Match
    {
        Task<IReadOnlyCollection<LeagueModel>> GetLeagueListingAsync(string language);

        Task<IReadOnlyCollection<LiveLeagueGameModel>> GetLiveLeagueGamesAsync(int? leagueId = default(int?), long? matchId = default(long?));

        Task<MatchDetailModel> GetMatchDetailsAsync(long matchId);

        Task<MatchHistoryModel> GetMatchHistoryAsync(int? heroId = default(int?), int? gameMode = default(int?), int? skill = default(int?), string minPlayers = "", string accountId = "", string leagueId = "", long? startAtMatchId = default(long?), string matchesRequested = "", string tournamentGamesOnly = "");

        Task<IReadOnlyCollection<MatchHistoryMatchModel>> GetMatchHistoryBySequenceNumberAsync(long? startAtMatchSequenceNumber = default(long?), int? matchesRequested = default(int?));

        Task<IReadOnlyCollection<TeamInfoModel>> GetTeamInfoByTeamIdAsync(long? startAtTeamId = default(long?), int? teamsRequested = default(int?));
    }
}