using Game;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class GameResultScene : MonoBehaviour
    {

        [SerializeField] private WinGameResults winPanel;
        [SerializeField] private LoseGameResults losePanel;
        [SerializeField] private GameObject nextLevelButton;
        [SerializeField] private Text numberLevel;
        [SerializeField] private Coins coinsPanel;

        private LevelRepository _levelRepository;
        
        private void Start()
        {
            _levelRepository = GameObject.FindWithTag(LevelRepository.Tag).GetComponent<LevelRepository>();
            var gameResults = GameObject.FindWithTag(GameResult.Tag).GetComponent<GameResult>();
            var player = GameObject.FindWithTag(Player.Tag).GetComponent<Player>();

            numberLevel.text = $"Уровень {_levelRepository.LevelSelected.Number}";
            
            if (gameResults.IsWin)
            {
                var coins = 100 + (int) gameResults.Time;
                player.Coins += coins;
                
                nextLevelButton.SetActive(_levelRepository.HasLevel(_levelRepository.LevelNumberSelected + 1));
                winPanel.gameObject.SetActive(true);
                winPanel.Init(100, (int) gameResults.Time);
            }
            else
            {
                nextLevelButton.SetActive(false);
                losePanel.gameObject.SetActive(true);
                losePanel.Init(_levelRepository.LevelSelected.diamondParamsList, gameResults.NotCollected);
            }
            
            coinsPanel.SetCoins(player.Coins);

            Destroy(gameResults.gameObject);
        }

        public void OnNextLevel()
        {
            _levelRepository.LevelNumberSelected++; 
            SceneChanger.StartGameScene();
        }

        public void OnRestartLevel()
        {
            SceneChanger.StartGameScene();
        }

        public void ToMainMenu()
        {
            SceneChanger.StartMainMenuScene();
        }
    }
}