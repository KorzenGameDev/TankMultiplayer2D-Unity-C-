using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Camera myCamera;
    Transform target = null;

    public void SetTargetToCamera(Transform t) { target = t; }

    void CameraFallow()
    {
        if(target!=null)
        {
            Vector3 cameraPos = myCamera.transform.position;
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, -10);
            if(targetPos.x>52f) targetPos = new Vector3(52f, target.position.y, -10);
            if (targetPos.x < -52f) targetPos = new Vector3(-52f, target.position.y, -10);
            myCamera.transform.position = Vector3.Lerp(cameraPos, targetPos, 1f);
        }
        else
        {
            Vector3 waitPos = new Vector3(0f, 0f, -10f);
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, waitPos, .1f);
        }
    }
    void LateUpdate() { CameraFallow(); }
}
