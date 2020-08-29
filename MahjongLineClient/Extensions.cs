using System;

namespace MahjongLineClient
{
    public static class Extensions
    {
        /// <summary>
        /// Extension; computes the N-index player after (or before) the specified player index.
        /// </summary>
        /// <param name="playerIndex">The player index.</param>
        /// <param name="nIndex">The N value.</param>
        /// <returns>The relative player index.</returns>
        public static int RelativePlayerIndex(this int playerIndex, int nIndex)
        {
            if (nIndex == 0)
            {
                return playerIndex;
            }

            int nIndexMod = nIndex % 4;
            int newIndex = playerIndex + nIndexMod;

            if (nIndex > 0 && newIndex > 3)
            {
                newIndex = newIndex % 4;
            }
            else if (nIndex < 0 && newIndex < 0)
            {
                newIndex = 4 - Math.Abs(newIndex % 4);
            }

            return newIndex;
        }

        /// <summary>
        /// Extension; gets the wind to the left of the specified wind.
        /// </summary>
        /// <param name="origin">The wind.</param>
        /// <returns>The left wind.</returns>
        public static WindPivot Left(this WindPivot origin)
        {
            switch (origin)
            {
                case WindPivot.East:
                    return WindPivot.North;
                case WindPivot.South:
                    return WindPivot.East;
                case WindPivot.West:
                    return WindPivot.South;
                default:
                    return WindPivot.West;
            }
        }

        /// <summary>
        /// Extension; gets the wind to the right of the specified wind.
        /// </summary>
        /// <param name="origin">The wind.</param>
        /// <returns>The right wind.</returns>
        public static WindPivot Right(this WindPivot origin)
        {
            switch (origin)
            {
                case WindPivot.East:
                    return WindPivot.South;
                case WindPivot.South:
                    return WindPivot.West;
                case WindPivot.West:
                    return WindPivot.North;
                default:
                    return WindPivot.East;
            }
        }

        /// <summary>
        /// Extension; gets the wind to the opposite of the specified wind.
        /// </summary>
        /// <param name="origin">The wind.</param>
        /// <returns>The opposite wind.</returns>
        public static WindPivot Opposite(this WindPivot origin)
        {
            switch (origin)
            {
                case WindPivot.East:
                    return WindPivot.West;
                case WindPivot.South:
                    return WindPivot.North;
                case WindPivot.West:
                    return WindPivot.East;
                default:
                    return WindPivot.South;
            }
        }
    }
}
