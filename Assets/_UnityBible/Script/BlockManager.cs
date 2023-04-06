using Fusion;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityBibleSample
{
    public class BlockManager : NetworkBehaviour, ISpawned
    {
        [Networked(OnChanged = nameof(ChangeSetting))][SerializeField] private int score { get; set; }

        public void InitializeObjSettings(int size)
        {
            score = size;
        }

        public static void ChangeSetting(Changed<BlockManager> changed)
        {
            changed.Behaviour.transform.localScale = Vector3.one * changed.Behaviour.score;
            changed.Behaviour.GetComponent<Rigidbody>().mass *= changed.Behaviour.score;
        }

        public int GetScore()
        {
            return score;
        }
    }
}
