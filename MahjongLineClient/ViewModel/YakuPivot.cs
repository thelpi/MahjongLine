using System.Collections.Generic;

namespace MahjongLineClient
{
    /// <summary>
    /// Represents a yaku.
    /// </summary>
    public class YakuPivot
    {
        /// <summary>
        /// Name (japanese).
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Name (english).
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Fan count;
        /// <c>13</c> for yakuman;
        /// <c>0</c> if the yaku must be concealed.
        /// </summary>
        public int FanCount { get; set; }
        /// <summary>
        /// Bonus fan count when concealed.
        /// </summary>
        public int ConcealedBonusFanCount { get; set; }
        /// <summary>
        /// List of <see cref="YakuPivot"/> which make this yaku obsolete.
        /// Yakumans are not in this list as they make every other yakus obsolete.
        /// </summary>
        /// <remarks>Empty for yakumans.</remarks>
        public IReadOnlyCollection<YakuPivot> Upgrades { get; set; }
        /// <summary>
        /// Inferred; indicates if the yaku is only valid when concealed.
        /// </summary>
        public bool IsConcealedOnly { get; set; }
        /// <summary>
        /// Inferred; indicates the fan count when concealed.
        /// </summary>
        public int ConcealedFanCount { get; set; }
        /// <summary>
        /// Inferred; indicates if the yaku is a yakuman (when concealed, at least).
        /// </summary>
        public bool IsYakuman { get; set; }

        /// <summary>
        /// Dora display name.
        /// </summary>
        public const string Dora = "Dora";
        /// <summary>
        /// Ura dora display name.
        /// </summary>
        public const string UraDora = "Uradora";
        /// <summary>
        /// Red dora display name.
        /// </summary>
        public const string RedDora = "Akadora";
    }
}
