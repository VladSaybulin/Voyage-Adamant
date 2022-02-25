using System;
using Game;
using UnityEditor;
using UnityEngine;
using Utils;

public class Starter: MonoBehaviour
{

    [SerializeField] private GameObject levelRepositoryGameObject;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject audioTrackGameObjet;

    private void Start()
    {
        DontDestroyOnLoad(levelRepositoryGameObject);
        DontDestroyOnLoad(playerGameObject);
        DontDestroyOnLoad(audioTrackGameObjet);
        SceneChanger.StartMainMenuScene();
    }
}