﻿using Steam.Models.SteamCommunity;
using SteamWebAPI2.Models.SteamCommunity;
using SteamWebAPI2.Models.SteamPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SteamWebAPI2.Utilities;

namespace SteamWebAPI2.Interfaces
{
    public class PlayerService : SteamWebInterface, IPlayerService
    {
        public PlayerService(string steamWebApiKey)
            : base(steamWebApiKey, "IPlayerService")
        {
        }

        public async Task<string> IsPlayingSharedGameAsync(long steamId, int appId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            parameters.AddIfHasValue(steamId, "steamid");
            parameters.AddIfHasValue(appId, "appid_playing");
            var playingSharedGameResult = await CallMethodAsync<PlayingSharedGameResultContainer>("IsPlayingSharedGame", 1, parameters);
            return playingSharedGameResult.Result.LenderSteamId;
        }

        public async Task<IReadOnlyCollection<BadgeQuestModel>> GetCommunityBadgeProgressAsync(long steamId, int? badgeId = null)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            parameters.AddIfHasValue(steamId, "steamid");
            parameters.AddIfHasValue(badgeId, "badgeid");
            var badgeProgressResult = await CallMethodAsync<CommunityBadgeProgressResultContainer>("GetCommunityBadgeProgress", 1, parameters);
            var badgeProgressModels = AutoMapperConfiguration.Mapper.Map<IList<BadgeQuest>, IList<BadgeQuestModel>>(badgeProgressResult.Result.Quests);
            return new ReadOnlyCollection<BadgeQuestModel>(badgeProgressModels);
        }

        public async Task<BadgesResultModel> GetBadgesAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            parameters.AddIfHasValue(steamId, "steamid");
            var badgesResult = await CallMethodAsync<BadgesResultContainer>("GetBadges", 1, parameters);
            var badgesResultModel = AutoMapperConfiguration.Mapper.Map<BadgesResult, BadgesResultModel>(badgesResult.Result);
            return badgesResultModel;
        }

        public async Task<int> GetSteamLevelAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            parameters.AddIfHasValue(steamId, "steamid");
            var steamLevelResult = await CallMethodAsync<SteamLevelResultContainer>("GetSteamLevel", 1, parameters);
            return steamLevelResult.Result.PlayerLevel;
        }

        public async Task<OwnedGamesResultModel> GetOwnedGamesAsync(long steamId, bool? includeAppInfo = null, bool? includeFreeGames = null, IReadOnlyCollection<int> appIdsToFilter = null)
        {
            int? includeAppInfoBit = 0;
            if (includeAppInfo.HasValue) { includeAppInfoBit = includeAppInfo.Value ? 1 : 0; }

            int? includeFreeGamesBit = 0;
            if (includeFreeGames.HasValue) { includeFreeGamesBit = includeFreeGames.Value ? 1 : 0; }

            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            parameters.AddIfHasValue(includeFreeGamesBit, "include_played_Free_games");
            parameters.AddIfHasValue(steamId, "steamid");
            parameters.AddIfHasValue(includeAppInfoBit, "include_appinfo");

            if (appIdsToFilter != null)
            {
                string appIdsDelimited = String.Join(",", appIdsToFilter);
                parameters.AddIfHasValue(appIdsDelimited, "appids_filter");
            }

            var ownedGamesResult = await CallMethodAsync<OwnedGamesResultContainer>("GetOwnedGames", 1, parameters);

            // for some reason, some games have trailing spaces in the result
            if (ownedGamesResult.Result != null && ownedGamesResult.Result.OwnedGames != null)
            {
                foreach (var ownedGame in ownedGamesResult.Result.OwnedGames)
                {
                    if (!String.IsNullOrEmpty(ownedGame.Name))
                    {
                        ownedGame.Name = ownedGame.Name.Trim();
                    }
                }
            }

            var ownedGamesResultModel = AutoMapperConfiguration.Mapper.Map<OwnedGamesResult, OwnedGamesResultModel>(ownedGamesResult.Result);
            
            return ownedGamesResultModel;
        }

        public async Task<RecentlyPlayedGamesResultModel> GetRecentlyPlayedGamesAsync(long steamId)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
            parameters.AddIfHasValue(steamId, "steamid");
            var recentlyPlayedGamesResult = await CallMethodAsync<RecentlyPlayedGameResultContainer>("GetRecentlyPlayedGames", 1, parameters);

            var recentlyPlayedGamesResultModel = AutoMapperConfiguration.Mapper.Map<RecentlyPlayedGameResult, RecentlyPlayedGamesResultModel>(recentlyPlayedGamesResult.Result);

            return recentlyPlayedGamesResultModel;
        }
    }
}