using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class PlayerAction : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(PlayFireParticle))] private TickTimer delay { get; set; }
        private bool underFire => !delay.ExpiredOrNotRunning(Runner);
        [SerializeField] private Transform ShootReference;
        [SerializeField] private AudioSource ShootSource;
        [SerializeField] private float fireInterval;
        [SerializeField] private float ShootPower;
        [SerializeField] private float RayLength;
        [SerializeField] private float RayDuration;
        [SerializeField] private LayerMask hitMask;
        private NetworkCharacterControllerPrototype _cc;
        private Camera _Camera;
        private Quaternion rotation;
        private AudioListener _audioListener;
        private ParticleSystem fireParticle;

        private bool canMove;

        private void Awake()
        {
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
            _Camera = GetComponentInChildren<Camera>();
            _audioListener = GetComponentInChildren<AudioListener>();
            fireParticle = ShootReference.GetComponentInChildren<ParticleSystem>();
        }

        public override void FixedUpdateNetwork()
        {
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
                }
            }
        }

        //各シミュレーションが終了した後に実行される
        public override void Render()
        {
            // シミュレーションが終わって確定したカメラの位置を基準に、次の補正値を計算する
            var cameraForward = Vector3.Scale(_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            rotation = Quaternion.FromToRotation(Vector3.forward, cameraForward);
        }

        private void Fire()
        {
            delay = TickTimer.CreateFromSeconds(Runner, fireInterval);

            var hit = Runner.LagCompensation.Raycast(
                ShootReference.position, ShootReference.forward, RayLength, //Rayの基準点、向き、長さ
                Object.InputAuthority,
                out var lagCompensatedHit,
                hitMask.value, //判定を行うレイヤーを指定
                HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority); //Unityのコライダーも判定、自分自身は無視

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
                changed.Behaviour.ShootSource?.Play();
                changed.Behaviour.fireParticle.Play();
            }
        }
    }

}