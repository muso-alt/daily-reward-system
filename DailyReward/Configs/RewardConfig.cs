using System;
using Muso.DailyReward.Interfaces;
using UnityEngine;

namespace Muso.DailyReward
{
    [CreateAssetMenu(fileName = nameof(RewardConfig), menuName = nameof(Muso) + "/" + nameof(RewardConfig),
        order = 0)]
    public class RewardConfig : ScriptableObject
    {
        [SerializeField] private Reward[] _dailyRewards;
        [SerializeField] private TimeBasedReward[] _rewardsByTime;

        public Reward[] DailyRewards => _dailyRewards;
        public TimeBasedReward[] RewardsByTime => _rewardsByTime;
    }

    [Serializable]
    public struct TimeBasedReward
    {
        [SerializeField] private int _time;
        [SerializeField] private Reward _reward;

        public int Time => _time;
        public Reward RewardByTime => _reward;
    }
}