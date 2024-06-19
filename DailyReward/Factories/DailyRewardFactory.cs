using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Factories
{
    public class DailyRewardFactory : RewardFactory
    {
        private readonly RewardConfig _rewardConfig;

        public DailyRewardFactory(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
        }
        
        public override IRewardStrategy CreateRewardStrategy()
        {
            var rewardStrategy = new DailyRewardStrategy(_rewardConfig);
            
            return rewardStrategy;
        }
    }
}