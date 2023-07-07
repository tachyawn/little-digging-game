using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraZ : CinemachineExtension
{
    public bool lockX = false;
    public float xMin = -10f;
    public float xMax = 10f;
    
    public bool lockY = false;
    public float yMin = -20f;
    public float yMax = 10f;

    public bool lockZ = false;
    public float zMin = -10f;
    public float zMax = 10f;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos = KeepCamInRange(pos);
            state.RawPosition = pos;
        }
    }

    private Vector3 KeepCamInRange(Vector3 pos)
    {
        Vector3 newPos = pos;
        if (lockX)
        {
            if (pos.x < xMin) pos.x = xMin;
            else if (pos.x > xMax) pos.x = xMax;
        }
        if (lockY)
        {
            if (pos.y < yMin) pos.y = yMin;
            else if (pos.y > yMax) pos.y = yMax;
        }
        if (lockZ)
        {
            if (pos.z < zMin) pos.z = zMin;
            else if (pos.z > zMax) pos.z = zMax;
        }

        return newPos;
    }
}
