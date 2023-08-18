using Fusion;
using TMPro;
using UnityEngine;

namespace UnityBibleSample
{
    public class GameManager : NetworkBehaviour, ISpawned
    {
        [SerializeField] private NetworkObject _playerPrefab;
        [SerializeField] private GoalManager _goalManagerR;
        [SerializeField] private GoalManager _goalManagerB;
        [Networked] private TickTimer gameTimer { get; set; }
        [SerializeField] private float gameTime;
        [Networked] private TickTimer intervalTimer { get; set; }
        [Networked] private bool inInterval { get; set; }
        [SerializeField] private float gameInterval;
        [SerializeField] private TextMeshProUGUI TMPTime;
        [SerializeField] private TextMeshProUGUI TMPResult;

        public override void Spawned()
        {
            inInterval = false;
            _goalManagerR.inGame = true;
            _goalManagerB.inGame = true;
            if (gameTimer.ExpiredOrNotRunning(Runner)) gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);
        }

        public override void FixedUpdateNetwork()
        {
            UpdateCountTxt();

            if (gameTimer.ExpiredOrNotRunning(Runner)) TimeUpSquense();
        }

        private void TimeUpSquense()
        {
            if (inInterval)
            {
                if (intervalTimer.ExpiredOrNotRunning(Runner) == false) return;

                gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);
                inInterval = false;
                _goalManagerR.inGame = true;
                _goalManagerB.inGame = true;
                TMPResult.text = "";
            }
            else
            {
                intervalTimer = TickTimer.CreateFromSeconds(Runner, gameInterval);
                inInterval = true;
                _goalManagerR.inGame = false;
                _goalManagerB.inGame = false;
                TMPTime.text = "0";

                var scoreR = _goalManagerR.TeamScore;
                var scoreB = _goalManagerB.TeamScore;
                var resultTxt = "";
                if (scoreR == scoreB)
                {
                    resultTxt = "Draw Game";
                }
                else if (scoreR > scoreB)
                {
                    resultTxt = "Red Team Win!";
                }
                else if (scoreR < scoreB)
                {
                    resultTxt = "Blue Team Win!";
                }
                TMPResult.text = resultTxt;

                _goalManagerR.ResetScore();
                _goalManagerB.ResetScore();
            }
        }

        private void UpdateCountTxt()
        {
            var time = gameTimer.RemainingTime(Runner);
            if (time == null || time == 0f) return;
            TMPTime.text = Mathf.FloorToInt((float)time  + 1).ToString();
        }
    }
}