using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void Update()
    {
        // transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = Mouse.current.position.ReadValue();
    }

    public void HideCursor()
    {
        _image.enabled = false;
    }

    public void ShowCursor()
    {
        _image.enabled = true;
    }
}
