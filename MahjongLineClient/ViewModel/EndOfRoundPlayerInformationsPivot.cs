using System.Collections.Generic;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents informations relative to one player at the end of the round.
    /// </summary>
    public class EndOfRoundPlayerInformationsPivot
    {
        /// <summary>
        /// Index in <see cref="GamePivot.Players"/>.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Fan count.
        /// </summary>
        public int FanCount { get; set; }
        /// <summary>
        /// Fu count.
        /// </summary>
        public int FuCount { get; set; }
        /// <summary>
        /// The points gain from the hand itself (zero or positive).
        /// </summary>
        public int HandPointsGain { get; set; }
        /// <summary>
        /// Points gain for this round (might be negative).
        /// </summary>
        public int PointsGain { get; set; }
        /// <summary>
        /// Dora count.
        /// </summary>
        public int DoraCount { get; set; }
        /// <summary>
        /// Ura-dora count.
        /// </summary>
        public int UraDoraCount { get; set; }
        /// <summary>
        /// Red dora count.
        /// </summary>
        public int RedDoraCount { get; set; }
        /// <summary>
        /// Inferred; list of yakus in the hand.
        /// </summary>
        public IReadOnlyCollection<YakuPivot> Yakus { get; set; }
        /// <summary>
        /// Inferred; <c>True</c> if concealed hand; <c>False</c> otherwise.
        /// </summary>
        public bool Concealed { get; set; }
        /// <summary>
        /// Related hand.
        /// </summary>
        public HandPivot Hand { get; set; }
    }
}
