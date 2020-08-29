using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents an hand.
    /// </summary>
    public class HandPivot
    {
        /// <summary>
        /// List of concealed tiles.
        /// </summary>
        public IReadOnlyCollection<TilePivot> ConcealedTiles { get; set; }
        /// <summary>
        /// List of declared <see cref="TileComboPivot"/>.
        /// </summary>
        public IReadOnlyCollection<TileComboPivot> DeclaredCombinations { get; set; }
        /// <summary>
        /// The latest pick; can't be known by <see cref="_concealedTiles"/> (sorted list).
        /// </summary>
        public TilePivot LatestPick { get; set; }
        /// <summary>
        /// Yakus, if the hand is complete; otherwise <c>Null</c>.
        /// </summary>
        public IReadOnlyCollection<YakuPivot> Yakus { get; set; }
        /// <summary>
        /// Combinations computed in the hand to produce <see cref="Yakus"/>;
        /// <c>Null</c> if <see cref="Yakus"/> is <c>Null</c> or contains <see cref="YakuPivot.KokushiMusou"/> or <see cref="YakuPivot.NagashiMangan"/>.
        /// </summary>
        public List<TileComboPivot> YakusCombinations { get; set; }
        /// <summary>
        /// Inferred; indicates if the hand is complete (can tsumo or ron depending on context).
        /// </summary>
        public bool IsComplete { get; set; }
        /// <summary>
        /// Inferred; indicates if the hand is concealed.
        /// </summary>
        public bool IsConcealed { get; set; }
        /// <summary>
        /// Inferred; every tiles of the hand; concealed or not; into combination or not.
        /// </summary>
        public IReadOnlyCollection<TilePivot> AllTiles { get; set; }

        /// <summary>
        /// Gets tiles from the hand ordered for display on the score screen.
        /// </summary>
        /// <returns>The list of tiles;
        /// the <see cref="Tuple{T1, T2, T3}.Item2"/> indicates if the tile should be displayed leaned.
        /// the <see cref="Tuple{T1, T2, T3}.Item3"/> indicates if the tile should be displayed apart.
        /// </returns>
        public List<Tuple<TilePivot, bool, bool>> GetFullHandForDisplay()
        {
            List<Tuple<TilePivot, bool, bool>> r = new List<Tuple<TilePivot, bool, bool>>();

            r.AddRange(AllTiles.Select(t => new Tuple<TilePivot, bool, bool>(t,
                DeclaredCombinations.Any(c => ReferenceEquals(c.OpenTile, t)),
                ReferenceEquals(LatestPick, t))));

            return r.OrderBy(tuple => tuple.Item3).ToList();
        }
    }
}
