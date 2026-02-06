using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed = 10f;
    [SerializeField] private GameObject _bodyReference;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _move_direction;
    private float _rotate_direction;

    private void Start()
    {
        _rigidbody2D = GetComponentInChildren<Rigidbody2D>();
    }

    #region Input
    public void Move(InputAction.CallbackContext context)
    {
        _move_direction = context.ReadValue<Vector2>();
    }

    public void MousePoint(InputAction.CallbackContext context)
    {
        // Vector2 mouse_pos = context.ReadValue<Vector2>();
        // Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        // Vector2 aim_direction = mouse_pos - screenPos;
        // float angle = Mathf.Atan2(aim_direction.y, aim_direction.x) * Mathf.Rad2Deg;
        //
        // Vector2 rotation_temp = transform.eulerAngles;
        // rotation_temp.x = angle;
        // transform.eulerAngles = rotation_temp;
        
        Vector2 mouseScreenPos = context.ReadValue<Vector2>();
        Vector2 startingScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        float angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;

        Vector2 rotation_temp = _bodyReference.transform.localEulerAngles;
        rotation_temp.y = angle;
        _bodyReference.transform.localEulerAngles = rotation_temp;
        // _bodyReference.transform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
    }
    #endregion

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _move_direction * movement_speed * Time.deltaTime * 50;
        // transform.Rotate(Vector3.up, _rotate_direction, Space.Self);
    }
}
