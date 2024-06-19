using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Factories
{
    public class RewardByTimeFactory : RewardFactory
    {
        private readonly RewardConfig _rewardConfig;

        public RewardByTimeFactory(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
        }
        
        public override IRewardStrategy CreateRewardStrategy()
        {
            var rewardStrategy = new RewardByTimeStrategy(_rewardConfig);
            
            return rewardStrategy;
        }
    }
}