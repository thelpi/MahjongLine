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
        [HttpPatch("{guid}/rounds?ronPlayerId={ronPlayerId}")]
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
        [HttpPatch("{guid}/calls/riichi?tileIndexInPlayerHand={tileIndexInPlayerHand}")]
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
        [HttpPatch("{guid}/calls/chii?startNumber={startNumber}")]
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
        [HttpPatch("{guid}/players/{playerIndex}/calls/kan?tileIndexInPlayerHand={tileIndexInPlayerHand}")]
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
        [HttpPatch("{guid}/calls/discard?tileIndexInPlayerHand={tileIndexInPlayerHand}")]
        public ActionResult Discard([FromRoute] Guid guid, [FromQuery] int tileIndexInPlayerHand)
        {
            RoundPivot round = CheckGame(guid).Round;
            TilePivot tile = round.GetTileFromIndex(tileIndexInPlayerHand);
            return Ok(round.Discard(tile));
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