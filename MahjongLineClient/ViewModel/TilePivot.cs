using System;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents a tile.
    /// </summary>
    /// <seealso cref="IEquatable{T}"/>
    /// <seealso cref="IComparable{T}"/>
    public class TilePivot : IEquatable<TilePivot>, IComparable<TilePivot>
    {
        /// <summary>
        /// Tile unique identifier.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Family.
        /// </summary>
        public FamilyPivot Family { get; set; }
        /// <summary>
        /// Number, between <c>1</c> and <c>9</c>.
        /// <c>0</c> for <see cref="FamilyPivot.Wind"/> and <see cref="FamilyPivot.Dragon"/>.
        /// </summary>
        public byte Number { get; set; }
        /// <summary>
        /// Wind.
        /// <c>Null</c> if not <see cref="FamilyPivot.Wind"/>.
        /// </summary>
        public WindPivot? Wind { get; set; }
        /// <summary>
        /// Dragon.
        /// <c>Null</c> if not <see cref="FamilyPivot.Dragon"/>.
        /// </summary>
        public DragonPivot? Dragon { get; set; }
        /// <summary>
        /// Indicates if the instance is a red dora.
        /// </summary>
        public bool IsRedDora { get; set; }
        /// <summary>
        /// Inferred; indicates if the instance is an honor.
        /// </summary>
        public bool IsHonor { get; set; }
        /// <summary>
        /// Inferred; indicates if the instance is a terminal.
        /// </summary>
        public bool IsTerminal { get; set; }
        /// <summary>
        /// Inferred; indicates if the instance is an honor or a terminal.
        /// </summary>
        public bool IsHonorOrTerminal { get; set; }

        #region Interfaces implementation and overrides from base

        /// <summary>
        /// Overriden; provides a textual representation of the instance.
        /// </summary>
        /// <returns>Textual representation of the instance.</returns>
        public override string ToString()
        {
            switch (Family)
            {
                case FamilyPivot.Dragon:
                    return $"{Family.ToString().ToLowerInvariant()}_{Dragon.Value.ToString().ToLowerInvariant()}";
                case FamilyPivot.Wind:
                    return $"{Family.ToString().ToLowerInvariant()}_{Wind.Value.ToString().ToLowerInvariant()}";
                default:
                    return $"{Family.ToString().ToLowerInvariant()}_{Number.ToString()}" + (IsRedDora ? "_red" : string.Empty);
            }
        }

        /// <summary>
        /// Overriden; checks equality between an instance of <see cref="TilePivot"/> and any object.
        /// </summary>
        /// <param name="tile">The <see cref="TilePivot"/> instance.</param>
        /// <param name="obj">Any <see cref="object"/>.</param>
        /// <returns><c>True</c> if instances are equal or both <c>Null</c>; <c>False</c> otherwise.</returns>
        public static bool operator ==(TilePivot tile, object obj)
        {
            return tile is null ? obj is null : tile.Equals(obj);
        }

        /// <summary>
        /// Overriden; checks inequality between an instance of <see cref="TilePivot"/> and any object.
        /// </summary>
        /// <param name="tile">The <see cref="TilePivot"/> instance.</param>
        /// <param name="obj">Any <see cref="object"/>.</param>
        /// <returns><c>False</c> if instances are equal or both <c>Null</c>; <c>True</c> otherwise.</returns>
        public static bool operator !=(TilePivot tile, object obj)
        {
            return !(tile == obj);
        }

        /// <summary>
        /// Compares this instance with another one.
        /// </summary>
        /// <param name="other">The second instance.</param>
        /// <returns>
        /// <c>-1</c> if this instance precedes <paramref name="other"/>.
        /// <c>0</c> if this instance is equal to <paramref name="other"/>.
        /// <c>1</c> if <paramref name="other"/> precedes this instance.
        /// </returns>
        public int CompareTo(TilePivot other)
        {
            if (other is null)
            {
                return 1;
            }

            switch (Family)
            {
                case FamilyPivot.Dragon:
                    if (other.Family == FamilyPivot.Dragon)
                    {
                        return Dragon.Value < other.Dragon.Value ? -1 : (Dragon.Value == other.Dragon.Value ? 0 : 1);
                    }
                    else
                    {
                        return 1;
                    }
                case FamilyPivot.Wind:
                    if (other.Family == FamilyPivot.Wind)
                    {
                        return Wind.Value < other.Wind.Value ? -1 : (Wind.Value == other.Wind.Value ? 0 : 1);
                    }
                    else if (other.Family == FamilyPivot.Dragon)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                default:
                    if (other.Family == Family)
                    {
                        if (Number < other.Number)
                        {
                            return -1;
                        }
                        else if (Number > other.Number)
                        {
                            return 1;
                        }
                        else
                        {
                            if (IsRedDora && !other.IsRedDora)
                            {
                                return -1;
                            }
                            else if (!IsRedDora && other.IsRedDora)
                            {
                                return 1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                    }
                    else if (other.Family < Family)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
            }
        }

        /// <summary>
        /// Checks the equality between this instance and another one.
        /// <see cref="IsRedDora"/> value is ignored for this comparison.
        /// </summary>
        /// <param name="other">The second instance.</param>
        /// <returns><c>True</c> if both instances are equal; <c>False</c> otherwise.</returns>
        public bool Equals(TilePivot other)
        {
            return !(other is null)
                && other.Family == Family
                && other.Wind == Wind
                && other.Dragon == Dragon
                && other.Number == Number;
        }

        /// <summary>
        /// Overriden; provides an hashcode for this instance.
        /// </summary>
        /// <returns>Hashcode of this instance.</returns>
        public override int GetHashCode()
        {
            return Tuple.Create(Family, Number, Wind, Dragon).GetHashCode();
        }

        /// <summary>
        /// Overriden; checks the equality between this instance and any other object.
        /// If <paramref name="obj"/> is a <see cref="TilePivot"/>, see <see cref="Equals(TilePivot)"/>.
        /// </summary>
        /// <param name="obj">Any <see cref="object"/>.</param>
        /// <returns><c>True</c> if both instances are equal; <c>False</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TilePivot);
        }

        #endregion Interfaces implementation and overrides from base

        /// <summary>
        /// Checks if the instance has the same ID than another instance.
        /// </summary>
        /// <param name="other">The second instance.</param>
        /// <returns><c>True</c> if instances have the same <see cref="Id"/>; <c>False</c> otherwise.</returns>
        public bool IdEquals(TilePivot other)
        {
            return Id == other?.Id;
        }
    }
}
