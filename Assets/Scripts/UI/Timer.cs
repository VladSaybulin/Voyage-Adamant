using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Timer: MonoBehaviour
    {

        [SerializeField] private Text text;

        public void SetTime(float time)
        {
            var t = (int)Mathf.Round(time);
            text.text = $"{t / 60:D}:{t % 60:D2}";
        }

    }
}