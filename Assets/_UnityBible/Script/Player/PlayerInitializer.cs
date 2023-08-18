using Fusion;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace UnityBibleSample
{
    public class PlayerInitializer : NetworkBehaviour
    {
        [SerializeField] private List<MeshRenderer> bodyMeshRenderers;
        [SerializeField] private List<Material> teamMaterials;
        private Camera _Camera;
        private CinemachineVirtualCamera _cmVCam;
        [Networked(OnChanged = nameof(CameraSetting))][SerializeField] private int playerID { get; set; }
        [Networked(OnChanged = nameof(ChangeSetting))][SerializeField] public int teamIndex { get; set; }

        private void Awake()
        {
            _Camera = GetComponentInChildren<Camera>();
            _cmVCam = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        public void Initialize(int team)
        {
            teamIndex = team;
            GetComponent<TurningTurretByKey>().SetStartRotation(teamIndex);

            //playerIDに0を入れてもコールバックが起きないため+1する
            playerID = Object.InputAuthority.PlayerId + 1;
        }

        public static void CameraSetting(Changed<PlayerInitializer> changed)
        {
            int id = 9 + changed.Behaviour.playerID;
            changed.Behaviour._Camera.cullingMask |= 1 << id;
            changed.Behaviour._cmVCam.gameObject.layer = id;
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