using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents informations computed at the end of a round.
    /// </summary>
    public class EndOfRoundInformationsPivot
    {
        /// <summary>
        /// <c>True</c> to "Ryuukyoku" (otherwie, resets <see cref="GamePivot.PendingRiichiCount"/>).
        /// </summary>
        public bool Ryuukyoku { get; set; }
        /// <summary>
        /// <c>True</c> if the current east has not won this round.
        /// </summary>
        public bool ToNextEast { get; set; }
        /// <summary>
        /// Indicates the end of the game if <c>True</c>.
        /// </summary>
        /// <remarks><c>internal</c> because it sets long after the constructor call.</remarks>
        public bool EndOfGame { get; set; }
        /// <summary>
        /// Indicates if the dura-dora tiles must be displayed.
        /// </summary>
        public bool DisplayUraDora { get; set; }
        /// <summary>
        /// Honba count.
        /// </summary>
        public int HonbaCount { get; set; }
        /// <summary>
        /// Pending riichi count.
        /// </summary>
        public int PendingRiichiCount { get; set; }
        /// <summary>
        /// Count of dora to display.
        /// </summary>
        public int DoraVisibleCount { get; set; }
        /// <summary>
        /// Informations relative to each player.
        /// </summary>
        public IReadOnlyCollection<EndOfRoundPlayerInformationsPivot> PlayersInfo { get; set; }
        /// <summary>
        /// List of dora indicators for this round.
        /// </summary>
        public IReadOnlyCollection<TilePivot> DoraTiles { get; set; }
        /// <summary>
        /// List of uradora indicators for this round.
        /// </summary>
        public IReadOnlyCollection<TilePivot> UraDoraTiles { get; set; }
        /// <summary>
        /// Inferred; count of uradora to display.
        /// </summary>
        public int UraDoraVisibleCount { get; set; }
        /// <summary>
        /// Related hand.
        /// </summary>
        public HandPivot Hand { get; set; }

        /// <summary>
        /// Get the <see cref="PlayerInformationsPivot.PointsGain"/> of the specified player index.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="defaultValue">Optionnal, value to return if player not found; default value is <c>0</c>.</param>
        /// <returns>The points gain.</returns>
        public int GetPlayerPointsGain(int playerIndex, int defaultValue = 0)
        {
            return PlayersInfo.FirstOrDefault(p => p.Index == playerIndex)?.PointsGain ?? defaultValue;
        }
    }
}
