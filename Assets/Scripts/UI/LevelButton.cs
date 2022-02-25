using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class LevelButton: MonoBehaviour
    {
        [SerializeField] private Text levelNumberText;

        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetLevelNumber(int levelNumber) => levelNumberText.text = levelNumber.ToString();

        public void AddListener(UnityAction action) => _button.onClick.AddListener(action);
    }
}