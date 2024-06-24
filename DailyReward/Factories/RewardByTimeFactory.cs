using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Data;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Factories
{
    public class RewardByTimeFactory : RewardFactory
    {
        private readonly RewardConfig _rewardConfig;
        private readonly RewardsModel _rewardsModel;

        public RewardByTimeFactory(RewardConfig rewardConfig, RewardsModel rewardsModel)
        {
            _rewardConfig = rewardConfig;
            _rewardsModel = rewardsModel;
        }
        
        public override IRewardStrategy CreateRewardStrategy()
        {
            var rewardStrategy = new RewardByTimeStrategy(_rewardConfig, _rewardsModel);
            
            return rewardStrategy;
        }
    }
}