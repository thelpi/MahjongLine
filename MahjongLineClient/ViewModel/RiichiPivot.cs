using System.Collections.Generic;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents informations relative to a riichi call.
    /// </summary>
    public class RiichiPivot
    {
        /// <summary>
        /// Position in the player discard.
        /// </summary>
        public int DiscardRank { get; set; }
        /// <summary>
        /// <c>True</c> if the riichi is "daburu" (at first turn without interruption).
        /// </summary>
        public bool IsDaburu { get; set; }
        /// <summary>
        /// The tile discarded when the call has been made.
        /// </summary>
        public TilePivot Tile { get; set; }
        /// <summary>
        /// The rank, in the virtual discard of each opponent, when the riichi call has been made.
        /// </summary>
        /// <remarks>Key is the opponent index, value is the rank (<c>-1</c> if none).</remarks>
        public IReadOnlyDictionary<int, int> OpponentsVirtualDiscardRank { get; set; }
    }
}
