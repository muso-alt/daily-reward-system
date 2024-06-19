using Muso.DailyReward.Interfaces;

namespace Muso.DailyReward.Strategies
{
    public class RewardByTimeStrategy : IRewardStrategy
    {
        private readonly RewardConfig _rewardConfig;

        public RewardByTimeStrategy(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
        }

        public bool CanGetReward()
        {
            return true;
        }

        public void ClaimReward()
        {
            return;
        }

        public Reward GetActiveReward()
        {
            return _rewardConfig.RewardsByTime[0].RewardByTime;
        }
    }
}