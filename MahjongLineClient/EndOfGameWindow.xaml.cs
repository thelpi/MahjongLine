using System.Collections.Generic;
using System.Windows;

namespace MahjongLineClient
{
    /// <summary>
    /// Logique d'interaction pour EndOfGameWindow.xaml
    /// </summary>
    public partial class EndOfGameWindow : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game">The game.</param>
        public EndOfGameWindow(RequestManager requestManager, GamePivot game)
        {
            InitializeComponent();

            List<PlayerScorePivot> playerScores = requestManager.ComputeCurrentRanking(game);
            for (int i = 0; i < playerScores.Count; i++)
            {
                this.FindControl("LblRank", i).Content = playerScores[i].Rank;
                this.FindControl("LblPlayer", i).Content = playerScores[i].Player.Name;
                this.FindControl("LblPoints", i).Content = playerScores[i].Player.Points;
                this.FindControl("LblUma", i).ApplyGainAndLostStyle(playerScores[i].Uma);
                this.FindControl("LblScore", i).ApplyGainAndLostStyle(playerScores[i].Score);
            }
        }
    }
}
