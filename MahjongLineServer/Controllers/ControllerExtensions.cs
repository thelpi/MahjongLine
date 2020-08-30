using System;
using System.Linq;
using MahjongLineServer.Controllers.Exceptions;
using MahjongLineServer.Pivot;
using Microsoft.AspNetCore.Mvc;

namespace MahjongLineServer.Controllers
{
    /// <summary>
    /// Extension methods for <see cref="Controller"/>.
    /// </summary>
    internal static class ControllerExtensions
    {
        /// <summary>
        /// Checks game parameter.
        /// </summary>
        /// <param name="controller">Controller who makes the call.</param>
        /// <param name="id">Game identifier.</param>
        /// <returns>The <see cref="GamePivot"/> instance if found.</returns>
        /// <exception cref="InvalidGameIdentifierException">Instance with the specified identifier not found.</exception>
        internal static GamePivot CheckGame(this Controller controller, Guid id)
        {
            GamePivot game = GameController.Games.SingleOrDefault(g => g.Id == id);
            if (game == null)
            {
                throw new InvalidGameIdentifierException(id);
            }

            return game;
        }

        /// <summary>
        /// Checks the player index argument.
        /// </summary>
        /// <param name="controller">Controller who makes the call.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <exception cref="InvalidPlayerIndexException">Invalid player index.</exception>
        internal static void CheckPlayerIndex(this Controller controller, int playerIndex)
        {
            if (playerIndex < 0 || playerIndex > 3)
            {
                throw new InvalidPlayerIndexException(playerIndex);
            }
        }

        /// <summary>
        /// Checks if it's currently the turn of the player specified by the index argument.
        /// </summary>
        /// <param name="controller">Controller who makes the call.</param>
        /// <param name="round">Current round.</param>
        /// <param name="playerIndex">Player index.</param>
        /// <exception cref="InvalidPlayerIndexException">Invalid player index.</exception>
        internal static void CheckPlayerTurn(this Controller controller, RoundPivot round, int playerIndex)
        {
            if (round.CurrentPlayerIndex != playerIndex)
            {
                throw new InvalidPlayerIndexException(playerIndex);
            }
        }
    }
}
