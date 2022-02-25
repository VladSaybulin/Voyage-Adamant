using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game
{
    public class Game : MonoBehaviour
    {

        public enum GameState { Starting, InGame, Pause, GameOver }
        
        [SerializeField] private float offsetSpawn;

        [SerializeField] private DiamondSpawner diamondSpawner;
        [SerializeField] private BonusSpawner bonusSpawner;
        [SerializeField] private UIManager uiManager;


        private Coroutine _countdownCoroutine;

        private Level _level;
        private GameState _state = GameState.Starting;

        private float _time;
        private List<int> _needCollectDiamonds;
        
        private void Start()
        {
            _level = GameObject.FindWithTag(LevelRepository.Tag).GetComponent<LevelRepository>().LevelSelected;
            var spawnBounds = GetSpawnBounds();
            
            diamondSpawner.Level = _level;
            diamondSpawner.SpawnBounds = spawnBounds;
            diamondSpawner.DiamondEvents.AddListener(HandleDiamondEvents);

            bonusSpawner.Level = _level;
            bonusSpawner.SpawnBounds = spawnBounds;
            bonusSpawner.BonusEvents.AddListener(HandleBonusEvents);

            uiManager.Level = _level;
            uiManager.SetCenterText(_level.Number + " уровень");

            _needCollectDiamonds = new List<int>(_level.diamondParamsList.Count);
            InitLevel();
            
            _countdownCoroutine = StartCoroutine(CountdownAndStartGameRoutine());
        }

        private void Update()
        {
            if (_state == GameState.InGame)
            {
                _time -= Time.deltaTime;
                if (_time <= 0)
                {
                    _time = 0;
                    Lose();
                }
            }

            uiManager.SetTimer(_time);
        }

        public void Exit()
        {
            SceneChanger.StartMainMenuScene();
        }

        public void Pause()
        {
            if(_state == GameState.GameOver) return;
            if (_state == GameState.Starting)
            {
                diamondSpawner.StartSpawn();
                bonusSpawner.StartSpawn();
            }
            
            StopCountdownRoutine();
            SetGameState(GameState.Pause);
            uiManager.SetActivePauseMenu(true);
            uiManager.SetCenterText("");
        }

        public void Resume()
        {
            uiManager.SetActivePauseMenu(false);
            _countdownCoroutine = StartCoroutine(CountdownAndResumeGameRoutine());
        }

        public void Restart()
        {
            if(_state == GameState.GameOver) return;
            if (_state == GameState.Starting)
                Resume();

            SetGameState(GameState.Starting);
            GameOver();
            InitLevel();
            
            for (int i = 0; i < _needCollectDiamonds.Count; i++)
                uiManager.SetAmountDiamonds(i, _needCollectDiamonds[i]);

            _countdownCoroutine = StartCoroutine(CountdownAndStartGameRoutine());
        }

        private void InitLevel()
        {
            _time = _level.time;
            _needCollectDiamonds.Clear();
            for (int i = 0; i < _level.diamondParamsList.Count; i++)
            {
                _needCollectDiamonds.Add(_level.diamondParamsList[i].needCollect);
            }
        }

        private void Lose()
        {
            GameOver();
            uiManager.SetCenterText("Время вышло");

            var gameResultsGameObject = new GameObject("GameResults");
            var gameResults = gameResultsGameObject.AddComponent<GameResult>();
            gameResults.Lose(_needCollectDiamonds);
            DontDestroyOnLoad(gameResultsGameObject);
            StartCoroutine(DelayedTask(3, SceneChanger.StartGameResultsScene));
        }

        private void Win()
        {
            GameOver();
            uiManager.SetCenterText("Уровень пройден!");

            var gameResultsGameObject = new GameObject("GameResults");
            var gameResults = gameResultsGameObject.AddComponent<GameResult>();
            gameResults.Win(_time);
            DontDestroyOnLoad(gameResultsGameObject);
            StartCoroutine(DelayedTask(3, SceneChanger.StartGameResultsScene));
        }

        private void GameOver()
        {
            diamondSpawner.ResetSpawn();
            bonusSpawner.ResetSpawn();
            SetGameState(GameState.GameOver);
        }

        private void SetGameState(GameState newState)
        {
            print($"Change state. {_state.ToString()} -> {newState.ToString()}");
            _state = newState;
            Time.timeScale = _state == GameState.Pause ? 0 : 1;
        }

        private Bounds GetSpawnBounds()
        {
            var cam = Camera.main;
            var height = cam.orthographicSize * 2;
            var spawnSize = new Vector3(height * cam.aspect + offsetSpawn, height + offsetSpawn, 0);
            return new Bounds(cam.transform.position, spawnSize);
        }

        private void StopCountdownRoutine()
        {
            if (_countdownCoroutine == null) return;
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        private void HandleDiamondEvents(DiamondEvent diamondEvent, Diamond instance)
        {
            if (_state != GameState.InGame) return;
            switch (diamondEvent)
            {
                case DiamondEvent.Clicked: HandleOnClickDiamond(instance);
                    break;
                case DiamondEvent.Collected: HandleOnCollectDiamond(instance);
                    break;
            }
        }

        private void HandleOnClickDiamond(Diamond instance)
        {
            instance.TakeDamage(1);
        }

        private void HandleOnCollectDiamond(Diamond instance)
        {
            var index = instance.Index;

            if (_needCollectDiamonds[index] > 0)
            {
                _needCollectDiamonds[index]--;
                if (_needCollectDiamonds.Sum() == 0)
                    Win();
                
                uiManager.SetAmountDiamonds(index, _needCollectDiamonds[index]);
            }
        }
        
        private void HandleBonusEvents(BonusEvent bonusEvent, Bonus instance)
        {
            if (_state != GameState.InGame) return;
            if(bonusEvent == BonusEvent.Clicked)
                instance.Activate();
        }

        private IEnumerator DelayedTask(float seconds, UnityAction task)
        {
            yield return new WaitForSecondsRealtime(seconds);
            task.Invoke();
        }
        
        private IEnumerator CountdownAndStartGameRoutine()
        {
            var wait = new WaitForSecondsRealtime(1);
            
            yield return wait;

            for (int i = 3; i > 0; i--)
            {
                uiManager.SetCenterText(i.ToString());
                yield return wait;
            }

            uiManager.SetCenterText("Старт");
            
            diamondSpawner.StartSpawn();
            bonusSpawner.StartSpawn();
            SetGameState(GameState.InGame);
            
            yield return wait;
            
            uiManager.SetCenterText("");
            _countdownCoroutine = null;
        }
        
        private IEnumerator CountdownAndResumeGameRoutine()
        {
            var wait = new WaitForSecondsRealtime(1);
            
            for (int i = 3; i > 0; i--)
            {
                uiManager.SetCenterText(i.ToString());
                yield return wait;
            }

            SetGameState(GameState.InGame);
            
            uiManager.SetCenterText("");
            _countdownCoroutine = null;
        }
        
    }
}