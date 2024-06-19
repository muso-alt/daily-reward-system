using Muso.DailyReward;
using Muso.DailyReward.Factories;
using Muso.DailyReward.Interfaces;

namespace Runtime.DailyReward
{
    public class RewardService
    {
        private readonly RewardConfig _rewardConfig;
        private readonly IRewardStrategy _dailyRewardStrategy;
        private readonly IRewardStrategy _byTimeRewardStrategy;

        public RewardService(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
            
            _dailyRewardStrategy = new DailyRewardFactory(rewardConfig).CreateRewardStrategy();
            _byTimeRewardStrategy = new RewardByTimeFactory(rewardConfig).CreateRewardStrategy();
        }

        public Reward[] GetDailyRewardsList()
        {
            return _rewardConfig.DailyRewards;
        }

        public TimeBasedReward[] GetRewardsByTimeList()
        {
            return _rewardConfig.RewardsByTime;
        }

        public Reward GetDailyReward()
        {
            return _dailyRewardStrategy.GetActiveReward();
        }

        public bool ClaimDailyReward()
        {
            if (_dailyRewardStrategy.CanGetReward())
            {
                _dailyRewardStrategy.ClaimReward();
                return true;
            }

            return false;
        }

        public Reward GetRewardByTime()
        {
            return _byTimeRewardStrategy.GetActiveReward();
        }

        public bool ClaimRewardByTime()
        {
            if (_byTimeRewardStrategy.CanGetReward())
            {
                _byTimeRewardStrategy.ClaimReward();
                return true;
            }

            return false;
        }
    }
}