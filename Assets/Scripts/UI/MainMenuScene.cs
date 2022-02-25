using System;
using UnityEngine;
using Utils;

namespace UI
{
    public class MainMenuScene: MonoBehaviour
    {

        [SerializeField] private Coins coinsPanel; 
        
        private void Start()
        {
            var player = GameObject.FindWithTag(Player.Tag).GetComponent<Player>();
            coinsPanel.SetCoins(player.Coins);
        }

        public void OnExitButtonClick()
        {
            SceneChanger.Quit();
        }
        
        public void OnPlayButtonClick()
        {
            SceneChanger.StartLevelSelection();
        }
        
    }
}