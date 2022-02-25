using System;
using System.Linq;
using Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LevelDetails: MonoBehaviour
    {

        [SerializeField] private Text levelNumber;
        [SerializeField] private Timer timer;
        [SerializeField] private NeedCollectDiamonds needCollectDiamonds;
        

        public void Show(Level level)
        {
            gameObject.SetActive(true);
            var needCollectParams = level.diamondParamsList
                .Where(dp => dp.needCollect > 0)
                .Select(dp => new Pair<int, Color>(dp.Index, dp.color))
                .ToList();
            needCollectDiamonds.Init(level.diamondParamsList.Count, needCollectParams);
            foreach (var dp in level.diamondParamsList)
                needCollectDiamonds.SetAmount(dp.Index, dp.needCollect);
            
            timer.SetTime(level.time);
            levelNumber.text = $"{level.Number} уровень";
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            needCollectDiamonds.Clear();
        }

    }
}