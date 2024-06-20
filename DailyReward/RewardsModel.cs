﻿using System.Collections.Generic;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Observers
{
    public class RewardsModel
    {
        public readonly Dictionary<int, RewardState> RewardsByState = new();
        public readonly List<int> PreparedRewards = new();
    }
}