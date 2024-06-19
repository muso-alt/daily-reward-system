using System;
using UnityEngine;

namespace Muso.DailyReward.Interfaces
{
    [Serializable]
    public class Reward
    {
        [SerializeField] private string _name;

        public string Name => _name;
    }
}