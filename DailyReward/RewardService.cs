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
        private readonly IRewardStrategy _dailyRewardStrategy;
        private readonly IRewardStrategy _rewardByTimeStrategy;
        private readonly RewardsModel _rewardsModel;

        public RewardService(RewardConfig rewardConfig)
        {
            _rewardsModel = new RewardsModel();
            
            _dailyRewardStrategy = new DailyRewardFactory(rewardConfig, _rewardsModel).CreateRewardStrategy();
            _rewardByTimeStrategy = new RewardByTimeFactory(rewardConfig, _rewardsModel).CreateRewardStrategy();
        }

        public RewardState GetStateOfIndex(int index)
        {
            var rewards = _rewardsModel.RewardsByState;

            return rewards[index];
        }

        public AsyncReactiveProperty<string> GetRemainTimeToNextReward()
        {
            return _dailyRewardStrategy.RemainTime;
        }
        
        public AsyncReactiveProperty<string> GetRemainTimeToSessionReward()
        {
            return _rewardByTimeStrategy.RemainTime;
        }

        public Reward GetTimeBaseReward()
        {
            return _rewardByTimeStrategy.GetActiveReward();
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

        public bool CanClaimTimeBasedReward()
        {
            return _rewardByTimeStrategy.CanGetReward();
        }

        public void ClaimByTimeReward()
        {
            _rewardByTimeStrategy.ClaimReward();
        }
    }
}