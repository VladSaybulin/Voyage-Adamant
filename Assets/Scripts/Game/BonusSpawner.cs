using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
    public class BonusSpawner : MonoBehaviour
    {

        public readonly UnityEvent<BonusEvent, Bonus> BonusEvents = new UnityEvent<BonusEvent, Bonus>();

        private Transform _transform;

        private IEnumerator _spawnRoutine;
        
        private Bounds _spawnBounds;
        private Level _level;
        
        private readonly List<Bonus> _bonuses = new List<Bonus>();
        private List<Bonus> _prefabs;
        private List<int> _chances;

        public Level Level
        {
            set => _level = value;
        }
        
        public Bounds SpawnBounds
        {
            set => _spawnBounds = value;
        }
        
        private void Start()
        {
            
            _transform = GetComponent<Transform>();

            _prefabs = new List<Bonus>(_level.bonusParamsList.Count + 1);
            _chances = new List<int>(_level.bonusParamsList.Count + 1);
            foreach (var bp in _level.bonusParamsList)
            {
                _prefabs.Add(bp.prefab);
                _chances.Add(bp.chance);
            }
            _prefabs.Add(null);
            _chances.Add(_level.nullBonusChance);
            
            //SortChances();

            _spawnRoutine = SpawnRoutine();
        }

        public void StartSpawn()
        {
            StartCoroutine(_spawnRoutine);
        }

        public void ResetSpawn()
        {
            StopCoroutine(_spawnRoutine);
            _bonuses.ForEach(bonus =>
            { 
                bonus.Events.RemoveListener(BonusEvents.Invoke); 
                bonus.Activate();
            });
        }

        private void SpawnBonus(Bonus bonus)
        {
            var b = Instantiate(bonus, _transform);
            b.Events.AddListener(BonusEvents.Invoke);
            b.Events.AddListener((e, b) =>
            {
                if (e == BonusEvent.Finished) 
                    Destroy(b.gameObject);
            });
            b.Scale = _level.entityScale;
            b.SpawnBounds = _spawnBounds;
            b.DurationMove = Random.Range(_level.minDurationMove, _level.maxDurationMove);
            b.StartMoving();
            _bonuses.Add(b);
        }

        private void SortChances()
        {
            for (int i = 0; i < _chances.Count; i++)
            {
                int index = i;
                int max = _chances[index];
                for (int j = i + 1; j < _chances.Count; j++)
                {
                    if (max < _chances[j])
                    {
                        index = j;
                        max = _chances[index];
                    }
                }
                
                (_chances[i], _chances[index]) = (_chances[index], _chances[i]);
                (_prefabs[i], _prefabs[index]) = (_prefabs[index], _prefabs[i]);
            }
        }

        private IEnumerator SpawnRoutine()
        {
            var wait = new WaitForSeconds(1);
            while (true)
            {
                var bonus = _prefabs[RandomUtility.Chance(_chances)];
                if (bonus != null)
                    SpawnBonus(bonus);
                yield return wait;
            }
        }
    }
}