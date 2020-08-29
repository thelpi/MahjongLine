using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 
        /// </summary>
        /// <returns></returns>
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

            GamePivot game = _games.FirstOrDefault(g => g.Id == guid);
            if (game == null)
            {
                return NotFound();
            }

            game.AddPlayer(request.PlayerName, request.IsCpu);

            return Ok(game);
        }
    }
}