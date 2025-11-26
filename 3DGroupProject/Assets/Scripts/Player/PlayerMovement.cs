using UnityEngine;

/* 
free flying body cam like movement with main camera attached to player.
want it to have wasd as main movement.
can control direction player is facing by holding and dragging right click.
this will change the perspective of the camera and player.
thus, using wasd would reflect camera direction for the feel of free movement.
*/
public class PlayerMovement : MonoBehaviour
{
    //player movement speed
    public float moveSpeed = 10f; //default move speed

    //for player cam rotation
    public float lookSensitivity = 2f; //mouse sensitivity
    public float maxVerticalAngle = 89f; //prevents full flip

    //rotation smoothing speed
    public float rotationSpeed = 10f; //how fast smoothed rotation happens

    private Vector2 keyInput; //wasd keys for movement
    private Vector2 mouseInput; //mouse movement for looking around

    private bool holdingM2; //check if right mouse button is held

    //camera rotation
    private float horizontalR; //x rotation
    private float verticalR; //y rotation

    void Start()
    {
        //initialize rotation values at start
        horizontalR = transform.eulerAngles.y;
        verticalR = transform.eulerAngles.x;
    }

    void Update()
    {
        GetInput(); //get player input each frame
        PlayerMove(); //move player based on input
        MouseMove(); //rotate player direction based on mouse input
    }

    void GetInput()
    {
        //input for wasd movement
        keyInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //mouse movement for camera rotation
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //right mouse button check
        holdingM2 = Input.GetMouseButton(1);
    }

    void PlayerMove()
    {
        //to get a free cam like movement sorta like roblox
        Vector3 moveDir = transform.forward * keyInput.y + transform.right * keyInput.x;

        //move player position
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void MouseMove()
    {
        //if not holding right mouse button, do not rotate camera
        if (!holdingM2)
        {
            Cursor.lockState = CursorLockMode.None; //mouse free
            return; //do not rotate camera
        }

        Cursor.lockState = CursorLockMode.Locked; //locks mouse at center of screen

        //add look input to rotation amounts
        horizontalR += mouseInput.x * lookSensitivity; //horizontal movement
        verticalR -= mouseInput.y * lookSensitivity;   //vert movement

        //limit vertical rotation
        verticalR = Mathf.Clamp(verticalR, -maxVerticalAngle, maxVerticalAngle);

        //to smooth out rotation
        //apparently quaternion is good for rotations, used euler angles
        //the lerp is for smoothing
        Quaternion targetRotation = Quaternion.Euler(verticalR, horizontalR, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

