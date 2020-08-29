﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public EndOfGameWindow(GamePivot game)
        {
            InitializeComponent();

            List<PlayerScorePivot> playerScores = ScoreTools.ComputeCurrentRanking(game);
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
