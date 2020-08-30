using System;
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
        /// <summary>
        /// Collection of <see cref="HandPivot"/>.
        /// </summary>
        public IReadOnlyCollection<HandPivot> Hands { get; set; }
        /// <summary>
        /// Discard tiles for every player.
        /// </summary>
        public IReadOnlyCollection<IReadOnlyCollection<TilePivot>> Discards { get; set; }

        /// <summary>
        /// Checks if the specified player is riichi.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <returns><c>True</c> if riichi; <c>False</c> otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="playerIndex"/> is out of range.</exception>
        public bool IsRiichi(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }

            return Riichis.ElementAt(playerIndex) != null;
        }

        /// <summary>
        /// Gets the hand of a specified player.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>Instance of <see cref="HandPivot"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="playerIndex"/> should be between 0 and 3.</exception>
        public HandPivot GetHand(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }

            return Hands.ElementAt(playerIndex);
        }

        /// <summary>
        /// Gets the discard of a specified player.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <returns>Collection of discarded <see cref="TilePivot"/> instances.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="playerIndex"/> should be between 0 and 3.</exception>
        public IReadOnlyCollection<TilePivot> GetDiscard(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }

            return Discards.ElementAt(playerIndex);
        }

        /// <summary>
        /// Checks, for a specified player, if the specified rank is the one when the riichi call has been made.
        /// </summary>
        /// <param name="playerIndex">The player index.</param>
        /// <param name="rank">The rank.</param>
        /// <returns><c>True</c> if the specified rank is the riichi one.</returns>
        public bool IsRiichiRank(int playerIndex, int rank)
        {
            if (playerIndex < 0 || playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }

            return Riichis.ElementAt(playerIndex) != null && Riichis.ElementAt(playerIndex).DiscardRank == rank;
        }

        /// <summary>
        /// Gets the index of a tile in the concealed tiles of the current player.
        /// </summary>
        /// <param name="tile">The tile to find.</param>
        /// <param name="playerIndex">Optionnal; the player index if not the current one.</param>
        /// <returns>The tile index.</returns>
        public int GetTileIndex(TilePivot tile, int? playerIndex = null)
        {
            int realPlayerIndex = playerIndex ?? CurrentPlayerIndex;
            return GetHand(realPlayerIndex).ConcealedTiles.ToList().FindIndex(t => ReferenceEquals(t, tile));
        }
    }
}