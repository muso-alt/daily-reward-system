using UnityEngine;

namespace Muso.DailyReward
{
    public class Reward : MonoBehaviour
    {
        [SerializeField] private string _name;

        public string Name => _name;
    }
}