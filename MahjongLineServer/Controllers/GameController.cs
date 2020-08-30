using System;
using System.Collections.Generic;
using System.Linq;
using MahjongLineServer.Controllers.Exceptions;
using MahjongLineServer.Controllers.Requests;
using MahjongLineServer.Pivot;
using Microsoft.AspNetCore.Mvc;

namespace MahjongLineServer.Controllers
{
    /// <summary>
    /// Game controller.
    /// </summary>
    /// <seealso cref="Controller"/>
    [Produces("application/json")]
    [Route("games")]
    public class GameController : Controller
    {
        private static readonly List<GamePivot> _games = new List<GamePivot>();

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <returns>Game creation request.</returns>
        [HttpPost]
        public ActionResult CreateNewGame([FromBody] CreateGameRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var g = new GamePivot(request.InitialPointsRule, request.EndOfGameRule,
                request.WithRedDoras, request.UseNagashiMangan, request.UseRenhou);

            _games.Add(g);

            return Ok(g);
        }

        /// <summary>
        /// Adds a player to the game.
        /// </summary>
        /// <param name="guid">The game GUID.</param>
        /// <param name="request">Player informations request.</param>
        /// <returns>Instance of game.</returns>
        [HttpPost("{guid}/players")]
        public ActionResult AddPlayer([FromRoute] Guid guid, [FromBody] PlayerRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            GamePivot game = CheckGame(guid);

            game.AddPlayer(request.PlayerName, request.IsCpu);

            return Ok(game);
        }

        /// <summary>
        /// Gets players rankings.
        /// </summary>
        /// <param name="guid">The game GUID.</param>
        /// <returns>Collection of <see cref="PlayerScorePivot"/>.</returns>
        [HttpGet("{guid}/rankings")]
        public ActionResult ComputeRankings([FromRoute] Guid guid)
        {
            return Ok(ScoreTools.ComputeCurrentRanking(CheckGame(guid)));
        }

        /// <summary>
        /// Gets current wind for specified player.
        /// </summary>
        /// <param name="guid">The game GUID.</param>
        /// <param name="playerIndex">The player index.</param>
        /// <returns>Current <see cref="WindPivot"/>.</returns>
        [HttpGet("{guid}/players/{playerIndex}/winds")]
        public ActionResult GetPlayerCurrentWind([FromRoute] Guid guid, [FromRoute] int playerIndex)
        {
            return Ok(CheckGame(guid).GetPlayerCurrentWind(CheckPlayerIndex(playerIndex)));
        }

        /// <summary>
        /// Gets a game by its identifier.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>Game instance.</returns>
        [HttpGet("{guid}")]
        public ActionResult GetGame([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid));
        }

        /// <summary>
        /// Proceeds to next round.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="ronPlayerId">Ron player index; if any.</param>
        /// <returns>Instance of <see cref="EndOfRoundInformationsPivot"/>.</returns>
        [HttpPatch("{guid}/rounds")]
        public ActionResult NextRound([FromRoute] Guid guid, [FromQuery] int? ronPlayerId)
        {
            return Ok(CheckGame(guid).NextRound(CheckPlayerIndex(ronPlayerId)));
        }

        /// <summary>
        /// Picks a tile.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>Picked tile.</returns>
        [HttpPatch("{guid}/calls/pick")]
        public ActionResult Pick([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.Pick());
        }

        /// <summary>
        /// Calls riichi while discarding the specified tile.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="tileIndexInPlayerHand">Tile index in player hand.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("{guid}/calls/riichi")]
        public ActionResult CallRiichi([FromRoute] Guid guid, [FromQuery] int tileIndexInPlayerHand)
        {
            RoundPivot round = CheckGame(guid).Round;
            TilePivot tile = round.GetTileFromIndex(tileIndexInPlayerHand);
            return Ok(round.CallRiichi(tile));
        }

        /// <summary>
        /// Proceeds to call chii.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="startNumber">Tile number of the first tile of the sequence.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("{guid}/calls/chii")]
        public ActionResult CallChii([FromRoute] Guid guid, [FromQuery] int startNumber)
        {
            return Ok(CheckGame(guid).Round.CallChii(startNumber));
        }

        /// <summary>
        /// Proceeds to call kan for the specified player.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="tileIndexInPlayerHand">Optionnal tile index.</param>
        /// <returns>The compensation tile.</returns>
        [HttpPatch("{guid}/players/{playerIndex}/calls/kan")]
        public ActionResult CallKan([FromRoute] Guid guid, [FromRoute] int playerIndex, [FromQuery] int? tileIndexInPlayerHand)
        {
            RoundPivot round = CheckGame(guid).Round;
            TilePivot tile = tileIndexInPlayerHand.HasValue ? round.GetTileFromIndex(tileIndexInPlayerHand.Value, playerIndex) : null;
            return Ok(round.CallKan(CheckPlayerIndex(playerIndex), tile));
        }

        /// <summary>
        /// Proceeds to undo a compensation pick.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>No content.</returns>
        [HttpPatch("{guid}/compensation-pick-undoing")]
        public ActionResult UndoPickCompensationTile([FromRoute] Guid guid)
        {
            CheckGame(guid).Round.UndoPickCompensationTile();
            return NoContent();
        }

        /// <summary>
        /// Proceeds to call pon for the specified player.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("{guid}/players/{playerIndex}/calls/pon")]
        public ActionResult CallPon([FromRoute] Guid guid, [FromRoute] int playerIndex)
        {
            return Ok(CheckGame(guid).Round.CallPon(CheckPlayerIndex(playerIndex)));
        }

        /// <summary>
        /// Proceeds to discard.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="tileIndexInPlayerHand">Tile index in the current player hand.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("{guid}/calls/discard")]
        public ActionResult Discard([FromRoute] Guid guid, [FromQuery] int tileIndexInPlayerHand)
        {
            RoundPivot round = CheckGame(guid).Round;
            TilePivot tile = round.GetTileFromIndex(tileIndexInPlayerHand);
            return Ok(round.Discard(tile));
        }

        /// <summary>
        /// Checks for tsumo call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="isKanCompensation"><c>1</c> if kan compensation; <c>0</c> otherwise.</param>
        /// <returns><c>True</c> if can tsumo; <c>False</c> otherwise.</returns>
        [HttpGet("{guid}/check-calls/tsumo")]
        public ActionResult CanCallTsumo([FromRoute] Guid guid, [FromQuery] byte isKanCompensation)
        {
            return Ok(CheckGame(guid).Round.CanCallTsumo(isKanCompensation > 0));
        }

        /// <summary>
        /// Checks for auto-discard call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns><c>True</c> if can auto-discard.</returns>
        [HttpGet("{guid}/check-calls/auto-discard")]
        public ActionResult HumanCanAutoDiscard([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.HumanCanAutoDiscard());
        }

        /// <summary>
        /// Checks for chii call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>List of possible chiis, if any.</returns>
        [HttpGet("{guid}/check-calls/chii")]
        public ActionResult CanCallChii([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.CanCallChii());
        }

        /// <summary>
        /// Checks for kan call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>List of possible kans, if any.</returns>
        [HttpGet("{guid}/players/{playerIndex}/check-calls/kan")]
        public ActionResult CanCallKan([FromRoute] Guid guid, [FromRoute] int playerIndex)
        {
            return Ok(CheckGame(guid).Round.CanCallKan(CheckPlayerIndex(playerIndex)));
        }

        /// <summary>
        /// Checks for pon or kan call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if kan or pon possible; <c>False</c> otherwise.</returns>
        [HttpGet("{guid}/players/{playerIndex}/check-calls/pon-or-kan")]
        public ActionResult CanCallPonOrKan([FromRoute] Guid guid, [FromRoute] int playerIndex)
        {
            return Ok(CheckGame(guid).Round.CanCallPonOrKan(CheckPlayerIndex(playerIndex)));
        }

        /// <summary>
        /// Checks for riichi call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>Chii informations.</returns>
        [HttpGet("{guid}/check-calls/riichi")]
        public ActionResult CanCallRiichi([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.CanCallRiichi());
        }

        /// <summary>
        /// Checks for discard call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="tileIndexInPlayerHand">Tiles index in player hand.</param>
        /// <returns><c>True</c> if tile can be discarded; <c>False</c> otherwise.</returns>
        [HttpGet("{guid}/check-calls/discard")]
        public ActionResult CanDiscard([FromRoute] Guid guid, [FromQuery] int tileIndexInPlayerHand)
        {
            RoundPivot round = CheckGame(guid).Round;
            TilePivot tile = round.GetTileFromIndex(tileIndexInPlayerHand);
            return Ok(round.CanDiscard(tile));
        }

        /// <summary>
        /// Checks for pon call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if pon possible; <c>False</c> otherwise.</returns>
        [HttpGet("{guid}/players/{playerIndex}/check-calls/pon")]
        public ActionResult CanCallPon([FromRoute] Guid guid, [FromRoute] int playerIndex)
        {
            return Ok(CheckGame(guid).Round.CanCallPon(CheckPlayerIndex(playerIndex)));
        }

        /// <summary>
        /// Checks for ron call.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if ron possible; <c>False</c> otherwise.</returns>
        [HttpGet("{guid}/players/{playerIndex}/check-calls/ron")]
        public ActionResult CanCallRon([FromRoute] Guid guid, [FromRoute] int playerIndex)
        {
            return Ok(CheckGame(guid).Round.CanCallRon(CheckPlayerIndex(playerIndex)));
        }

        /// <summary>
        /// Checks for CPU kan decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="checkConcealedOnly"><c>1</c> if only concealed should be checked; <c>0</c> otherwise.</param>
        /// <returns>Kan informations, if any.</returns>
        [HttpGet("{guid}/cpu-check-calls/kan")]
        public ActionResult KanDecision([FromRoute] Guid guid, [FromQuery] byte checkConcealedOnly)
        {
            return Ok(CheckGame(guid).Round.IaManager.KanDecision(checkConcealedOnly > 0));
        }

        /// <summary>
        /// Checks for CPU ron decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="ronCalled"><c>1</c> if ron has already been called; <c>0</c> otherwise.</param>
        /// <returns>List of player index who proceed to call ron.</returns>
        [HttpGet("{guid}/cpu-check-calls/ron")]
        public ActionResult RonDecision([FromRoute] Guid guid, [FromQuery] byte ronCalled)
        {
            return Ok(CheckGame(guid).Round.IaManager.RonDecision(ronCalled > 0));
        }

        /// <summary>
        /// Checks for CPU chii decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>Chii sequence information if any.</returns>
        [HttpGet("{guid}/cpu-check-calls/chii")]
        public ActionResult ChiiDecision([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.IaManager.ChiiDecision());
        }

        /// <summary>
        /// Checks for CPU tsumo decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <param name="isKanCompensation"><c>1</c> if kan compensation; <c>0</c> otherwise.</param>
        /// <returns><c>True</c> if tsumo; <c>False</c> otherwise.</returns>
        [HttpGet("{guid}/cpu-check-calls/tsumo")]
        public ActionResult TsumoDecision([FromRoute] Guid guid, [FromQuery] byte isKanCompensation)
        {
            return Ok(CheckGame(guid).Round.IaManager.TsumoDecision(isKanCompensation > 0));
        }

        /// <summary>
        /// Checks for CPU pon decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>The player index who makes the call if any.</returns>
        [HttpGet("{guid}/cpu-check-calls/pon")]
        public ActionResult PonDecision([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.IaManager.PonDecision());
        }

        /// <summary>
        /// Checks for CPU riichi decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>Tile discarded.</returns>
        [HttpGet("{guid}/cpu-check-calls/riichi")]
        public ActionResult RiichiDecision([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.IaManager.RiichiDecision());
        }

        /// <summary>
        /// Checks for CPU discard decision.
        /// </summary>
        /// <param name="guid">Game identifier.</param>
        /// <returns>Tile discarded.</returns>
        [HttpGet("{guid}/cpu-check-calls/discard")]
        public ActionResult DiscardDecision([FromRoute] Guid guid)
        {
            return Ok(CheckGame(guid).Round.IaManager.DiscardDecision());
        }

        private static int? CheckPlayerIndex(int? playerIndex)
        {
            if (playerIndex.HasValue && (playerIndex < 0 || playerIndex > 3))
            {
                throw new InvalidPlayerIndexException(playerIndex.Value);
            }
            return playerIndex;
        }

        private static int CheckPlayerIndex(int playerIndex)
        {
            return CheckPlayerIndex((int?)playerIndex).Value;
        }

        private static GamePivot CheckGame(Guid guid)
        {
            GamePivot game = _games.FirstOrDefault(g => g.Id == guid);
            if (game == null)
            {
                throw new InvalidGameIdentifierException(guid);
            }

            return game;
        }
    }
}