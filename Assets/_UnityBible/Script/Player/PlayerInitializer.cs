using Fusion;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBibleSample
{
    public class PlayerInitializer : NetworkBehaviour
    {
        [SerializeField] private List<MeshRenderer> bodyMeshRenderers;
        [SerializeField] private List<Material> teamMaterials;
        [Networked(OnChanged = nameof(ChangeSetting))] [SerializeField] public int teamIndex { get; set; } = 0;

        public void Initialize(int team)
        {
            teamIndex = team;
            GetComponent<TurningTurretByKey>().SetStartRotation(teamIndex);
        }

        public static void ChangeSetting(Changed<PlayerInitializer> changed)
        {
            switch (changed.Behaviour.teamIndex)
            {   
                case 0:
                case 1:
                    changed.Behaviour.bodyMeshRenderers.ForEach(x => x.material = changed.Behaviour.teamMaterials[changed.Behaviour.teamIndex]);
                    break;
                default:
                    break;
            }
        }
    }
}