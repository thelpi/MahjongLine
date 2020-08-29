using System.Collections.Generic;
using System.Linq;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents a round in a game.
    /// </summary>
    public class RoundPivot
    {
        /// <summary>
        /// History of the latest players to play.
        /// First on the list is the latest to play.
        /// The list is cleared when a jump (ie a call) is made.
        /// </summary>
        public IReadOnlyCollection<int> PlayerIndexHistory { get; set; }
        /// <summary>
        /// Wall tiles.
        /// </summary>
        public IReadOnlyCollection<TilePivot> WallTiles { get; set; }
        /// <summary>
        /// List of compensation tiles. 4 at the beginning, between 0 and 4 at the end.
        /// </summary>
        public IReadOnlyCollection<TilePivot> CompensationTiles { get; set; }
        /// <summary>
        /// List of dora indicator tiles. Always 5 (doesn't mean they're all visible).
        /// </summary>
        public IReadOnlyCollection<TilePivot> DoraIndicatorTiles { get; set; }
        /// <summary>
        /// List of ura-dora indicator tiles. Always 5 (doesn't mean they're all visible).
        /// </summary>
        public IReadOnlyCollection<TilePivot> UraDoraIndicatorTiles { get; set; }
        /// <summary>
        /// Other tiles of the treasure Always 4 minus the number of tiles of <see cref="_compensationTiles"/>.
        /// </summary>
        public IReadOnlyCollection<TilePivot> DeadTreasureTiles { get; set; }
        /// <summary>
        /// Riichi informations of four players.
        /// </summary>
        /// <remarks>The list if filled by default with <c>Null</c> for every players.</remarks>
        public IReadOnlyCollection<RiichiPivot> Riichis { get; set; }
        /// <summary>
        /// The current player index, between 0 and 3.
        /// </summary>
        public int CurrentPlayerIndex { get; set; }
        /// <summary>
        /// The game in which this instance happens.
        /// </summary>
        internal GamePivot Game { get; set; }
        /// <summary>
        /// Inferred; indicates if the current player is the human player.
        /// </summary>
        public bool IsHumanPlayer { get; set; }
        /// <summary>
        /// Inferred; indicates if the previous player is the human player.
        /// </summary>
        public bool PreviousIsHumanPlayer { get; set; }
        /// <summary>
        /// Inferred; indicates the index of the player before <see cref="CurrentPlayerIndex"/>.
        /// </summary>
        public int PreviousPlayerIndex { get; set; }
        /// <summary>
        /// Inferred; indicates if the current round is over by wall exhaustion.
        /// </summary>
        public bool IsWallExhaustion { get; set; }
        /// <summary>
        /// Inferred; count of visible doras.
        /// </summary>
        public int VisibleDorasCount { get; set; }
    }
}