using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
   //---------------Components------------------//
    private CharacterController _controller;
    private Transform _camara;
    

    //---------------Input-----------------------//
    private float _horizontal;
    private float _vertical;
    private float xRotation;
    

   
    [SerializeField] private float _sensitivity = 100f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _JumpHeight = 2;

    //---------------Graveded--------------------//
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //---------------Grounded-------------------//
    [SerializeField] float _sensorRadius = 0.5f;
    [SerializeField] Transform _sensorPosition;
    [SerializeField] private LayerMask _groundLayer;

    
  
    
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camara = Camera.main.transform;
        
    }

    // Start is called before the first frame update
    void Start()
    {
         Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
        Movimiento();
    

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
            
        }
       
       

        Gravity();

       
        
    }

    void Movimiento()
    {
       float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
       float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

  
       xRotation = Mathf.Clamp(xRotation, -90, 90);

       _camara.localRotation = Quaternion.Euler(xRotation, 0, 0);
       transform.Rotate(Vector3.up * mouseX);


       Vector3 move = transform.right * _horizontal + transform.forward * _vertical;

       _controller.Move(move * _speed * Time.deltaTime);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }
}
