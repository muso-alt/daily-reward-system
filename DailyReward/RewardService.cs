using Cysharp.Threading.Tasks;
using Muso.DailyReward;
using Muso.DailyReward.Factories;
using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Observers;
using Muso.DailyReward.Strategies;

namespace Runtime.DailyReward
{
    public class RewardService
    {
        private readonly RewardConfig _rewardConfig;
        private readonly IRewardStrategy _dailyRewardStrategy;
        private readonly RewardObserver _rewardObserver;

        public RewardService(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
            _rewardObserver = new RewardObserver();
            
            _dailyRewardStrategy = new DailyRewardFactory(rewardConfig, _rewardObserver).CreateRewardStrategy();
        }

        public RewardState GetStateOfIndex(int index)
        {
            var rewards = _rewardObserver.GetRewardsByState();

            return rewards[index];
        }

        public AsyncReactiveProperty<string> GetRemainTimeToNextReward()
        {
            return _dailyRewardStrategy.RemainTime;
        }

        public Reward GetActiveReward()
        {
            return _dailyRewardStrategy.GetActiveReward();
        }

        public void ClaimReward()
        {
            _dailyRewardStrategy.ClaimReward();
        }

        public bool CanClaimReward()
        {
            return _dailyRewardStrategy.CanGetReward();
        }
    }
}