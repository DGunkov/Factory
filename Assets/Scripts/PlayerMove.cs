using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private FixedJoystick _joystick;

    private Rigidbody _player_rigidbody;
    private GameObject _camera;


    private void Start()
    {
        _camera = Camera.main.gameObject;
        _player_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _camera.transform.position = transform.position + new Vector3(0, 30, -25);
        _player_rigidbody.velocity = new Vector3(_joystick.Horizontal, _player_rigidbody.velocity.y, _joystick.Vertical) * 25;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_player_rigidbody.velocity);
        }
    }
}
