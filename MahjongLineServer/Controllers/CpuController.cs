using System;
using MahjongLineServer.Pivot;
using Microsoft.AspNetCore.Mvc;

namespace MahjongLineServer.Controllers
{
    /// <summary>
    /// API methods to manage CPU decisions.
    /// </summary>
    [Produces("application/json")]
    [Route("games/{gameId}/cpu-check-calls")]
    public class CpuController : Controller
    {
        /// <summary>
        /// Checks for CPU kan decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="checkConcealedOnly"><c>1</c> if only concealed should be checked; <c>0</c> otherwise.</param>
        /// <returns>Kan informations, if any.</returns>
        [HttpGet("kan")]
        public ActionResult KanDecision([FromRoute] Guid gameId, [FromQuery] byte checkConcealedOnly)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.KanDecision(checkConcealedOnly > 0));
        }

        /// <summary>
        /// Checks for CPU ron decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="ronCalled"><c>1</c> if ron has already been called; <c>0</c> otherwise.</param>
        /// <returns>List of player index who proceed to call ron.</returns>
        [HttpGet("ron")]
        public ActionResult RonDecision([FromRoute] Guid gameId, [FromQuery] byte ronCalled)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.RonDecision(ronCalled > 0));
        }

        /// <summary>
        /// Checks for CPU chii decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <returns>Chii sequence information if any.</returns>
        [HttpGet("chii")]
        public ActionResult ChiiDecision([FromRoute] Guid gameId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.ChiiDecision());
        }

        /// <summary>
        /// Checks for CPU tsumo decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="isKanCompensation"><c>1</c> if kan compensation; <c>0</c> otherwise.</param>
        /// <returns><c>True</c> if tsumo; <c>False</c> otherwise.</returns>
        [HttpGet("tsumo")]
        public ActionResult TsumoDecision([FromRoute] Guid gameId, [FromQuery] byte isKanCompensation)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.TsumoDecision(isKanCompensation > 0));
        }

        /// <summary>
        /// Checks for CPU pon decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <returns>The player index who makes the call if any.</returns>
        [HttpGet("pon")]
        public ActionResult PonDecision([FromRoute] Guid gameId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.PonDecision());
        }

        /// <summary>
        /// Checks for CPU riichi decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <returns>Tile to discard.</returns>
        [HttpGet("riichi")]
        public ActionResult RiichiDecision([FromRoute] Guid gameId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.RiichiDecision());
        }

        /// <summary>
        /// Checks for CPU discard decision.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <returns>Tile discarded.</returns>
        [HttpGet("discard")]
        public ActionResult DiscardDecision([FromRoute] Guid gameId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            return Ok(round.IaManager.DiscardDecision());
        }
    }
}
