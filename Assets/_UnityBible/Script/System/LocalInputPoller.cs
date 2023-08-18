using Fusion.Sockets;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityBibleSample
{
    public class LocalInputPoller : MonoBehaviour, INetworkRunnerCallbacks
    {
        private bool _mouseButton0;
        private bool _mouseButton1;
        private bool _keyArrowLeft;
        private bool _keyArrowRight;
        private bool _keySpace;

        private void Update()
        {
            _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
            _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
            _keyArrowLeft = _keyArrowLeft || Input.GetKey(KeyCode.LeftArrow);
            _keyArrowRight = _keyArrowRight || Input.GetKey(KeyCode.RightArrow);
            _keySpace = _keySpace || Input.GetKey(KeyCode.Space);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            if (Input.GetKey(KeyCode.W))
                data.direction += Vector3.forward;

            if (Input.GetKey(KeyCode.S))
                data.direction += Vector3.back;

            if (Input.GetKey(KeyCode.A))
                data.direction += Vector3.left;

            if (Input.GetKey(KeyCode.D))
                data.direction += Vector3.right;

            if (_mouseButton0)
                data.buttons |= NetworkInputData.MOUSEBUTTON1;
            _mouseButton0 = false;

            if (_mouseButton1)
                data.buttons |= NetworkInputData.MOUSEBUTTON2;
            _mouseButton1 = false;

            if (_keyArrowLeft)
                data.buttons |= NetworkInputData.KEYARROWLEFT;
            _keyArrowLeft = false;

            if (_keyArrowRight)
                data.buttons |= NetworkInputData.KEYARROWRIGHT;
            _keyArrowRight = false;

            if (_keySpace)
                data.buttons |= NetworkInputData.KEYSPACE;
            _keySpace = false;

            input.Set(data);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}