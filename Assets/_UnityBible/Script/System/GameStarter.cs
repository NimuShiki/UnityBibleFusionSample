using System.Collections.Generic;
using UnityEngine;
using Fusion.Sockets;
using System;
using Fusion;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;
using UnityEngine.SceneManagement;

namespace UnityBibleSample
{

    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameObject _individualObject;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
        [SerializeField] private GameMode _gameMode = GameMode.AutoHostOrClient;
        [SerializeField] private ROOMNAME roomName;
        private NetworkRunner _runnerInstance = null;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            _runnerInstance = FindObjectOfType<NetworkRunner>();
            if (_runnerInstance != null) return;
            
            StartGame();
        }

        public async void StartGame()
        {
            _runnerInstance = Instantiate(_networkRunnerPrefab);

            var startGameArgs = new StartGameArgs()
            {
                SessionName = roomName.ToString(),
                GameMode = _gameMode,
                Scene = 1,
                SceneManager = _runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            };

            await _runnerInstance.StartGame(startGameArgs);

#if UNITY_EDITOR
            _individualObject.SetActive(false);
            _canvas.SetActive(true);
#else
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
#endif
        }

    }

    enum ROOMNAME
    {
        product,
        test
    }
}