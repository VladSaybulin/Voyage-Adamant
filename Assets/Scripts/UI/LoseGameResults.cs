using System.Collections.Generic;
using Game;
using UnityEngine;
using Utils;

namespace UI
{
    public class LoseGameResults : MonoBehaviour
    {
        [SerializeField] private NeedCollectDiamonds needCollectDiamonds;

        public void Init(List<Level.DiamondParams> diamondParamsList, List<int> notCollected)
        {
            var diamondParams = new List<Pair<int, Color>>();

            for (int i = 0; i < diamondParamsList.Count; i++)
            {
                if (notCollected[i] > 0)
                {
                    diamondParams.Add(new Pair<int, Color>(i, diamondParamsList[i].color));
                }
            }
            
            needCollectDiamonds.Init(diamondParamsList.Count, diamondParams);
            
            for (int i = 0; i < notCollected.Count; i++)
                needCollectDiamonds.SetAmount(i, notCollected[i]);
                    
        }
    }
}