using System;
using System.Collections.Generic;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents a game.
    /// </summary>
    public class GamePivot
    {
        public const int HUMAN_INDEX = 0;

        /// <summary>
        /// Game unique identifier.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// List of players.
        /// </summary>
        public IReadOnlyCollection<PlayerPivot> Players { get; set; }
        /// <summary>
        /// Current dominant wind.
        /// </summary>
        public WindPivot DominantWind { get; set; }
        /// <summary>
        /// Index of the player in <see cref="_players"/> currently east.
        /// </summary>
        public int EastIndex { get; set; }
        /// <summary>
        /// Number of rounds with the current <see cref="EastIndex"/>.
        /// </summary>
        public int EastIndexTurnCount { get; set; }
        /// <summary>
        /// Pending riichi count.
        /// </summary>
        public int PendingRiichiCount { get; set; }
        /// <summary>
        /// East rank (1, 2, 3, 4).
        /// </summary>
        public int EastRank { get; set; }
        /// <summary>
        /// Current <see cref="RoundPivot"/>.
        /// </summary>
        public RoundPivot Round { get; set; }
        /// <summary>
        /// <c>True</c> if akadora are used; <c>False</c> otherwise.
        /// </summary>
        public bool WithRedDoras { get; set; }
        /// <summary>
        /// Indicates if the yaku <see cref="YakuPivot.NagashiMangan"/> is used or not.
        /// </summary>
        public bool UseNagashiMangan { get; set; }
        /// <summary>
        /// Indicates if the yakuman <see cref="YakuPivot.Renhou"/> is used or not.
        /// </summary>
        public bool UseRenhou { get; set; }
        /// <summary>
        /// The rule to check end of the game.
        /// </summary>
        public EndOfGameRulePivot EndOfGameRule { get; set; }
        /// <summary>
        /// The rule for players initial points.
        /// </summary>
        public InitialPointsRulePivot InitialPointsRule { get; set; }
        /// <summary>
        /// Inferred; current east player.
        /// </summary>
        public PlayerPivot CurrentEastPlayer { get; set; }
        /// <summary>
        /// Inferred; get players sorted by their ranking.
        /// </summary>
        public IReadOnlyCollection<PlayerPivot> PlayersRanked { get; set; }
        /// <summary>
        /// Inferred; gets the player index which was the first <see cref="WindPivot.East"/>.
        /// </summary>
        public int FirstEastIndex { get; set; }
        /// <summary>
        /// Indicates if the game can start.
        /// </summary>
        public bool IsReadyToStart { get; set; }
    }
}
