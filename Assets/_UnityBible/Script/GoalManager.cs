using Fusion;
using TMPro;
using UnityEngine;

namespace UnityBibleSample
{
    public class GoalManager : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(OnScoreChange))][SerializeField] private int teamScore { get; set; }
        public int TeamScore { get { return teamScore; } }
        [Networked] public bool inGame { get; set; }

        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private AudioSource GoalSound;

        public void AddScore(int score)
        {
            if (inGame == false) return;
            if(Object.HasStateAuthority) teamScore += score;
        }

        public void ResetScore()
        {
            teamScore = 0;
        }

        public static void OnScoreChange(Changed<GoalManager> changed)
        {
            changed.Behaviour.scoreTMP.text = changed.Behaviour.teamScore.ToString();
            changed.Behaviour.GoalSound.Play();
        }
    }
}