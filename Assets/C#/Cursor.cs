using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private void Update()
    {
        // transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = Mouse.current.position.ReadValue();
    }
}
