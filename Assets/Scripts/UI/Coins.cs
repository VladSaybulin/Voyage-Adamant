using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Coins: MonoBehaviour
    {

        [SerializeField] private Text coinsText;

        public void SetCoins(int coins)
        {
            coinsText.text = coins.ToString();
        }

    }
}