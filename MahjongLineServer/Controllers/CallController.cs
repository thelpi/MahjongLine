using System;
using MahjongLineServer.Pivot;
using Microsoft.AspNetCore.Mvc;

namespace MahjongLineServer.Controllers
{
    /// <summary>
    /// API methods to make calls (chii, pon, ...).
    /// </summary>
    /// <seealso cref="Controller"/>
    [Produces("application/json")]
    [Route("games/{gameId}/players/{playerIndex}/calls")]
    public class CallController : Controller
    {
        /// <summary>
        /// Picks a tile.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>Picked tile.</returns>
        [HttpPatch("pick")]
        public ActionResult Pick([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.Pick());
        }

        /// <summary>
        /// Calls riichi while discarding the specified tile.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="tileId">Tile identifier.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("riichi")]
        public ActionResult CallRiichi([FromRoute] Guid gameId, [FromRoute] int playerIndex, [FromQuery] Guid tileId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.CallRiichi(round.GetTileFromIdentifier(tileId)));
        }

        /// <summary>
        /// Proceeds to call chii.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="startNumber">Tile number of the first tile of the sequence.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("chii")]
        public ActionResult CallChii([FromRoute] Guid gameId, [FromRoute] int playerIndex, [FromQuery] int startNumber)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.CallChii(startNumber));
        }

        /// <summary>
        /// Proceeds to call kan for the specified player.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="tileId">Optionnal tile identifier.</param>
        /// <returns>The compensation tile.</returns>
        [HttpPatch("kan")]
        public ActionResult CallKan([FromRoute] Guid gameId, [FromRoute] int playerIndex, [FromQuery] Guid? tileId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            TilePivot tile = tileId.HasValue ? round.GetTileFromIdentifier(tileId.Value, playerIndex) : null;
            return Ok(round.CallKan(playerIndex, tile));
        }

        /// <summary>
        /// Proceeds to call pon for the specified player.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("pon")]
        public ActionResult CallPon([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            return Ok(round.CallPon(playerIndex));
        }

        /// <summary>
        /// Proceeds to discard.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="tileId">Tile identifier.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPatch("discard")]
        public ActionResult Discard([FromRoute] Guid gameId, [FromRoute] int playerIndex, [FromQuery] Guid tileId)
        {
            RoundPivot round = this.CheckGame(gameId).Round;
            this.CheckPlayerIndex(playerIndex);
            this.CheckPlayerTurn(round, playerIndex);
            return Ok(round.Discard(round.GetTileFromIdentifier(tileId)));
        }
    }
}
