using MahjongLineServer.Pivot;

namespace MahjongLineServer.Controllers.Requests
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateGameRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public InitialPointsRulePivot InitialPointsRule { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EndOfGameRulePivot EndOfGameRule { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool WithRedDoras { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool UseNagashiMangan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool UseRenhou { get; set; }
    }
}
