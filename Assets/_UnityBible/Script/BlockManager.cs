using Fusion;
using UnityEngine;

namespace UnityBibleSample
{
    public class BlockManager : NetworkBehaviour, ISpawned
    {
        [Networked(OnChanged = nameof(ChangeSetting))][SerializeField] private int score { get; set; }
        [Networked] private TickTimer particleTimer { get; set; }
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public void InitializeObjSettings(int size)
        {
            score = size;
        }

        public static void ChangeSetting(Changed<BlockManager> changed)
        {
            changed.Behaviour.transform.localScale = Vector3.one * changed.Behaviour.score;
            changed.Behaviour.GetComponent<Rigidbody>().mass *= changed.Behaviour.score;
        }

        public override void FixedUpdateNetwork()
        {
            if (particleTimer.ExpiredOrNotRunning(Runner)) {
                particleTimer = TickTimer.CreateFromSeconds(Runner, 1f);
                _particleSystem?.Play();
            }
        }

        public int GetScore()
        {
            return score;
        }
    }
}
