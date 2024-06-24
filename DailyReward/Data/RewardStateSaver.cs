using System;
using UnityEngine;

namespace Muso.DailyReward.Data
{
    public class RewardStateSaver 
    {
        public DateTime LastRewardTime { get; set; }
        public DateTime DayOneDate { get; set; }
        
        private const string LastDailyRewardKey = "LastDailyReward";
        private const string DayOneDateKey = "DayOneDate";

        public void Init()
        {
            LastRewardTime = GetDateFromPrefs(LastDailyRewardKey);
            DayOneDate = GetDateFromPrefs(DayOneDateKey);
        }
        
        public void Save()
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
            var dateTime = LastRewardTime.ToString("o");
            PlayerPrefs.SetString(LastDailyRewardKey, dateTime);
            PlayerPrefs.Save();
        }

        private void SaveDayOneDate()
        {
            var dateTime = DayOneDate.ToString("o");
            PlayerPrefs.SetString(DayOneDateKey, dateTime);
            PlayerPrefs.Save();
        }
    }
}