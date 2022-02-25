using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelRepository: MonoBehaviour
    {

        public const string Tag = "LevelRepository";

        [SerializeField] private List<Level> levels;
        private int _levelNumberSelected;

        public int LevelNumberSelected
        {
            get => _levelNumberSelected;
            set
            {
                if(!HasLevel(value)) 
                    throw new Exception("numberLevel out of range");
                _levelNumberSelected = value;
            }
        }

        public Level LevelSelected => levels[_levelNumberSelected];

        public int CountLevels => levels.Count;

        private void Awake()
        {
            tag = Tag;

            for (int i = 0; i < levels.Count; i++)
                levels[i].Init(i + 1);

        }

        public bool HasLevel(int levelNumber) => levelNumber < levels.Count;

        public Level GetLevel(int levelNumber) => HasLevel(levelNumber) ? levels[levelNumber] : null;
        
    }
}