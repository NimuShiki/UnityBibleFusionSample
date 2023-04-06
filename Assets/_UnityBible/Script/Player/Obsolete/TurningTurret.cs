using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class TurningTurret : NetworkBehaviour
    {
        [SerializeField] private Transform camera;
        [Networked] private Quaternion quaternion { get; set; }

        public override void FixedUpdateNetwork()
        {
            if (camera == null) return;
            var cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            //transform.rotation = Quaternion.FromToRotation(Vector3.forward, cameraForward);
            quaternion = Quaternion.FromToRotation(Vector3.forward, cameraForward);

        }
        public override void Render()
        {
            transform.rotation = quaternion;
        }
    }
}