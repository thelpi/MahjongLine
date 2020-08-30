using System;
using System.Linq;
using MahjongLineServer.Pivot;
using Microsoft.AspNetCore.Mvc;

namespace MahjongLineServer.Controllers
{
    /// <summary>
    /// API methods to check calls availability.
    /// </summary>
    /// <seealso cref="Controller"/>
    [Produces("application/json")]
    [Route("games/{gameId}/players/{playerIndex}/check-calls")]
    public class CallCheckController : Controller
    {
        /// <summary>
        /// Checks for tsumo call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="isKanCompensation"><c>1</c> if kan compensation; <c>0</c> otherwise.</param>
        /// <returns><c>True</c> if can tsumo; <c>False</c> otherwise.</returns>
        [HttpGet("tsumo")]
        public ActionResult CanCallTsumo([FromRoute] Guid gameId, [FromRoute] int playerIndex, [FromQuery] byte isKanCompensation)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.CanCallTsumo(isKanCompensation > 0));
        }

        /// <summary>
        /// Checks for auto-discard call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if can auto-discard.</returns>
        [HttpGet("auto-discard")]
        public ActionResult HumanCanAutoDiscard([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.HumanCanAutoDiscard());
        }

        /// <summary>
        /// Checks for chii call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>List of possible chiis, if any.</returns>
        [HttpGet("chii")]
        public ActionResult CanCallChii([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.CanCallChii().ToList());
        }

        /// <summary>
        /// Checks for kan call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>List of possible kans, if any.</returns>
        [HttpGet("kan")]
        public ActionResult CanCallKan([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            return Ok(round.CanCallKan(playerIndex));
        }

        /// <summary>
        /// Checks for pon or kan call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if kan or pon possible; <c>False</c> otherwise.</returns>
        [HttpGet("pon-or-kan")]
        public ActionResult CanCallPonOrKan([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            return Ok(round.CanCallPonOrKan(playerIndex));
        }

        /// <summary>
        /// Checks for riichi call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>Chii informations.</returns>
        [HttpGet("riichi")]
        public ActionResult CanCallRiichi([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.CanCallRiichi());
        }

        /// <summary>
        /// Checks for discard call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="tileId">Tile identifier.</param>
        /// <returns><c>True</c> if tile can be discarded; <c>False</c> otherwise.</returns>
        [HttpGet("discard")]
        public ActionResult CanDiscard([FromRoute] Guid gameId, [FromRoute] int playerIndex, [FromQuery] Guid tileId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.CanDiscard(round.GetTileFromIdentifier(tileId)));
        }

        /// <summary>
        /// Checks for pon call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if pon possible; <c>False</c> otherwise.</returns>
        [HttpGet("pon")]
        public ActionResult CanCallPon([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            return Ok(round.CanCallPon(playerIndex));
        }

        /// <summary>
        /// Checks for ron call.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if ron possible; <c>False</c> otherwise.</returns>
        [HttpGet("ron")]
        public ActionResult CanCallRon([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            return Ok(round.CanCallRon(playerIndex));
        }
    }
}
