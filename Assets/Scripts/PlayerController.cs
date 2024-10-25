using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    //---------------Components------------------//
    private CharacterController _controller;
    private Transform _camara;

    //---------------Input-----------------------//
    private float _horizontal;
    private float _vertical;
    private float _turnSmoothVelocity;
    [SerializeField] private float _JumpHeight = 2;
    

    //---------------Movement--------------------//
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSmoothTime = 0.05f;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
     
        if (Input.GetButton("Fire2"))
        {
            AimMovimiento();
        }
        else 
        {
            Movimiento(); 
        }


        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        Gravity();
        
        
    }

    void Movimiento()
    {
        Vector3 direction= new Vector3(_horizontal, 0, _vertical);

        if(direction != Vector3.zero)
        {
            float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camara.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref _turnSmoothVelocity, _turnSmoothTime);
       
            transform.rotation = Quaternion.Euler(0,smoothAngle, 0); 

            Vector3 moveDirection = Quaternion.Euler(0, targeAngle, 0) * Vector3.forward;
            
            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }
    }

    
    void AimMovimiento()
    {
        Vector3 direction= new Vector3(_horizontal, 0, _vertical);

        float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camara.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camara.eulerAngles.y, ref _turnSmoothVelocity, _turnSmoothTime);
       
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0); 

        if(direction != Vector3.zero)
        {
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }
    
}
