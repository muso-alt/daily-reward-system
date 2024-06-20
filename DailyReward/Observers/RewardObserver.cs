using System.Collections.Generic;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Observers
{
    public class RewardObserver
    {
        private readonly Dictionary<int, RewardState> _rewardsByState = new();
        
        public Dictionary<int, RewardState> GetRewardsByState()
        {
            return _rewardsByState;
        }
    }
}