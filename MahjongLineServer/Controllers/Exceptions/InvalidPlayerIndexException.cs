using System;

namespace MahjongLineServer.Controllers.Exceptions
{
    /// <summary>
    /// Exception thrown when the player index from the HTTP request is invalid.
    /// </summary>
    /// <seealso cref="Exception"/>
    public class InvalidPlayerIndexException : ArgumentException
    {
        /// <summary>
        /// Specified player index.
        /// </summary>
        public int PlayerIndex { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="playerIndex">Player index from the HTTP request.</param>
        public InvalidPlayerIndexException(int playerIndex)
            : base("Invalid player identifier.", nameof(playerIndex))
        {
            PlayerIndex = playerIndex;
        }
    }
}
