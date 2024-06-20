using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Observers;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Factories
{
    public class DailyRewardFactory : RewardFactory
    {
        private readonly RewardConfig _rewardConfig;
        private readonly RewardObserver _rewardObserver;

        public DailyRewardFactory(RewardConfig rewardConfig, RewardObserver rewardObserver)
        {
            _rewardConfig = rewardConfig;
            _rewardObserver = rewardObserver;
        }
        
        public override IRewardStrategy CreateRewardStrategy()
        {
            var rewardStrategy = new DailyRewardStrategy(_rewardConfig, _rewardObserver);
            
            return rewardStrategy;
        }
    }
}