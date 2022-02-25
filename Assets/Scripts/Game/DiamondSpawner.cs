using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
    public class DiamondSpawner: MonoBehaviour
    {

        public readonly UnityEvent<DiamondEvent, Diamond> DiamondEvents = new UnityEvent<DiamondEvent, Diamond>();

        [SerializeField] private Diamond diamondPrefab;

        private Transform _transform;

        private Bounds _spawnBounds;
        private Level _level;
        private List<int> _chances;
        private List<Level.DiamondParams> _diamondParamsList;
        private List<Diamond> _diamonds;


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

            _diamondParamsList = new List<Level.DiamondParams>(_level.diamondParamsList.Count);
            _chances = new List<int>(_diamondParamsList.Count);
            _level.diamondParamsList.ForEach(d =>
            {
                _diamondParamsList.Add(d);
                _chances.Add(d.chance);
            });

            _diamonds = new List<Diamond>(_level.amountDiamonds);
            for (int i = 0; i < _level.amountDiamonds; i++)
            {
                var diamond = Instantiate(diamondPrefab, _transform);
                diamond.Scale = _level.entityScale;
                diamond.SpawnBounds = _spawnBounds;
                diamond.transform.position = _spawnBounds.min;
                _diamonds.Add(diamond);
            }
        }

        public void StartSpawn()
        {
            foreach (var diamond in _diamonds)
            {
                diamond.Events.AddListener(DiamondEvents.Invoke);
                diamond.Events.AddListener((e, d) =>
                {
                    if(e == DiamondEvent.Finished)
                        SpawnDiamond(d);
                });
                SpawnDiamond(diamond);
            }
        }

        public void ResetSpawn()
        {
            _diamonds.ForEach(diamond =>
            {
                diamond.Events.RemoveAllListeners();
                diamond.TakeDamage(99);
            });
        }
        
        private void SortChances()
        {
            for (int i = 0; i < _chances.Count; i++)
            {
                int minIndex = i;
                int min = _chances[minIndex];
                for (int j = i + 1; j < _chances.Count; j++)
                {
                    if (min > _chances[j])
                    {
                        minIndex = j;
                        min = _chances[minIndex];
                    }
                }
                
                (_chances[i], _chances[minIndex]) = (_chances[minIndex], _chances[i]);
                (_diamondParamsList[i], _diamondParamsList[minIndex]) = (_diamondParamsList[minIndex], _diamondParamsList[i]);
            }
        }

        private void SpawnDiamond(Diamond diamond)
        { 
            int index = RandomUtility.Chance(_chances);
            var diamondParams = _diamondParamsList[index];
            diamond.Color = diamondParams.color;
            diamond.Hp = diamondParams.hp;
            diamond.Index = diamondParams.Index;
            diamond.DurationMove = Random.Range(_level.minDurationMove, _level.maxDurationMove);
            diamond.StartMoving();
        }
    }
}