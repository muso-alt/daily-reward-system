using System;
using UnityEngine;

namespace Muso.DailyReward
{
    [Serializable]
    public class Reward
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _rewardSprite;
        [SerializeField] private bool _giveBigGift;
        
        [SerializeField] private int _coinCount;
        [SerializeField] private int _crystalsCount;

        public string Name => _name;

        public bool GiveBigGift => _giveBigGift;
        public int CoinCount => _coinCount;
        public int CrystalsCount => _crystalsCount;
        public Sprite RewardImage => _rewardSprite;
    }
}