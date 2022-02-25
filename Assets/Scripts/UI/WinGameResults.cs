using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinGameResults : MonoBehaviour
    {
        [SerializeField] private Text coinsResult;
        
        public void Init(int levelCoins, int timeCoins)
        {
            coinsResult.text = $"Монеты за прохождение: {levelCoins}\nМонеты за время: {timeCoins}\nИтого: {levelCoins + timeCoins}";
        }

    }
}