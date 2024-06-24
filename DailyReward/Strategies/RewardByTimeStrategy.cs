using System;
using Cysharp.Threading.Tasks;
using Muso.DailyReward.Interfaces;
using Muso.DailyReward.Data;
using UnityEngine;

namespace Muso.DailyReward.Strategies
{
    public class RewardByTimeStrategy : IRewardStrategy
    {
        public AsyncReactiveProperty<string> RemainTime { get; } = new(default);

        private readonly RewardConfig _rewardConfig;
        private readonly RewardsModel _rewardsModel;
        
        private const string LastRewardTimeKey = "LastRewardTime";
        private const string RewardIndexInCurrentSessionKey = "RewardIndexInCurrentSession";
        
        private DateTime _lastGiftTime;
        private int _indexOfReward;

        public RewardByTimeStrategy(RewardConfig rewardConfig, RewardsModel rewardsModel)
        {
            _rewardConfig = rewardConfig;
            _rewardsModel = rewardsModel;
            
            LoadLastGiftTime();
            LoadRewardInCurrentSession();
            UpdateRemainTime().Forget();
        }

        public bool CanGetReward()
        {
            return _indexOfReward < _rewardConfig.RewardsByTime.Length && _rewardsModel.PreparedRewards.Count > 0;
        }

        public Reward GetActiveReward()
        {
            if (_indexOfReward >= _rewardConfig.RewardsByTime.Length)
            {
                return null;
            }
            
            var reward = _rewardConfig.RewardsByTime[_indexOfReward].RewardByTime;

            return reward;
        }
        
        public void ClaimReward()
        {
            var reward = _rewardsModel.PreparedRewards[0];
            _rewardsModel.PreparedRewards.Remove(reward);
            _indexOfReward++;
            
            SaveLastGiftTime();
        }

        public Reward GetRewardByIndex(int index)
        {
            return _rewardConfig.RewardsByTime[index].RewardByTime;
        }

        private void LoadLastGiftTime()
        {
            if (PlayerPrefs.HasKey(LastRewardTimeKey))
            {
                var lastGiftTimeString = PlayerPrefs.GetString(LastRewardTimeKey);
                
                if (DateTime.TryParse(lastGiftTimeString, out _lastGiftTime))
                {
                    return;
                }
            }
            
            _lastGiftTime = DateTime.MinValue;
        }
        
        private void LoadRewardInCurrentSession()
        {
            if ((DateTime.Today - _lastGiftTime).Days > 0)
            {
                return;
            }
            
            if (PlayerPrefs.HasKey(RewardIndexInCurrentSessionKey))
            {
                _indexOfReward = PlayerPrefs.GetInt(RewardIndexInCurrentSessionKey);
            }
        }
        
        private void SaveLastGiftTime()
        {
            PlayerPrefs.SetInt(RewardIndexInCurrentSessionKey, _indexOfReward);
            PlayerPrefs.SetString(LastRewardTimeKey, DateTime.Today.ToString("o"));
            PlayerPrefs.Save();
        }
        
        private async UniTask UpdateRemainTime()
        {
            for (var index = _indexOfReward; index < _rewardConfig.RewardsByTime.Length; index++)
            {
                var timeBasedReward = _rewardConfig.RewardsByTime[index];
                var countdownTime = TimeSpan.FromMinutes(timeBasedReward.Time);
                var targetTime = DateTime.Now.Add(countdownTime);

                while (DateTime.Now < targetTime)
                {
                    var remainingTime = targetTime - DateTime.Now;
                    RemainTime.Value = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                    await UniTask.Delay(500);
                }

                _rewardsModel.PreparedRewards.Add(index);
                RemainTime.Value = "00:00";
            }
        }
    }
}