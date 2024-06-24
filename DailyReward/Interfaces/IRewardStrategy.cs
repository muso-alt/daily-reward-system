using Cysharp.Threading.Tasks;

namespace Muso.DailyReward.Interfaces
{
    public interface IRewardStrategy
    {
        AsyncReactiveProperty<string> RemainTime { get; }
        public Reward GetActiveReward();
        public bool CanGetReward();
        public void ClaimReward();
        Reward GetRewardByIndex(int index);
    }
}