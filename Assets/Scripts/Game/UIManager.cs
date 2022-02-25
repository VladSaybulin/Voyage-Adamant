using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using GameState = Game.Game.GameState;

namespace Game
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private NeedCollectDiamonds needCollectDiamonds;
        [SerializeField] private Timer timer;
        [SerializeField] private Text centerText;
        [SerializeField] private GameObject pauseMenu;


        private Level _level;
        
        public Level Level
        {
            set => _level = value;
        }
        
        private void Start()
        {
            var needCollectParams = _level.diamondParamsList
                .Where(dp => dp.needCollect > 0)
                .Select(dp => new Pair<int, Color>(dp.Index, dp.color))
                .ToList();
            needCollectDiamonds.Init(_level.diamondParamsList.Count, needCollectParams);
            foreach (var dp in _level.diamondParamsList)
                SetAmountDiamonds(dp.Index, dp.needCollect);
        }

        public void SetActivePauseMenu(bool active) => pauseMenu.SetActive(active);

        public void SetTimer(float time)
        {
            timer.SetTime(time);
        }

        public void SetAmountDiamonds(int index, int amount) => needCollectDiamonds.SetAmount(index, amount);

        public void SetCenterText(string text) => centerText.text = text;

    }
}