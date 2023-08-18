using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBibleSample
{
    public class BlockSpawner : NetworkBehaviour, INetworkRunnerCallbacks, ISpawned
    {
        [Networked] private int _spawnRadius { get; set; } = 20;
        [Networked] private int _spawnHeight { get; set; } = 10;
        [Networked] private int _blockNum { get; set; } = 2;
        [SerializeField] private NetworkObject _blockPrefab;
        [Networked] private int _blockCapsuleNum { get; set; } = 1;
        [SerializeField] private NetworkObject _blockCapsulePrefab;
        [SerializeField] private Transform targetParent;

        int[] size = { 2, 3, 4 };
        int[] probabilities = { 8, 3, 1 };

        public override void Spawned()
        {
            Runner.AddCallbacks(this);
            BlockSpawn(Runner, 5, 2);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            BlockSpawn(Runner, _blockNum, _blockCapsuleNum);
        }

        private void BlockSpawn(NetworkRunner runner,int block, int bomb)
        {
            for (int i = 0; i < block; i++)
            {
                var pos = UnityEngine.Random.insideUnitSphere * _spawnRadius;
                pos.y = _spawnHeight;
                var obj = runner.Spawn(_blockPrefab, pos, Quaternion.identity, onBeforeSpawned: InitializeObjBeforeSpawn);
                //親子共にNetworkTransformAnchorをアタッチしておかないと無効
                if(Object.HasStateAuthority) obj.transform.SetParent(targetParent, false);
            }

            for (int i = 0; i < bomb; i++)
            {
                var pos2 = UnityEngine.Random.insideUnitSphere * _spawnRadius;
                pos2.y = _spawnHeight;
                var obj = runner.Spawn(_blockCapsulePrefab, pos2, Quaternion.identity);
                //親子共にNetworkTransformAnchorをアタッチしておかないと無効
                if (Object.HasStateAuthority) obj.transform.SetParent(targetParent, false);
            }
        }

        private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
        {
            var total = probabilities.Sum();
            var randomValue = UnityEngine.Random.Range(0, total);
            int blockSize = 1;
            float cumulativeProbability = 0f;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue < cumulativeProbability)
                {
                    blockSize = size[i];
                    break;
                }
            }
            var objSB = obj.GetComponent<BlockManager>();
            objSB.InitializeObjSettings(blockSize);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
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
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    }
}