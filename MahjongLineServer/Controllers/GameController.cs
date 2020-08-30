using System;
using System.Collections.Generic;
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
        private const int MAX_HOST_GAMES = 100;

        /// <summary>
        /// Collection of <see cref="GamePivot"/> instances.
        /// </summary>
        public static IReadOnlyCollection<GamePivot> Games { get { return _games; } }

        /// <summary>
        /// Gets every games.
        /// </summary>
        /// <returns>List of <see cref="GamePivot"/> instances.</returns>
        [HttpGet]
        public ActionResult GetGamesList()
        {
            return Ok(Games);
        }

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

            if (_games.Count >= MAX_HOST_GAMES)
            {
                return Forbid();
            }

            GamePivot game = new GamePivot(request.InitialPointsRule, request.EndOfGameRule,
                request.WithRedDoras, request.UseNagashiMangan, request.UseRenhou);
            _games.Add(game);
            return Ok(game);
        }

        /// <summary>
        /// Adds a player to the game.
        /// </summary>
        /// <param name="gameId">The game GUID.</param>
        /// <param name="request">Player informations request.</param>
        /// <returns>Instance of game.</returns>
        [HttpPost("{gameId}/players")]
        public ActionResult AddPlayer([FromRoute] Guid gameId, [FromBody] PlayerRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            GamePivot game = this.CheckGame(gameId);
            game.AddPlayer(request.PlayerName, request.IsCpu);
            return Ok(game);
        }

        /// <summary>
        /// Gets players rankings.
        /// </summary>
        /// <param name="gameId">The game GUID.</param>
        /// <returns>Collection of <see cref="PlayerScorePivot"/>.</returns>
        [HttpGet("{gameId}/rankings")]
        public ActionResult ComputeRankings([FromRoute] Guid gameId)
        {
            GamePivot game = this.CheckGame(gameId);
            return Ok(ScoreTools.ComputeCurrentRanking(game));
        }

        /// <summary>
        /// Gets current wind for specified player.
        /// </summary>
        /// <param name="gameId">The game GUID.</param>
        /// <param name="playerIndex">The player index.</param>
        /// <returns>Current <see cref="WindPivot"/>.</returns>
        [HttpGet("{gameId}/players/{playerIndex}/winds")]
        public ActionResult GetPlayerCurrentWind([FromRoute] Guid gameId, [FromRoute] int playerIndex)
        {
            GamePivot game = this.CheckGame(gameId);
            this.CheckPlayerIndex(playerIndex);
            return Ok(game.GetPlayerCurrentWind(playerIndex));
        }

        /// <summary>
        /// Gets a game by its identifier.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <returns>Game instance.</returns>
        [HttpGet("{gameId}")]
        public ActionResult GetGame([FromRoute] Guid gameId)
        {
            GamePivot game = this.CheckGame(gameId);
            return Ok(game);
        }

        /// <summary>
        /// Proceeds to next round.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <param name="ronPlayerId">Ron player index; if any.</param>
        /// <returns>Instance of <see cref="EndOfRoundInformationsPivot"/>.</returns>
        [HttpPatch("{gameId}/rounds")]
        public ActionResult NextRound([FromRoute] Guid gameId, [FromQuery] int? ronPlayerId)
        {
            GamePivot game = this.CheckGame(gameId);
            if (ronPlayerId.HasValue)
            {
                this.CheckPlayerIndex(ronPlayerId.Value);
            }
            return Ok(game.NextRound(ronPlayerId));
        }

        /// <summary>
        /// Proceeds to undo a compensation pick.
        /// </summary>
        /// <param name="gameId">Game identifier.</param>
        /// <returns>No content.</returns>
        [HttpPatch("{gameId}/compensation-pick-undoing")]
        public ActionResult UndoPickCompensationTile([FromRoute] Guid gameId)
        {
            GamePivot game = this.CheckGame(gameId);
            game.Round.UndoPickCompensationTile();
            return NoContent();
        }
    }
}