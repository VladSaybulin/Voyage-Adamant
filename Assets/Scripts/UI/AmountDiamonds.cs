using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmountDiamonds : MonoBehaviour
    {

        [SerializeField] private Text amount;
        [SerializeField] private Image image;

        public int Amount
        {
            set => amount.text = value.ToString();
        }

        public Color Color
        {
            set => image.color = value;
        }
        

    }
}