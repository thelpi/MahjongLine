namespace MahjongLineClient
{
    /// <summary>
    /// Represents a player score at the end of the game.
    /// </summary>
    public class PlayerScorePivot
    {
        /// <summary>
        /// Ranking.
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// The player.
        /// </summary>
        public PlayerPivot Player { get; set; }
        /// <summary>
        /// Uma.
        /// </summary>
        public int Uma { get; set; }
        /// <summary>
        /// Final score; <see cref="PlayerPivot.Points"/> (only thousands) plus <see cref="Uma"/>.
        /// </summary>
        public int Score { get; set; }
    }
}
