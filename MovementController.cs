using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //NEWLY ADDED
    
    CharacterController Controller;
 
    public float Speed;
    public float fixedspeed;
    public float sprint;
 
 int VecolcityHash;
    public Transform Cam;
    Animator animator;



    private Vector3 velocity;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private LayerMask groundmask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
 
   
    // Start is called before the first frame update
    void Start()
    {
 
        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        VecolcityHash = Animator.StringToHash("Velocity");
        
 
    }
 
    // Update is called once per frame
    void Update()
    {

        IsGrounded = Physics.CheckSphere(transform.position, GroundCheckDistance, groundmask);
        if(IsGrounded && velocity.y <0){
            velocity.y = -2f;
        }
        fixedspeed = 2;
 
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
 
        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Movement.y = 0f;

 
 
        Controller.Move(Movement);
 
        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMove>().sensivity * Time.deltaTime);
 
 
            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;
 
            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
 
        }
    
        if(Movement != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)){
                //WALK
                animator.SetFloat("Velocity", 0.5f);
        }else if(Movement != Vector3.zero && Input.GetKey(KeyCode.LeftShift)){
                //RUN
                animator.SetFloat("Velocity", 1);
        }
        else if(Movement == Vector3.zero){
                //WALK
                animator.SetFloat("Velocity", 0);
        }
        
        velocity.y += gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

        
    }
    private void FixedUpdate() {
         if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = fixedspeed + sprint;
        }
         if (!Input.GetKey(KeyCode.LeftShift))
        {
            Speed = fixedspeed;
        }
    }
    private void Jump(){
        velocity.y = Mathf.Sqrt(jumpHeight *-2 *gravity);
    }
}
