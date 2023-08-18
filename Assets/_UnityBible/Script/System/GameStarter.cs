using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameObject _individualObject;
        [SerializeField] private GameObject _canvasStarat;
        [SerializeField] private GameObject _canvasAdduser;
        [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
        [SerializeField] private GameMode _gameMode = GameMode.AutoHostOrClient;
        [SerializeField] private ROOMNAME roomName;
        private NetworkRunner _runnerInstance = null;
        [SerializeField] private AudioSource TitleBGM;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            TitleBGM?.Play();
        }

        public async void StartGame()
        {
            _runnerInstance = Instantiate(_networkRunnerPrefab);

            var startGameArgs = new StartGameArgs()
            {
                SessionName = roomName.ToString(),
                PlayerCount = 20,
                GameMode = _gameMode,
                Scene = 1,
                SceneManager = _runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            };

            _canvasStarat.SetActive(false);
            TitleBGM?.Stop();

#if UNITY_EDITOR
            _individualObject.SetActive(false);
            _canvasAdduser.SetActive(true);
#endif
            await _runnerInstance.StartGame(startGameArgs);
        }

    }

    enum ROOMNAME
    {
        product,
        test
    }
}