using System;
using Cysharp.Threading.Tasks;
using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Observers;

namespace Muso.DailyReward.Strategies
{
    public class RewardByTimeStrategy : IRewardStrategy
    {
        public AsyncReactiveProperty<string> RemainTime { get; } = new(default);

        private readonly RewardConfig _rewardConfig;
        private readonly RewardByTimeObserver _rewardByTime;

        public RewardByTimeStrategy(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
            _rewardByTime = new RewardByTimeObserver(_rewardConfig);
        }

        public bool CanGetReward()
        {
            return _rewardByTime.CanReceiveReward();
        }

        public void ClaimReward()
        {
            _rewardByTime.ClaimReward();
        }

        public Reward GetActiveReward()
        {
            if (!CanGetReward())
            {
                return null;
            }
            
            var index = _rewardByTime.GetActiveRewardIndex();
            return _rewardConfig.RewardsByTime[index].RewardByTime;
        }
        
        private async UniTask PrepareGiftsAsync()
        {
            foreach (var timeBasedReward in _rewardConfig.RewardsByTime)
            {
                var countdownTime = TimeSpan.FromMinutes(timeBasedReward.Time);
                var targetTime = DateTime.Now.Add(countdownTime);

                while (DateTime.Now < targetTime)
                {
                    var remainingTime = targetTime - DateTime.Now;
                    RemainTime.Value = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                    await UniTask.Delay(500);
                }

                RemainTime.Value = "00:00";
            }
        }
    }
}