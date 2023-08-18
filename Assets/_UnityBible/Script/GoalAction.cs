using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class GoalAction : NetworkBehaviour
    {
        [SerializeField] private int scoreRate;
        private GoalManager _GoalManager;

        private void Awake()
        {
            _GoalManager = GetComponentInParent<GoalManager>();   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Target") == false) return;
            other.GetComponent<ITargetReaction>().Respawn();
            var score = other.GetComponent<BlockManager>().GetScore();
            _GoalManager.AddScore(score * scoreRate);
        }
    }
}