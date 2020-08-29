using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MahjongLineClient
{
    public class RequestManager
    {
        /// <summary>
        /// Event triggered when the tiles count in the wall changes.
        /// </summary>
        public event EventHandler NotifyWallCount;

        private readonly HttpClient _client;

        public RequestManager()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Properties.Settings.Default.ServerUrl)
            };
        }

        #region Idempotent

        public List<PlayerScorePivot> ComputeCurrentRanking(GamePivot game)
        {
            throw new NotImplementedException();
        }

        public WindPivot GetPlayerCurrentWind(int pIndex)
        {
            throw new NotImplementedException();
        }

        public List<TilePivot> GetDiscard(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsRiichiRank(int playerIndex, int rank)
        {
            throw new NotImplementedException();
        }

        public bool CanCallTsumo(bool isKanCompensation)
        {
            throw new NotImplementedException();
        }

        public HandPivot GetHand(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool HumanCanAutoDiscard()
        {
            throw new NotImplementedException();
        }

        public Dictionary<TilePivot, bool> CanCallChii()
        {
            throw new NotImplementedException();
        }

        public List<TilePivot> CanCallKan(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool CanCallPonOrKan(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public List<TilePivot> CanCallRiichi()
        {
            throw new NotImplementedException();
        }

        public bool CanDiscard(TilePivot tile)
        {
            throw new NotImplementedException();
        }

        public bool CanCallPon(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool CanCallRon(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsRiichi(int playerIndex)
        {
            throw new NotImplementedException();
        }

        #region IA decisions

        public Tuple<int, TilePivot> KanDecision(bool checkConcealedOnly)
        {
            throw new NotImplementedException();
        }

        public List<int> RonDecision(bool ronCalled)
        {
            throw new NotImplementedException();
        }

        public Tuple<TilePivot, bool> ChiiDecision()
        {
            throw new NotImplementedException();
        }

        public bool TsumoDecision(bool isKanCompensation)
        {
            throw new NotImplementedException();
        }

        public int PonDecision()
        {
            throw new NotImplementedException();
        }

        public TilePivot RiichiDecision()
        {
            throw new NotImplementedException();
        }

        public TilePivot DiscardDecision()
        {
            throw new NotImplementedException();
        }

        #endregion IA decisions

        #endregion Idempotent

        #region Not idempotent

        public GamePivot CreateGame(InitialPointsRulePivot pointRule, EndOfGameRulePivot endOfGameRule, bool useRedDoras, bool useNagashiMangan, bool useRenhou)
        {
            var game = SendQuery<GamePivot>(HttpMethod.Post, "games", new
            {
                InitialPointsRule = pointRule,
                EndOfGameRule = endOfGameRule,
                WithRedDoras = useRedDoras,
                UseNagashiMangan = useNagashiMangan,
                UseRenhou = useRenhou
            });

            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "HUMAN", IsCpu = false });
            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "CPU_1", IsCpu = true });
            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "CPU_2", IsCpu = true });
            game = SendQuery<GamePivot>(HttpMethod.Post, $"games/{game.Id.ToString()}/players", new { PlayerName = "CPU_3", IsCpu = true });

            return game;
        }

        public EndOfRoundInformationsPivot NextRound(int? ronPlayerId)
        {
            throw new NotImplementedException();
        }

        public TilePivot Pick()
        {
            NotifyWallCount?.Invoke(null, null);
            throw new NotImplementedException();
        }

        public bool CallRiichi(TilePivot tile)
        {
            throw new NotImplementedException();
        }

        public bool CallChii(int startNumber)
        {
            throw new NotImplementedException();
        }

        public TilePivot CallKan(int playerIndex, TilePivot tileChoice = null)
        {
            NotifyWallCount?.Invoke(null, null);
            throw new NotImplementedException();
        }

        public void UndoPickCompensationTile()
        {
            throw new NotImplementedException();
        }

        public bool CallPon(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool Discard(TilePivot tile)
        {
            throw new NotImplementedException();
        }

        #endregion Not idempotent

        private T SendQuery<T>(HttpMethod method, string route, object content = null)
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
    }
}
