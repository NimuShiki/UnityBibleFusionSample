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
                //�}���`�s�A��Runner visibility nodes�Ή��ł��Ȃ������̑Ώ�
                _audioListener.enabled = false;
                _Camera.enabled = false;
            }

            // �C���v�b�g����
            if (GetInput(out NetworkInputData data))
            {
                // �ړ�
                data.direction.Normalize();
                if (data.direction.sqrMagnitude > 0 && canMove)
                {
                    _cc.Move(rotation * data.direction * Runner.DeltaTime);
                }

                // �A�N�V����
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

        //�e�V�~�����[�V�������I��������Ɏ��s�����
        public override void Render()
        {
            // �V�~�����[�V�������I����Ċm�肵���J�����̈ʒu����ɁA���̕␳�l���v�Z����
            var cameraForward = Vector3.Scale(_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            rotation = Quaternion.FromToRotation(Vector3.forward, cameraForward);
        }

        private void Fire()
        {
            delay = TickTimer.CreateFromSeconds(Runner, fireInterval);

            var hit = Runner.LagCompensation.Raycast(
                ShootReference.position, ShootReference.forward, RayLength, //Ray�̊�_�A�����A����
                Object.InputAuthority,
                out var lagCompensatedHit,
                hitMask.value, //������s�����C���[���w��
                HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority); //Unity�̃R���C�_�[������A�������g�͖���

            Debug.DrawRay(
                ShootReference.position,
                ShootReference.forward * RayLength,
                Color.red,
                RayDuration);

            //���e���� 
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