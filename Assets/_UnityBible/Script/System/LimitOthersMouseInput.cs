using Cinemachine;
using Fusion;
using UnityEngine;

public class LimitOthersMouseInput : NetworkBehaviour
{
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public float GetAxisCustom(string axisName)
    {
        if (Object.HasInputAuthority == false) return 0;

        return UnityEngine.Input.GetAxis(axisName);
    }
}
