using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class TurningTurretByKey : NetworkBehaviour
    {
        [SerializeField] private Transform Turret;
        [SerializeField] private float _rotateSpeed;
        private float direction;

        [Networked] private Quaternion quaternion { get; set; }
        private Interpolator<Quaternion> quaternionInterpolator;

        void Start()
        {
            quaternionInterpolator = GetInterpolator<Quaternion>(nameof(quaternion));
            quaternion *= Quaternion.AngleAxis(1f, new Vector3(0, 1, 0));
        }

        public override void FixedUpdateNetwork()
        {
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

                if (direction == 0) return;
                
                quaternion *= Quaternion.AngleAxis(_rotateSpeed * Runner.DeltaTime, new Vector3(0, direction, 0));
            }
        }

        public void SetStartRotation(int teamIndex)
        {
            quaternion = Quaternion.identity;
            quaternionInterpolator = GetInterpolator<Quaternion>(nameof(quaternion));

            quaternion *= Quaternion.AngleAxis(teamIndex * 180f, new Vector3(0, 1, 0));
        }

        public override void Render()
        {
            Turret.rotation = quaternionInterpolator.Value;
        }
    }
}