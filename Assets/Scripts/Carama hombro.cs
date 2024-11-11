using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Caramahombro : MonoBehaviour
{
    //---------------Components------------------//
    private CharacterController _controller;
    private Transform _camara;

    private Transform _lookAtTarget;

    //---------------Input-----------------------//
    private float _horizontal;
    private float _vertical;
    private float _turnSmoothVelocity; 
    
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _JumpHeight = 2;

    //---------------Graveded--------------------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------------Grounded-------------------//
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] Transform _sensorPosition;
    [SerializeField] private LayerMask _groundLayer;

    //---------------Camera---------------------//
    [SerializeField] private AxisState xAxis;
    [SerializeField] private AxisState yAxis;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camara = Camera.main.transform;
        _lookAtTarget = GameObject.Find("LookAtPlayer").transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        Gravity(); 
        Movimiento(); 
    }

    void Movimiento()
    {
        Vector3 move= new Vector3(_horizontal, 0, _vertical);

        yAxis.Update(Time.deltaTime);
        xAxis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, xAxis.Value, 0);
        _lookAtTarget.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0);

        if(move != Vector3.zero)
        {
            float targeAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camara.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targeAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }

    }

     void Gravity()
    {
        if(!IsGrounded())
        {
             _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = -1;
        }

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_JumpHeight * -2 * _gravity);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

    }
}
