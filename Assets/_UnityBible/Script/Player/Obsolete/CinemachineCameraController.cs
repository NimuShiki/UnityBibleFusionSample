using Cinemachine;
using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class CinemachineCameraController : NetworkBehaviour
    {
        private CinemachinePOV transposer;
        [SerializeField] CinemachineVirtualCamera cmvCamera;
        [SerializeField] Transform Turret;
        [SerializeField] private float baseX;
        [SerializeField] float mouse_x_delta;
        [SerializeField] float _rotateSpeed;
        [Networked][SerializeField] float direction { get; set; }
        [Networked][SerializeField] private Quaternion quaternion { get; set; }
        [SerializeField] private Quaternion lastQuaternion;
         private Interpolator<Quaternion> quaternionInterpolator;

        void Start()
        {
            transposer = cmvCamera.GetCinemachineComponent<CinemachinePOV>();
            //transposer = GetComponent<CinemachineVirtualCamera>()!.GetCinemachineComponent<CinemachineOrbitalTransposer>();

            //Turret.rotation = quaternion;
            quaternion = Quaternion.identity;
            quaternionInterpolator = GetInterpolator<Quaternion>(nameof(quaternion));
        }

        public override void FixedUpdateNetwork()
        {
            if (transposer == null) return;
            if (Object.HasInputAuthority == false) return;

            if (GetInput(out NetworkInputData data))
            {
                direction = 0;

                if ((data.buttons & NetworkInputData.KEYARROWRIGHT) != 0)
                {
                    direction += 1;
                }
                if ((data.buttons & NetworkInputData.KEYARROWLEFT) != 0)
                {
                    direction -= 1;
                }

                //transposer.m_HorizontalAxis.Value += direction * RotateSpeed;
                //transposer.m_XAxis.Value += direction * RotateSpeed;

                //ƒ^ƒŒƒbƒg‚ð‰ñ‚·
                //Turret.Rotate(transform.rotation.x + direction * RotateSpeed / 90f, 0, 0);
                //Turret.rotation = Quaternion.FromToRotation(Turret.forward, new Vector3(direction * RotateSpeed / 90f, 0, 0));
                //Turret.eulerAngles += RotateSpeed * Runner.DeltaTime * new Vector3(direction, 0, 0);
                //Turret.rotation *= Quaternion.AngleAxis(50 * Runner.DeltaTime, new Vector3(0, direction, 0));
                
                quaternion *= Quaternion.AngleAxis(_rotateSpeed * Runner.DeltaTime, new Vector3(0, direction, 0));
            }
        }

        public override void Render()
        {
            //Turret.rotation *= Quaternion.AngleAxis(_rotateSpeed * Runner.DeltaTime, new Vector3(0, direction, 0));
            //quaternion = Turret.rotation;

            Turret.rotation = quaternionInterpolator.Value;
        }
    }
}