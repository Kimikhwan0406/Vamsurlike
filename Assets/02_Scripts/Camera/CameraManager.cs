using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : SingletonBehaviour<CameraManager>
{
    [SerializeField] CinemachineCamera cinemachineCamera;

    public void FollowPlayer(Transform transform)
    {
        cinemachineCamera.Follow = transform;

        cinemachineCamera.PreviousStateIsValid = false;
    }

    public void ClearFollow()
    {
        cinemachineCamera.Follow = null;

        cinemachineCamera.PreviousStateIsValid = false;
    }
}
