using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Muso.DailyReward.Strategies;

namespace Muso.DailyReward.Interfaces
{
    public interface IRewardStrategy
    {
        AsyncReactiveProperty<string> RemainTime { get; }
        public Reward GetActiveReward();
        public bool CanGetReward();
        public void ClaimReward();
    }
}