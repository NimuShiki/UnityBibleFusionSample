using Cinemachine;
using Fusion;
using System.Linq;
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
            quaternion = Quaternion.identity;
            quaternionInterpolator = GetInterpolator<Quaternion>(nameof(quaternion));
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

                quaternion *= Quaternion.AngleAxis(_rotateSpeed * Runner.DeltaTime, new Vector3(0, direction, 0));
            }
        }

        public override void Render()
        {
            Turret.rotation = quaternionInterpolator.Value;
        }
    }
}