using System;
using UnityEngine;

public class Player: MonoBehaviour
{
    public const string Tag = "PlayerData";
    private const string CoinsPrefName = "coins";
        
    private int _coins;

    public int Coins
    {
        get => _coins;
        set => _coins = value;
    } 
    private void Awake()
    {
        tag = Tag;
        _coins = PlayerPrefs.GetInt(CoinsPrefName, 0);
    }

    public void AddCoins(int coins)
    {
        _coins += coins;
        PlayerPrefs.SetInt(CoinsPrefName, _coins);
        PlayerPrefs.Save();
    }
}