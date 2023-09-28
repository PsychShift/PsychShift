using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 20.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    private CharacterInfo currentCharacter = new CharacterInfo();
    [SerializeField] GameObject tempCharacter;
    [SerializeField] public float swapDistance = 10f;
    public LayerMask swapableLayer;

    [SerializeField] private float smoothInputSpeed = 0.2f;


    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private void Start()
    {
        InputManager.Instance.OnSlowActionStateChanged += SlowMotion;
        InputManager.Instance.OnSwapPressed += SwapPressed;
        cameraTransform = Camera.main.transform;
        SwapCharacter(tempCharacter);
    }

    void Update()
    {
        #region movement
        groundedPlayer = currentCharacter.controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = InputManager.Instance.GetPlayerMovement();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        
        currentCharacter.controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (InputManager.Instance.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        currentCharacter.controller.Move(playerVelocity * Time.deltaTime);
        #endregion
        
    }    
    
    public void SwapPressed()
    {
        SwapCharacter(CheckForCharacter());
    }

    public GameObject CheckForCharacter()
        {
            // Create a ray from the camera's position into the scene.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            
            // Declare a RaycastHit variable to store information about the raycast hit.
            RaycastHit hit;

            // Perform the raycast and check if it hits something with the "Swapable" tag.
            if (Physics.Raycast(ray, out hit, swapDistance, swapableLayer))
            {
                // Check if the hit object has the "Swapable" tag.
                if (hit.collider.CompareTag("Swapable"))
                {
                    // Return the GameObject that was hit.
                    return hit.collider.gameObject;
                }
            }

            // If no "Swapable" object was hit, return null.
            return null;
        }

    public void SwapCharacter(GameObject newCharacter)
    {
        if(newCharacter == null) return;
        currentCharacter = new CharacterInfo
        {
            characterContainer = newCharacter,
            model = newCharacter.transform.GetChild(1).gameObject,
            cameraRoot = newCharacter.transform.GetChild(0),
            controller = newCharacter.GetComponent<CharacterController>()
        };
        virtualCamera.Follow = currentCharacter.cameraRoot;
    }

    private void SlowMotion(bool timeSlow)
    {
        if(timeSlow)
        {
            TimeManager.Instance.DoSlowmotion();
        }
        else
        {
            TimeManager.Instance.UndoSlowmotion();
        }
    }
}
