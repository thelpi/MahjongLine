namespace MahjongLineServer.Controllers.Requests
{
    /// <summary>
    /// Player creation request.
    /// </summary>
    public class PlayerRequest
    {
        /// <summary>
        /// Player name.
        /// </summary>
        public string PlayerName { get; set; }
        /// <summary>
        /// CPU / human player.
        /// </summary>
        public bool IsCpu { get; set; }
    }
}
