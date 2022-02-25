using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class Level: ScriptableObject
    {
        [Serializable]
        public struct DiamondParams
        {
            public Color color;
            public int hp;
            public int needCollect;
            public int chance;
            [NonSerialized] public int Index;
        }

        [Serializable]
        public struct BonusParams
        {
            public Bonus prefab;
            public int chance;
        }

        public float time;
        public int amountDiamonds;
        public int nullBonusChance;
        
        [Header("Common")] 
        public float entityScale;
        public float minDurationMove;
        public float maxDurationMove;

        public List<DiamondParams> diamondParamsList;
        [Space] public List<BonusParams> bonusParamsList;

        public int Number { get; private set; }

        public void Init(int number)
        {
            Number = number;
            for (int i = 0; i < diamondParamsList.Count; i++)
            {
                var dp = diamondParamsList[i];
                dp.Index = i;
                diamondParamsList[i] = dp;
            }
        }
    }
}