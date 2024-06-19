using System;
using UnityEngine;

namespace Runtime.DailyReward
{
    public class DailyRewardObserver
    {
        public int Day { get; private set; }
        
        private readonly TimeSpan _rewardInterval = TimeSpan.FromDays(1);
        
        private const string LastDailyRewardKey = "LastDailyReward";
        private const string DayOneDateKey = "DayOneDate";

        private DateTime _lastRewardTime;
        private DateTime _dayOneDate;
        
        public DailyRewardObserver()
        {
            Initialize();
        }

        private void Initialize()
        {
            _lastRewardTime = GetDateFromPrefs(LastDailyRewardKey);
            _dayOneDate = GetDateFromPrefs(DayOneDateKey);

            //If user skips more than one day, reset dates
            if ((DateTime.Today - _lastRewardTime).Days > 1)
            {
                _lastRewardTime = DateTime.MinValue;
                _dayOneDate = DateTime.Today;
                Save();
            }

            if (_lastRewardTime != DateTime.MinValue && _dayOneDate != DateTime.MinValue)
            {
                Day = (_lastRewardTime - _dayOneDate).Days;
            }
        }

        public void ClaimDailyReward()
        {
            _lastRewardTime = DateTime.Now;
            
            if (_dayOneDate == DateTime.MinValue)
            {
                _dayOneDate = DateTime.Today;
            }
            
            Save();
        }
        
        public bool CanReceiveDailyReward()
        {
            return (DateTime.Now - _lastRewardTime) >= _rewardInterval;
        }

        private void Save()
        {
            SaveLastClaimedRewardDate();
            SaveDayOneDate();
        }
        
        private DateTime GetDateFromPrefs(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return DateTime.MinValue;
            }
            
            var value = PlayerPrefs.GetString(key);
            return DateTime.TryParse(value, out var date) ? date : DateTime.MinValue;
        }
        
        private void SaveLastClaimedRewardDate()
        {
            var dateTime = _lastRewardTime.ToString("o");
            PlayerPrefs.SetString(LastDailyRewardKey, dateTime);
            PlayerPrefs.Save();
        }

        private void SaveDayOneDate()
        {
            var dateTime = _dayOneDate.ToString("o");
            PlayerPrefs.SetString(DayOneDateKey, dateTime);
            PlayerPrefs.Save();
        }
    }
}