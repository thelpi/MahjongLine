﻿namespace MahjongLineClient
{
    /// <summary>
    /// Represents a tile.
    /// </summary>
    public class TilePivot
    {
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
    }
}
