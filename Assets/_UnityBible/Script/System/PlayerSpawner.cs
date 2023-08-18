using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBibleSample
{
    public class PlayerSpawner : SimulationBehaviour, INetworkRunnerCallbacks
    {

        [SerializeField] private NetworkObject _playerPrefab;
        private readonly Dictionary<PlayerRef, NetworkObject> _playerMap = new Dictionary<PlayerRef, NetworkObject>();
        private TeamManager teamManager;

        private void Awake()
        {
            teamManager = GetComponent<TeamManager>();
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer && _playerPrefab != null)
            {
                Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
                
                var teamIndex = teamManager.GetTeamIndex();
                Vector3 randomPosition = teamManager.GetRandomPosition(teamIndex);

                var character = runner.Spawn(_playerPrefab, randomPosition, Quaternion.identity, inputAuthority: player,
                    (Runner, no) => no.GetComponent<PlayerInitializer>().Initialize(teamIndex));

                _playerMap[player] = character;
                teamManager.AddCharacter(player,teamIndex);
                runner.SetPlayerObject(player, character);

                Log.Info($"Spawn for Player: {player}");
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_playerMap.TryGetValue(player, out var character))
            {
                runner.Despawn(character);
                _playerMap.Remove(player);
                Log.Info($"Despawn for Player: {player}");
            }
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
            Application.Quit(0);
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    }
}