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

        private static int CheckPlayerIndex(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex > 3)
            {
                throw new InvalidPlayerIndexException(playerIndex);
            }
            return playerIndex;
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