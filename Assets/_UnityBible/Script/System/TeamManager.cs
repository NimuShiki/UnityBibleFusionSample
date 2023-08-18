using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityBibleSample
{
    public class TeamManager : SimulationBehaviour
    {
        [SerializeField] private float innerRadius = 6f;
        [SerializeField] private float outerRadius = 10f;

        private readonly Dictionary<PlayerRef, int> _playerMap = new Dictionary<PlayerRef, int>();

        public int GetTeamIndex()
        {
            var teamIndex = 0;

            var all = _playerMap.Count();
            var teamB = _playerMap.Where(x => x.Value == 0).Count();

            if (all - teamB < teamB) {
                teamIndex = 1;
            }
            else if (all - teamB == teamB)
            {
                teamIndex = UnityEngine.Random.Range(0, 2);
            }

            return teamIndex;
        }

        public Vector3 GetRandomPosition(int team)
        {

            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

            float randomDistance = UnityEngine.Random.Range(innerRadius, outerRadius);

            var pos = randomDirection * randomDistance;
            pos.y = 0;
            pos.z += team * 60 - 30;

            return pos;
        }

        public void AddCharacter(PlayerRef playerRef,int team)
        {
            _playerMap[playerRef] = team;
        }

        public void RemoveCharacter(PlayerRef playerRef)
        {
            _playerMap.Remove(playerRef);
        }
    }
}