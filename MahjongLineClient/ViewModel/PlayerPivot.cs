namespace MahjongLineClient
{
    /// <summary>
    /// Represents a player.
    /// </summary>
    public class PlayerPivot
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="WindPivot"/> at the first round of the game.
        /// </summary>
        public WindPivot InitialWind { get; set; }
        /// <summary>
        /// Number of points.
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// Indicates if the player is managed by the CPU.
        /// </summary>
        public bool IsCpu { get; set; }
    }
}
