using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MahjongLineClient
{
    public class RequestManager
    {
        /// <summary>
        /// Event triggered when the tiles count in the wall changes.
        /// </summary>
        public event EventHandler NotifyWallCount;
        /// <summary>
        /// Event triggered when the game instance is refreshed from the server.
        /// </summary>
        public event EventHandler NotifyGameRefresh;

        private readonly HttpClient _client;

        // shortcut to avoid giving the ID for each request.
        private Guid _gameId;

        public RequestManager()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Properties.Settings.Default.ServerUrl)
            };
        }

        #region Idempotent

        public List<PlayerScorePivot> ComputeCurrentRanking(Guid gameId)
        {
            return SendQuery<List<PlayerScorePivot>>(HttpMethod.Get, $"games/{gameId.ToString()}/rankings");
        }

        public WindPivot GetPlayerCurrentWind(int playerIndex)
        {
            return SendQuery<WindPivot>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/winds");
        }

        public bool CanCallTsumo(int playerIndex, bool isKanCompensation)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/tsumo?isKanCompensation={(isKanCompensation ? 1 : 0)}");
        }

        public bool HumanCanAutoDiscard(int playerIndex)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/auto-discard");
        }

        public Dictionary<TilePivot, bool> CanCallChii(int playerIndex)
        {
            var datas = SendQuery<List<KeyValuePair<TilePivot, bool>>>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/chii");

            return datas.ToDictionary(d => d.Key, d => d.Value);
        }

        public List<TilePivot> CanCallKan(int playerIndex)
        {
            return SendQuery<List<TilePivot>>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/kan");
        }

        public bool CanCallPonOrKan(int playerIndex)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/pon-or-kan");
        }

        public List<TilePivot> CanCallRiichi(int playerIndex)
        {
            return SendQuery<List<TilePivot>>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/riichi");
        }

        public bool CanDiscard(int playerIndex, TilePivot tile)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/discard?tileId={tile.Id}");
        }

        public bool CanCallPon(int playerIndex)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/pon");
        }

        public bool CanCallRon(int playerIndex)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/players/{playerIndex}/check-calls/ron");
        }

        #region IA decisions

        public Tuple<int, TilePivot> KanDecision(bool checkConcealedOnly)
        {
            return SendQuery<Tuple<int, TilePivot>>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/kan?checkConcealedOnly={(checkConcealedOnly ? 1 : 0)}");
        }

        public List<int> RonDecision(bool ronCalled)
        {
            return SendQuery<List<int>>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/ron?ronCalled={(ronCalled ? 1 : 0)}");
        }

        public Tuple<TilePivot, bool> ChiiDecision()
        {
            return SendQuery<Tuple<TilePivot, bool>>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/chii");
        }

        public bool TsumoDecision(bool isKanCompensation)
        {
            return SendQuery<bool>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/tsumo?isKanCompensation={(isKanCompensation ? 1 : 0)}");
        }

        public int PonDecision()
        {
            return SendQuery<int>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/pon");
        }

        public TilePivot RiichiDecision()
        {
            return SendQuery<TilePivot>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/riichi");
        }

        public TilePivot DiscardDecision()
        {
            return SendQuery<TilePivot>(HttpMethod.Get, $"games/{_gameId.ToString()}/cpu-check-calls/discard");
        }

        #endregion IA decisions

        #endregion Idempotent

        #region Not idempotent

        public GamePivot CreateGame(string playerName, InitialPointsRulePivot pointRule, EndOfGameRulePivot endOfGameRule, bool useRedDoras, bool useNagashiMangan, bool useRenhou)
        {
            var game = SendQuery<GamePivot>(HttpMethod.Post, "games", new
            {
                InitialPointsRule = pointRule,
                EndOfGameRule = endOfGameRule,
                WithRedDoras = useRedDoras,
                UseNagashiMangan = useNagashiMangan,
                UseRenhou = useRenhou
            });

            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = playerName, IsCpu = false });
            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "CPU_1", IsCpu = true });
            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "CPU_2", IsCpu = true });
            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "CPU_3", IsCpu = true });

            _gameId = game.Id;
            return game;
        }

        public EndOfRoundInformationsPivot NextRound(int? ronPlayerId)
        {
            EndOfRoundInformationsPivot result = SendQuery<EndOfRoundInformationsPivot>(Patch(), $"games/{_gameId.ToString()}/rounds?ronPlayerId={ronPlayerId}");
            RefreshGame();
            return result;
        }

        public void UndoPickCompensationTile()
        {
            SendQuery<bool>(Patch(), $"games/{_gameId.ToString()}/compensation-pick-undoing");
            RefreshGame();
        }

        public TilePivot Pick(int playerIndex)
        {
            TilePivot result = SendQuery<TilePivot>(Patch(), $"games/{_gameId.ToString()}/players/{playerIndex}/calls/pick");
            RefreshGame();
            NotifyWallCount?.Invoke(result, null);
            return result;
        }

        public bool CallRiichi(int playerIndex, TilePivot tile)
        {
            bool result = SendQuery<bool>(Patch(), $"games/{_gameId.ToString()}/players/{playerIndex}/calls/riichi?tileId={tile.Id}");
            RefreshGame();
            return result;
        }

        public bool CallChii(int playerIndex, int startNumber)
        {
            bool result = SendQuery<bool>(Patch(), $"games/{_gameId.ToString()}/players/{playerIndex}/calls/chii?startNumber={startNumber}");
            RefreshGame();
            return result;
        }

        public TilePivot CallKan(int playerIndex, TilePivot tile = null)
        {
            TilePivot result = SendQuery<TilePivot>(Patch(), $"games/{_gameId.ToString()}/players/{playerIndex}/calls/kan?tileId={tile?.Id}");
            RefreshGame();
            NotifyWallCount?.Invoke(result, null);
            return result;
        }

        public bool CallPon(int playerIndex)
        {
            bool result = SendQuery<bool>(Patch(), $"games/{_gameId.ToString()}/players/{playerIndex}/calls/pon");
            RefreshGame();
            return result;
        }

        public bool Discard(int playerIndex, TilePivot tile)
        {
            bool result = SendQuery<bool>(Patch(), $"games/{_gameId.ToString()}/players/{playerIndex}/calls/discard?tileId={tile.Id}");
            RefreshGame();
            return result;
        }

        #endregion Not idempotent

        private HttpMethod Patch()
        {
            return new HttpMethod("PATCH");
        }

        private void RefreshGame()
        {
            var gameRefreshed = SendQuery<GamePivot>(HttpMethod.Get, $"games/{_gameId.ToString()}");
            NotifyGameRefresh?.Invoke(gameRefreshed, null);
        }

        private T SendQuery<T>(HttpMethod method, string route, object content = null)
        {
            try
            {
                var response = _client.SendAsync(new HttpRequestMessage
                {
                    Content = content == null ? null :
                    new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json"),
                    Method = method,
                    RequestUri = new Uri(route, UriKind.Relative)
                }).GetAwaiter().GetResult();

                response.EnsureSuccessStatusCode();

                string jsonContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonContent);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
