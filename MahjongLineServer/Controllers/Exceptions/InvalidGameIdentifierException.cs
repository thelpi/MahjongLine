using System;

namespace MahjongLineServer.Controllers.Exceptions
{
    /// <summary>
    /// Exception thrown when the game GUID from the HTTP request is invalid.
    /// </summary>
    /// <seealso cref="Exception"/>
    public class InvalidGameIdentifierException : ArgumentException
    {
        /// <summary>
        /// Specified game GUID.
        /// </summary>
        public Guid GameGuid { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gameGuid">Game GUID from the HTTP request.</param>
        public InvalidGameIdentifierException(Guid gameGuid)
            : base("Invalid game GUID.", nameof(gameGuid))
        {
            GameGuid = gameGuid;
        }
    }
}
