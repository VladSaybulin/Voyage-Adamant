using Game;
using UnityEngine;
using Utils;

namespace UI
{
    public class LevelSelectionScene: MonoBehaviour
    {

        [SerializeField] private LevelButton levelButtonPrefab;
        [SerializeField] private LevelDetails levelDetails;
        [SerializeField] private GameObject levelPanel;
        [SerializeField] private Coins coinsPanel; 

        private LevelRepository _levelRepository;

        private void Start()
        {
            var player = GameObject.FindWithTag(Player.Tag).GetComponent<Player>();
            _levelRepository = GameObject.FindWithTag(LevelRepository.Tag).GetComponent<LevelRepository>();
            var levelPanelTransform = levelPanel.GetComponent<Transform>();
            for (int i = 0; i < _levelRepository.CountLevels; i++)
            {
                var levelButton = Instantiate(levelButtonPrefab, levelPanelTransform);
                var levelNumber = i;
                levelButton.SetLevelNumber(levelNumber + 1);
                levelButton.AddListener(() => ShowLevelDetails(levelNumber));
            }
            
            coinsPanel.SetCoins(player.Coins);
        }

        public void OnBackButton()
        {
            SceneChanger.StartMainMenuScene();
        }

        public void OnPlayButtonClick()
        {
            SceneChanger.StartGameScene();
        }

        private void ShowLevelDetails(int numberLevel)
        {
            _levelRepository.LevelNumberSelected = numberLevel;
            levelDetails.Show(_levelRepository.LevelSelected);
        }
    }
}