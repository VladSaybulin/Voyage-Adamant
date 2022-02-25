using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameResult : MonoBehaviour
    {
        public const string Tag = "GameResults";
        
        public bool IsWin { get; private set; }
        public float Time { get; private set; }
        public List<int> NotCollected { get; private set;  }


        private void Awake()
        {
            tag = Tag;
        }

        public void Win(float time)
        {
            Time = time;
            IsWin = true;
        }

        public void Lose(List<int> notCollected)
        {
            IsWin = false;
            NotCollected = notCollected;
        }
        
    }
}