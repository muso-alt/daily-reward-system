using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Muso.DailyReward.Interfaces;
using UnityEngine;

namespace Muso.DailyReward.Observers
{
    public class RewardByTimeObserver
    {
        public string RemainTime { get; private set; }
        
        private const string LastGiftTimeKey = "LastGiftTime";
        
        private readonly RewardConfig _rewardConfig;
        private readonly List<int> _preparedRewards;
        
        private DateTime _lastGiftTime;
        private float _time;
        
        public RewardByTimeObserver(RewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
            _preparedRewards = new List<int>();
            
            Initialize();
        }

        private void Initialize()
        {
            LoadLastGiftTime();
            
            var now = DateTime.Today;

            if ((now - _lastGiftTime).TotalDays >= 1)
            {
                PrepareGiftsAsync().Forget();
            }
        }

        public bool CanReceiveReward()
        {
            return _preparedRewards.Count > 0;
        }

        public int GetActiveRewardIndex()
        {
            return _preparedRewards[0];
        }
        
        public void ClaimReward()
        {
            var reward = _preparedRewards[0];
            
            SaveLastGiftTime();
            _preparedRewards.Remove(reward);
        }
        
        private void LoadLastGiftTime()
        {
            if (PlayerPrefs.HasKey(LastGiftTimeKey))
            {
                var lastGiftTimeString = PlayerPrefs.GetString(LastGiftTimeKey);
                if (DateTime.TryParse(lastGiftTimeString, out _lastGiftTime))
                {
                    return;
                }
            }
            
            _lastGiftTime = DateTime.MinValue;
        }
        
        private void SaveLastGiftTime()
        {
            PlayerPrefs.SetString(LastGiftTimeKey, DateTime.Today.ToString("o"));
            PlayerPrefs.Save();
        }
        
        private async UniTask PrepareGiftsAsync()
        {
            for (var index = 0; index < _rewardConfig.RewardsByTime.Length; index++)
            {
                var timeBasedReward = _rewardConfig.RewardsByTime[index];
                
                var countdownTime = TimeSpan.FromMinutes(timeBasedReward.Time);
                var targetTime = DateTime.Now.Add(countdownTime);

                while (DateTime.Now < targetTime)
                {
                    TimeSpan remainingTime = targetTime - DateTime.Now;
                    RemainTime = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                    await UniTask.Delay(500);
                }

                RemainTime = "00:00";
                _preparedRewards.Add(index);
            }
        }
    }
}