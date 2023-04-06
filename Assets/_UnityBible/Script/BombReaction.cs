using Fusion;
using System.Collections.Generic;
using UnityEngine;


namespace UnityBibleSample
{
    public class BombReaction : NetworkBehaviour, ITargetReaction
    {
        [SerializeField] private float radius;
        [SerializeField] private float pow;
        [SerializeField] private LayerMask bomMask;
        private Rigidbody rigidbody;
        private List<Collision> targets;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Interact(Vector3 power, Vector3 pos)
        {
            RPC_Reaction(power, pos);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_Reaction(Vector3 power, Vector3 pos)
        {
            rigidbody.AddForceAtPosition(power, pos, ForceMode.Impulse);

            //List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();
            //Runner.LagCompensation.OverlapSphere(Vector3.zero, radius, Object.InputAuthority, _hits);
            
            //HitboxManager hitboxManager = new HitboxManager();
            //hitboxManager.OverlapSphere(pos, radius, Object.InputAuthority, _hits);
            
            //Collider[] colliders = Physics.OverlapSphere(Vector3.zero, radius, bomMask);
            //Debug.Log("colliders " + transform.position);
            //Debug.Log("colliders " + colliders.Count());

            //foreach (var rb in targets)
            //{
            //    rb.gameObject.GetComponent<IHitReaction>().Interact(power);
            //}
        }

        public void Respawn()
        {
            var pos = UnityEngine.Random.insideUnitSphere * 10;
            pos.y = 20;
            transform.position = pos; //ˆÚ“®‚ªŒ©‚¦‚¿‚á‚¤‚Ì‚Å‚²‚Ü‚©‚µ‚½‚¢
            rigidbody.velocity = Vector3.zero;
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Target") == false) return;
        //    collision.gameObject.GetComponent<IHitReaction>().Interact(new Vector3(0, 10, 0));

        //    Debug.Log("collision ", collision.gameObject);
        //    targets.Add(collision);
        //    Debug.Log("targets " + targets.Count);
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Target") == false) return;

        //    targets.Remove(collision);

        //}
    }
}