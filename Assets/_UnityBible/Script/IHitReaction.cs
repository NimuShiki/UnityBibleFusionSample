using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBibleSample
{
    public interface ITargetReaction
    {
        void Interact(Vector3 power, Vector3 pos);
        void Respawn();
    }
}