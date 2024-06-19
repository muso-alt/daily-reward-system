using Muso.DailyReward.Interfaces;
using Runtime.DailyReward;

namespace Muso.DailyReward.Strategies
{
    public class DailyRewardStrategy : IRewardStrategy
    {
        private readonly RewardConfig _rewardConfig;
        private readonly DailyRewardObserver _rewardObserver;

        public DailyRewardStrategy(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
            _rewardObserver = new DailyRewardObserver();
        }

        public bool CanGetReward()
        {
            return _rewardObserver.CanReceiveDailyReward();
        }

        public void ClaimReward()
        {
            _rewardObserver.ClaimDailyReward();
        }

        public Reward GetActiveReward()
        {
            return _rewardConfig.DailyRewards[_rewardObserver.Day % _rewardConfig.DailyRewards.Length];
        }
    }
}