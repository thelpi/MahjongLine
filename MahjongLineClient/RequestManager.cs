using System;
using System.Collections.Generic;

namespace MahjongLineClient
{
    public class RequestManager
    {
        /// <summary>
        /// Event triggered when the tiles count in the wall changes.
        /// </summary>
        public event EventHandler NotifyWallCount;

        public RequestManager()
        {
            // something to do
        }

        public List<PlayerScorePivot> ComputeCurrentRanking(GamePivot game)
        {
            throw new NotImplementedException();
        }

        public GamePivot CreateGame(InitialPointsRulePivot pointRule, EndOfGameRulePivot endOfGameRule, bool useRedDoras, bool useNagashiMangan, bool useRenhou)
        {
            throw new NotImplementedException();
        }

        public WindPivot GetPlayerCurrentWind(int pIndex)
        {
            throw new NotImplementedException();
        }

        public EndOfRoundInformationsPivot NextRound(int? ronPlayerId)
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

        public bool CallChii(int startNumber)
        {
            throw new NotImplementedException();
        }

        public TilePivot CallKan(int playerIndex, TilePivot tileChoice = null)
        {
            // do something about the wall
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

        public bool Discard(TilePivot tile)
        {
            throw new NotImplementedException();
        }

        public bool HumanCanAutoDiscard()
        {
            throw new NotImplementedException();
        }

        public bool CanDiscard(TilePivot tile)
        {
            throw new NotImplementedException();
        }

        public TilePivot Pick()
        {
            // so something about the wall
            throw new NotImplementedException();
        }

        public bool CallRiichi(TilePivot tile)
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
    }
}
