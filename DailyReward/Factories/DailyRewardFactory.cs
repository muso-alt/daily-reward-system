using Muso.DailyReward.Data;
using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Factories
{
    public class DailyRewardFactory : RewardFactory
    {
        private readonly RewardConfig _rewardConfig;
        private readonly RewardsModel _rewardsModel;

        public DailyRewardFactory(RewardConfig rewardConfig, RewardsModel rewardsModel)
        {
            _rewardConfig = rewardConfig;
            _rewardsModel = rewardsModel;
        }
        
        public override IRewardStrategy CreateRewardStrategy()
        {
            var rewardStrategy = new DailyRewardStrategy(_rewardConfig, _rewardsModel);
            
            return rewardStrategy;
        }
    }
}