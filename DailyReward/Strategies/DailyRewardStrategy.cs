using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Observers;

namespace Muso.DailyReward.Strategies
{
    public class DailyRewardStrategy : IRewardStrategy
    {
        public AsyncReactiveProperty<string> RemainTime { get; } = new(default);
        
        private readonly RewardConfig _rewardConfig;
        private readonly RewardObserver _rewardObserver;
        private readonly RewardStateSaver _rewardStateSaver;
        private readonly CancellationTokenSource _tokenSource;
        private readonly TimeSpan _rewardInterval = TimeSpan.FromDays(1);

        private const float TestIncreaseDay = 0f;
        private int _daysFromReward;
        
        public DailyRewardStrategy(RewardConfig rewardConfig, RewardObserver rewardObserver)
        {
            _rewardConfig = rewardConfig;
            _rewardObserver = rewardObserver;
            
            _rewardStateSaver = new RewardStateSaver();
            _tokenSource = new CancellationTokenSource();
            
            Initialize();
        }

        private void Initialize()
        {
            _rewardStateSaver.Init();
            
            UpdateDateState();
            UpdateRewardsState();

            UpdateRemainTime().Forget();
        }

        public void ClaimReward()
        {
            _rewardStateSaver.LastRewardTime = DateTime.Now + TimeSpan.FromDays(TestIncreaseDay);
            
            if (_rewardStateSaver.DayOneDate == DateTime.MinValue)
            {
                _rewardStateSaver.DayOneDate = DateTime.Today + TimeSpan.FromDays(TestIncreaseDay);
            }
            
            _rewardStateSaver.Save();
            UpdateRewardsState();
        }

        public bool CanGetReward()
        {
            return ((DateTime.Now + TimeSpan.FromDays(TestIncreaseDay)) - _rewardStateSaver.LastRewardTime) >=
                   _rewardInterval;
        }

        public Reward GetActiveReward()
        {
            var rewards = _rewardObserver.GetRewardsByState();
            
            foreach (var reward in rewards)
            {
                if (reward.Value == RewardState.Active)
                {
                    return _rewardConfig.DailyRewards[reward.Key];
                }
            }

            return null;
        }

        private void UpdateRewardsState()
        {
            var rewards = _rewardObserver.GetRewardsByState();
            var count = _rewardConfig.DailyRewards.Length;
            
            for (var i = 0; i < count; i++)
            {
                var status = RewardState.Unavailable;
                var today = i == _daysFromReward;
                
                if (i < _daysFromReward || (today && !CanGetReward()))
                {
                    status = RewardState.Received;
                }
                else if (today && CanGetReward())
                {
                    status = RewardState.Active;
                }
                else if (i == _daysFromReward + 1 && !CanGetReward())
                {
                    status = RewardState.Next;
                }
                
                rewards[i] = status;
            }
        }
        
        private void UpdateDateState()
        {
            if (_rewardStateSaver.LastRewardTime != DateTime.MinValue)
            {
                _daysFromReward = (DateTime.Today + TimeSpan.FromDays(TestIncreaseDay) - _rewardStateSaver.DayOneDate)
                    .Days;
            }
            
            //If user skips more than one day, reset dates
            if ((DateTime.Today + TimeSpan.FromDays(TestIncreaseDay) - _rewardStateSaver.LastRewardTime).TotalDays > 1)
            {
                _rewardStateSaver.LastRewardTime = DateTime.MinValue;
                _rewardStateSaver.DayOneDate = DateTime.Today + TimeSpan.FromDays(TestIncreaseDay);
                _rewardStateSaver.Save();
            }
            //Reset after week
            else if (_daysFromReward >= _rewardConfig.DailyRewards.Length)
            {
                _rewardStateSaver.DayOneDate = DateTime.Today + TimeSpan.FromDays(TestIncreaseDay);
                _rewardStateSaver.Save();
            }
        }
        
        private async UniTask UpdateRemainTime()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                var tomorrowTime = _rewardStateSaver.LastRewardTime + _rewardInterval;
                var remainingTime = tomorrowTime - (DateTime.Now + TimeSpan.FromDays(TestIncreaseDay));
                RemainTime.Value = $@"{remainingTime:d\:hh\:mm\:ss}";
                await UniTask.Delay(500, cancellationToken: _tokenSource.Token);
            }
        }
    }
}