using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private void Awake()
    {
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
