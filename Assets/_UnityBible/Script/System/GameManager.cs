using Cinemachine;
using ExitGames.Client.Photon;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UnityBibleSample
{

    public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks,ISpawned
    {

        [SerializeField] private NetworkObject _playerPrefab;
        [SerializeField] private GoalManager _goalManagerR;
        [SerializeField] private GoalManager _goalManagerB;

        //[Networked] private List<NetworkRunner> networkRunners { get; set; }
        //[Networked, Capacity(40)] NetworkLinkedList<PlayerInitializer> playerInitializers { get; }
        //[Networked, Capacity(4)] NetworkLinkedList<PlayerInitializer> playerInitializers { get; } = MakeInitializer( new PlayerInitializer[] { null, null, null, null });
        //[Networked, Capacity(4)] NetworkLinkedList<int> counts { get; } = MakeInitializer(new int[] { 0, 0, 0, 0 });
        //[Networked, Capacity(4)] NetworkLinkedList<int> counts => default;
        //[Networked, Capacity(40)] NetworkLinkedList<Int16> counts { get; }

        //[Networked] private int teamRCount { get; set; }
        //[Networked] private int teamBCount { get; set; }

        //private float innerRadius = 6f;
        //private float outerRadius = 10f;

        [Networked] private TickTimer gameTimer { get; set; }
        [SerializeField] private float gameTime;
        [Networked] private TickTimer intervalTimer { get; set; }
        [Networked] private bool inInterval { get; set; }
        [SerializeField] private float gameInterval;
        [SerializeField] private TextMeshProUGUI TMPTime;
        [SerializeField] private TextMeshProUGUI TMPResult;
        [SerializeField] private AudioSource JingleWin;
        [SerializeField] private AudioSource JingleLose;

        public override void Spawned()
        {
            inInterval = false;
            _goalManagerR.inGame = true;
            _goalManagerB.inGame = true;
            if (gameTimer.ExpiredOrNotRunning(Runner)) gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);

            //if(playerInitializers == null) playerInitializers = new List<PlayerInitializer>();

            //OnPlayerJoined(Runner, Runner.LocalPlayer);
        }

        public override void FixedUpdateNetwork()
        {
            UpdateCountTxt();

            if (gameTimer.ExpiredOrNotRunning(Runner)) TimeUpSquense();
        }

        private void TimeUpSquense()
        {
            if (inInterval)
            {
                if (intervalTimer.ExpiredOrNotRunning(Runner) == false) return;

                gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);
                inInterval = false;
                _goalManagerR.inGame = true;
                _goalManagerB.inGame = true;
                TMPResult.text = "";
            }
            else
            {
                intervalTimer = TickTimer.CreateFromSeconds(Runner, gameInterval);
                inInterval = true;
                _goalManagerR.inGame = false;
                _goalManagerB.inGame = false;
                TMPTime.text = "0";

                var scoreR = _goalManagerR.TeamScore;
                var scoreB = _goalManagerB.TeamScore;
                var resultTxt = "";
                if (scoreR == scoreB)
                {
                    resultTxt = "Draw Game";
                }
                else if (scoreR > scoreB)
                {
                    resultTxt = "Red Team Win!";
                }
                else if (scoreR < scoreB)
                {
                    resultTxt = "Blue Team Win!";
                }
                TMPResult.text = resultTxt;

                _goalManagerR.ResetScore();
                _goalManagerB.ResetScore();
            }
        }

        private void UpdateCountTxt()
        {
            var time = gameTimer.RemainingTime(Runner);
            if (time == null || time == 0f) return;
            TMPTime.text = Mathf.FloorToInt((float)time  + 1).ToString();
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            int team = 0;

            //if (playerInitializers.Count != 0)
            //{
            //    Debug.Log("playerInitializers " + playerInitializers.First(),gameObject);
            //    var team0 = playerInitializers.Where(x => x.teamIndex == 0)?.Count();
            //    var team1 = playerInitializers.Where(x => x.teamIndex == 1)?.Count();
            //    //networkRunners.Add(runner);
            //    //var team0 = networkRunners.Where(x => x.GetComponent<PlayerInitializer>().teamIndex == 0).Count();
            //    //var team1 = networkRunners.Where(x => x.GetComponent<PlayerInitializer>().teamIndex == 1).Count();
            //    if (team0 > team1) team = 1;
            //}

            //    counts.Add((short)team);
            //foreach (var item in counts)
            //{
            //    Debug.Log("counts " + counts.Count(),gameObject);

            //}

            //if (teamBCount > teamRCount) team = 1;
            
            //if (runner.IsClient == false && _playerPrefab != null)
            //if (Runner.LocalPlayer == player && _playerPrefab != null) // shared modeiunity1weekj—p
            //{
            //    Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
                
            //    float randomDistance = UnityEngine.Random.Range(innerRadius, outerRadius);
                
            //    Vector3 randomPosition = randomDirection * randomDistance;
            //    randomPosition.y = 0;
            //    randomPosition.z += team;// * 40 - 20;

            //    var character = runner.Spawn(_playerPrefab, randomPosition, Quaternion.LookRotation(Vector3.zero), inputAuthority: player,
            //            (Runner, no) => no.GetComponent<PlayerInitializer>().Initialize(team)
            //        );

            //    //Debug.Log("itemcounts " + playerInitializers.Count(), gameObject);
            //    //playerInitializers.Add(character.GetComponent<PlayerInitializer>());

            //    if (team == 0) teamBCount++;
            //    if (team == 1) teamRCount++;

            //    runner.SetPlayerObject(player, character);
                
            //    Log.Info($"Spawn for Player: {player}");
            //}
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("OnPlayerLeft");

            //if (_playerMap.TryGetValue(player, out var character))
            //{
            //    runner.Despawn(character);
            //    _playerMap.Remove(player);
            //    Log.Info($"Despawn for Player: {player}");
            //}

            //if (networkRunners.Contains(runner))
            //{
            //    runner.Despawn(runner.GetPlayerObject(player));
            //    networkRunners.Remove(runner);
            //    Log.Info($"Despawn for Player: {player}");
            //}
        }

        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log("OnShutdown");
            Application.Quit(0);
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    }
}