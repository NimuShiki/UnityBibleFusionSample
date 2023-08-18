using Fusion;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace UnityBibleSample
{
    public class TargetReaction : NetworkBehaviour, ITargetReaction
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private new Rigidbody rigidbody;
        [Networked] private TickTimer delay { get; set; }
        [SerializeField] private float Interval;
        private NetworkTransform networkTransform;

        private void Awake()
        {
            networkTransform = GetComponent<NetworkTransform>();
        }

        public override void FixedUpdateNetwork()
        {
            if (delay.ExpiredOrNotRunning(Runner)) meshRenderer.enabled = true;
        }

        public void Interact(Vector3 power, Vector3 pos)
        {
            // Shared���[�h�œ��������邽�߂ɂ�RPC���K�v
            // �ʏ�GameMode�̐؂�ւ���O��ɂ��邱�Ƃ͂Ȃ��̂ŁA�f���I�Ȏ���
            if (Runner.GameMode == GameMode.Shared)
            {
                RPC_Reaction( power, pos);
            }
            else
            {
                rigidbody.AddForceAtPosition(power, pos, ForceMode.Impulse);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_Reaction(Vector3 power, Vector3 pos)
        {
            rigidbody.AddForceAtPosition(power, pos, ForceMode.Impulse);
        }

        public void Respawn()
        {
            var pos = UnityEngine.Random.insideUnitSphere * 20;
            pos.y = 20;

            networkTransform.TeleportToPosition(pos);
            rigidbody.velocity = Vector3.zero;

            meshRenderer.enabled = false;
            delay = TickTimer.CreateFromSeconds(Runner, Interval);
        }
    }
}