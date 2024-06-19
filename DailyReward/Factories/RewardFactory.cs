using Muso.DailyReward.Interfaces;

namespace Muso.DailyReward.Factories
{
    public abstract class RewardFactory
    {
        public abstract IRewardStrategy CreateRewardStrategy();
    }
}