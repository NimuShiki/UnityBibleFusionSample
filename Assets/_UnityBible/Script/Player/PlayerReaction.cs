using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class PlayerReaction : NetworkBehaviour, ITargetReaction
    {
        public void Interact(Vector3 power, Vector3 pos)
        {
            Debug.Log("[" + Runner.Tick + "] Player " + Object.InputAuthority.PlayerId + " Attacked !", this);
        }

        public void Respawn()
        {
            throw new System.NotImplementedException();
        }
    }
}