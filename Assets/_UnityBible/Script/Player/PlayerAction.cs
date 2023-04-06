using Cinemachine;
using Fusion;
using System.Linq;
using UnityEngine;

namespace UnityBibleSample
{
    public class PlayerAction : NetworkBehaviour
    {
        [Networked] private TickTimer delay { get; set; }
        [Networked(OnChanged = nameof(PlayFireParticle))] private NetworkBool underFire { get; set; }

        [SerializeField] private GameObject VisualObject;
        [SerializeField] private Transform ShootReference;
        [SerializeField] private AudioSource ShootSource;
        [SerializeField] private ParticleSystem fireParticle;
        [SerializeField] private float fireInterval;
        [SerializeField] private float ShootPower;
        [SerializeField] private float RayLength;
        [SerializeField] private float RayDuration;
        [SerializeField] private LayerMask hitMask;
        private NetworkCharacterControllerPrototype _cc;
        private Camera _Camera;
        private CinemachineVirtualCamera _cmVCam;
        private AudioListener _audioListener;
        [SerializeField] private CinemachineBrain _cmBrain;

        private bool canMove;

        private void Awake()
        {
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
            _Camera = GetComponentInChildren<Camera>();
            _cmVCam = GetComponentInChildren<CinemachineVirtualCamera>();
            _audioListener = GetComponentInChildren<AudioListener>();
        }

        public override void Spawned()
        {
            Runner.AddCallbacks(GetComponentsInChildren<INetworkRunnerCallbacks>());

            if (Object.HasInputAuthority)
            {
                _Camera.cullingMask |= 1 << 10;
                _cmVCam.gameObject.layer = 10;
                _cmVCam.Priority++;
            }
            else
            {
                _Camera.cullingMask |= 1 << 11;
                _cmVCam.gameObject.layer = 11;
            }
        }

        public override void FixedUpdateNetwork()
        {
            underFire = false;

            if (Object.HasInputAuthority == false)
            {
                //マルチピアでRunner visibility nodes対応できない部分の対処
                _audioListener.enabled = false;
                _Camera.enabled = false;
            }

            // インプット処理
            if (GetInput(out NetworkInputData data))
            {
                // 移動
                data.direction.Normalize();
                if (data.direction.sqrMagnitude > 0 && canMove)
                {
                    //Vector3 cameraForward = Vector3.Scale(_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                    //var direction = data.direction.y * cameraForward + _Camera.transform.right * data.direction.x;
                   
                    var cameraForward = Vector3.Scale(_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                    var rotation = Quaternion.FromToRotation(Vector3.forward, cameraForward);
                    _cc.Move(rotation * data.direction * Runner.DeltaTime);
                }

                // アクション
                if (delay.ExpiredOrNotRunning(Runner))
                {
                    canMove = true;
                    if ((data.buttons & NetworkInputData.KEYSPACE) != 0)
                    {
                        Fire();
                    }

                    if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0)
                    {
                    }
                }
            }
        }

        private void Fire()
        {
            delay = TickTimer.CreateFromSeconds(Runner, fireInterval);

            underFire = true;
            
            fireParticle.Play();

            var hit = Runner.LagCompensation.Raycast(
                ShootReference.position, ShootReference.forward, RayLength, //Rayの基準点、向き、長さ
                Object.InputAuthority,
                out var lagCompensatedHit,
                hitMask.value, //判定を行うレイヤーを指定
                HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority); //Unityのコライダーも取得、自分自身は無視

            Debug.DrawRay(
                ShootReference.position,
                ShootReference.forward * RayLength,
                Color.red,
                RayDuration);

            //着弾処理 
            if (hit)
            {
                lagCompensatedHit.GameObject.GetComponent<ITargetReaction>()?.Interact(ShootReference.forward * ShootPower, lagCompensatedHit.Point);
            }
        }

        public static void PlayFireParticle(Changed<PlayerAction> changed)
        {
            if (changed.Behaviour.underFire) {
                changed.Behaviour.ShootSource.Play();
                changed.Behaviour.fireParticle.Play();
            }
        }
    }

}