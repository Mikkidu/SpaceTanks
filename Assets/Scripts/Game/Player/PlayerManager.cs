using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class PlayerManager : MonoBehaviour
    {
        private static int _coinsValue;

        public delegate void OnCoinsChange(int coinsAmount);
        public static OnCoinsChange onCoinsChangeEvent;

        public static void AddCoins(int amount)
        {
            if (amount > 0)
            {
                _coinsValue += amount;
                if (onCoinsChangeEvent != null)
                    onCoinsChangeEvent.Invoke(_coinsValue);
            }
        }

        public static void SpendCoins(int price)
        {
            if (_coinsValue >= price)
            {
                AddCoins(-price);
            }
        }
    }
}
