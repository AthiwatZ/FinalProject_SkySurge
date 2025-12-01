using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Player

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z   // ค่า Z เดิมของกล้อง
        );
    }
}

