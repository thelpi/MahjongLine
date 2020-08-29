using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents a combination of <see cref="TilePivot"/>.
    /// </summary>
    public class TileComboPivot
    {
        /// <summary>
        /// Inferred; list of tiles; includes <see cref="OpenTile"/>.
        /// </summary>
        /// <remarks>Sorted by <see cref="IComparable{TilePivot}"/>.</remarks>
        public IReadOnlyCollection<TilePivot> Tiles { get; set; }
        /// <summary>
        /// Optionnal tile not concealed (from a call "pon", "chi" or "kan").
        /// The tile from a call "ron" is not considered as an open tile.
        /// </summary>
        public readonly TilePivot OpenTile;
        /// <summary>
        /// If <see cref="OpenTile"/> is specified, indicates the wind which the tile has been stolen from; otherwise <c>Null</c>.
        /// </summary>
        public readonly WindPivot? StolenFrom;
        /// <summary>
        /// Inferred; indicates if the combination is concealed.
        /// </summary>
        public bool IsConcealed { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is a pair.
        /// </summary>
        public bool IsPair { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is a brelan.
        /// </summary>
        public bool IsBrelan { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is a square.
        /// </summary>
        public bool IsSquare { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is a sequence.
        /// </summary>
        public bool IsSequence { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is a brelan or a square.
        /// </summary>
        public bool IsBrelanOrSquare { get; set; }
        /// <summary>
        /// Inferred; gets the combination <see cref="FamilyPivot"/>.
        /// </summary>
        public FamilyPivot Family { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is formed of honors.
        /// </summary>
        public bool IsHonor { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is formed of terminals.
        /// </summary>
        /// <remarks><see cref="HasTerminal"/> is necessarily <c>True</c> in that case.</remarks>
        public bool IsTerminal { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is formed with at least one terminal.
        /// </summary>
        public bool HasTerminal { get; set; }
        /// <summary>
        /// Inferred; indicates if the combination is <see cref="HasTerminal"/> or <see cref="IsHonor"/>.
        /// </summary>
        public bool HasTerminalOrHonor { get; set; }
        /// <summary>
        /// Inferred; if sequence, the first number of it; otherwise <c>0</c>.
        /// </summary>
        public byte SequenceFirstNumber { get; set; }
        /// <summary>
        /// Inferred; if sequence, the last number of it; otherwise <c>0</c>.
        /// </summary>
        public byte SequenceLastNumber { get; set; }

        /// <summary>
        /// Checks if the specified tile index must be displayed as concealed (aka tiles 2 and 3 from a square).
        /// </summary>
        /// <param name="i">The tile index.</param>
        /// <returns><c>True</c> if concealed display; <c>False</c> otherwise.</returns>
        public bool IsConcealedDisplay(int i)
        {
            return IsSquare && IsConcealed && i > 0 && i < 3;
        }

        /// <summary>
        /// Gets the list of tiles from the combination, sorted by wind logic for display.
        /// </summary>
        /// <param name="ownerWind">The current wind of the owner.</param>
        /// <returns>List of tiles tuple; the second item is <c>True</c> when the tile is the opened one.</returns>
        public List<Tuple<TilePivot, bool>> GetSortedTilesForDisplay(WindPivot ownerWind)
        {
            if (!StolenFrom.HasValue)
            {
                return Tiles.Select(t => new Tuple<TilePivot, bool>(t, false)).ToList();
            }

            var concealedOnly = new List<TilePivot>(Tiles);
            concealedOnly.Remove(OpenTile);

            int i = 0;

            var tiles = new List<Tuple<TilePivot, bool>>
            {
                GetTileForSortedListAtSpecifiedWind(ownerWind.Left(), concealedOnly, ref i),
                GetTileForSortedListAtSpecifiedWind(ownerWind.Opposite(), concealedOnly, ref i)
            };

            // For a square, the third tile is never from an opponent.
            if (IsSquare)
            {
                tiles.Add(new Tuple<TilePivot, bool>(concealedOnly[i], false));
                i++;
            }

            tiles.Add(GetTileForSortedListAtSpecifiedWind(ownerWind.Right(), concealedOnly, ref i));

            return tiles;
        }

        // Gets the tile corresponding to the specified wind in the purpose to create a sorted list for display.
        private Tuple<TilePivot, bool> GetTileForSortedListAtSpecifiedWind(WindPivot wind, List<TilePivot> concealedOnly, ref int i)
        {
            if (wind == StolenFrom.Value)
            {
                return new Tuple<TilePivot, bool>(OpenTile, true);
            }
            else
            {
                i++;
                return new Tuple<TilePivot, bool>(concealedOnly[i - 1], false);
            }
        }

        /// <summary>
        /// Overriden; provides a textual representation of the instance.
        /// </summary>
        /// <returns>Textual representation of the instance.</returns>
        public override string ToString()
        {
            if (IsPair)
            {
                if (Family == FamilyPivot.Dragon)
                {
                    return $"Pair {Family} {Tiles.First().Dragon.Value.ToString()}";
                }
                else if (Family == FamilyPivot.Wind)
                {
                    return $"Pair {Family} {Tiles.First().Wind.Value.ToString()}";
                }
                else
                {
                    return $"Pair {Family} {Tiles.First().Number}";
                }
            }
            else if (IsBrelan)
            {
                if (Family == FamilyPivot.Dragon)
                {
                    return $"Brelan {Family} {Tiles.First().Dragon.Value.ToString()}";
                }
                else if (Family == FamilyPivot.Wind)
                {
                    return $"Brelan {Family} {Tiles.First().Wind.Value.ToString()}";
                }
                else
                {
                    return $"Brelan {Family} {Tiles.First().Number}";
                }
            }
            else if (IsSquare)
            {
                if (Family == FamilyPivot.Dragon)
                {
                    return $"Square {Family} {Tiles.First().Dragon.Value.ToString()}";
                }
                else if (Family == FamilyPivot.Wind)
                {
                    return $"Square {Family} {Tiles.First().Wind.Value.ToString()}";
                }
                else
                {
                    return $"Square {Family} {Tiles.First().Number}";
                }
            }
            else
            {
                return $"Sequence {Family} [{Tiles.First().Number}, {Tiles.First().Number + 1}, {Tiles.First().Number + 2}]";
            }
        }
    }
}
