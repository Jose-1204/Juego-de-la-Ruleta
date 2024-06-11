namespace Ruleta.Server.Models
{
    public class BetRequest
    {
        public string UserName { get; set; }
        public int Amount { get; set; }
        public string BetType { get; set; }
        public string BetValue { get; set; }
        public int WinningNumber { get; set; }
        public string WinningColor { get; set; }
    }
}
