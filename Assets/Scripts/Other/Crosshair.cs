using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    void Update()
    {
        // ตำแหน่งเมาส์บน world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // ย้าย crosshair ให้ตามเมาส์เสมอ
        transform.position = mousePos;
    }
}