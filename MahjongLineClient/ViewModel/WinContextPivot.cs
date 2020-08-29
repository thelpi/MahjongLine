﻿using System;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents the context when a "ron" or a "tsumo" is called.
    /// </summary>
    public class WinContextPivot
    {
        #region Embedded properties

        // Use the yakuman 'Renhou' or not.
        private readonly bool _useRenhou;

        /// <summary>
        /// The latest tile (from self-draw or not).
        /// </summary>
        public TilePivot LatestTile { get; private set; }
        /// <summary>
        /// <c>True</c> if <see cref="LatestTile"/> was the last tile of the round (from wall or opponent discard).
        /// </summary>
        public bool IsRoundLastTile { get; private set; }
        /// <summary>
        /// <c>True</c> if the player has called riichi.
        /// </summary>
        public bool IsRiichi { get; private set; }
        /// <summary>
        /// <c>True</c> if the player has called riichi on the first turn.
        /// </summary>
        public bool IsFirstTurnRiichi { get; private set; }
        /// <summary>
        /// <c>True</c> if it's the first turn after calling riichi.
        /// </summary>
        public bool IsIppatsu { get; private set; }
        /// <summary>
        /// The current dominant wind.
        /// </summary>
        public WindPivot DominantWind { get; private set; }
        /// <summary>
        /// The current player wind.
        /// </summary>
        public WindPivot PlayerWind { get; private set; }
        /// <summary>
        /// <c>True</c> if it's first turn draw (without call made).
        /// </summary>
        public bool IsFirstTurnDraw { get; private set; }
        /// <summary>
        /// Draw type for <see cref="LatestTile"/>.
        /// </summary>
        public DrawTypePivot DrawType { get; private set; }
        /// <summary>
        /// <c>True</c> if nagashi mangan; <c>False</c> otherwise.
        /// </summary>
        public bool IsNagashiMangan { get; private set; }

        #endregion Embedded properties

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="latestTile">The <see cref="LatestTile"/> value.</param>
        /// <param name="drawType">The <see cref="DrawType"/> value.</param>
        /// <param name="dominantWind">The <see cref="DominantWind"/> value.</param>
        /// <param name="playerWind">The <see cref="PlayerWind"/> value.</param>
        /// <param name="isFirstOrLast">Optionnal; indicates a win at the first turn without any call made (<c>True</c>) or at the last tile of the round (<c>Null</c>); default value is <c>False</c>.</param>
        /// <param name="isRiichi">Optionnal; indicates if riichi (<c>True</c>) or riichi at first turn without any call made (<c>Null</c>); default value is <c>False</c>.</param>
        /// <param name="isIppatsu">Optionnal; indicates if it's a win by ippatsu (<paramref name="isRiichi"/> can't be <c>False</c> in such case); default value is <c>False</c>.</param>
        /// <param name="useRenhou">Optionnal; the <see cref="_useRenhou"/> value; default value is <c>False</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="latestTile"/> is <c>Null</c>.</exception>
        /// <exception cref="ArgumentException"><see cref="Messages.InvalidContextIppatsuValue"/></exception>
        public WinContextPivot(TilePivot latestTile, DrawTypePivot drawType, WindPivot dominantWind, WindPivot playerWind,
            bool? isFirstOrLast = false, bool? isRiichi = false, bool isIppatsu = false, bool useRenhou = false)
        {
            if (isRiichi == false && isIppatsu)
            {
                throw new ArgumentException(Messages.InvalidContextIppatsuValue, nameof(isIppatsu));
            }

            LatestTile = latestTile ?? throw new ArgumentNullException(nameof(latestTile));
            IsRoundLastTile = isFirstOrLast == null;
            IsRiichi = isRiichi != false;
            IsFirstTurnRiichi = isRiichi == null;
            IsIppatsu = isIppatsu;
            DominantWind = dominantWind;
            PlayerWind = playerWind;
            IsFirstTurnDraw = isFirstOrLast == true;
            DrawType = drawType;
            _useRenhou = useRenhou;
            IsNagashiMangan = false;
        }

        /// <summary>
        /// Empty constructor. To use when <see cref="IsNagashiMangan"/> is <c>True</c>.
        /// Every other properties are at their default value.
        /// </summary>
        public WinContextPivot()
        {
            IsNagashiMangan = true;
        }

        #endregion Constructors

        #region Public methods

        /// <summary>
        /// Checks if the context gives the yaku <see cref="YakuPivot.Tenhou"/>.
        /// </summary>
        /// <returns><c>True</c> if it gives the yaku; <c>False</c> otherwise.</returns>
        public bool IsTenhou()
        {
            return IsFirstTurnDraw && PlayerWind == WindPivot.East && DrawType.IsSelfDraw();
        }

        /// <summary>
        /// Checks if the context gives the yaku <see cref="YakuPivot.Chiihou"/>.
        /// </summary>
        /// <returns><c>True</c> if it gives the yaku; <c>False</c> otherwise.</returns>
        public bool IsChiihou()
        {
            return IsFirstTurnDraw && PlayerWind != WindPivot.East && DrawType.IsSelfDraw();
        }

        /// <summary>
        /// Checks if the context gives the yaku <see cref="YakuPivot.Renhou"/>.
        /// </summary>
        /// <returns><c>True</c> if it gives the yaku; <c>False</c> otherwise.</returns>
        public bool IsRenhou()
        {
            return IsFirstTurnDraw && !DrawType.IsSelfDraw() && _useRenhou;
        }

        #endregion Public methods
    }
}
