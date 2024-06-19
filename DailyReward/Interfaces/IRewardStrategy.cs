namespace Muso.DailyReward.Interfaces
{
    public interface IRewardStrategy
    {
        public bool CanGetReward();
        public void ClaimReward();
        public Reward GetActiveReward();
    }
}