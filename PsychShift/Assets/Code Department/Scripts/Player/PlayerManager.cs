/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera virtualCamera;
    public Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 20.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    private Player.CharacterInfo currentCharacter = new Player.CharacterInfo();
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
    private void OnDisable()
    {
        InputManager.Instance.OnSlowActionStateChanged -= SlowMotion;
        InputManager.Instance.OnSwapPressed -= SwapPressed;
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
        if (InputManager.Instance.IsJumpPressed && groundedPlayer)
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
        // Create a BoxCast to check for objects with the "Swapable" tag.
        Vector3 boxCenter = Camera.main.transform.position + Camera.main.transform.forward * (swapDistance / 2f);
        Vector3 boxHalfExtents = new Vector3(0.5f, 0.5f, swapDistance / 2f);
        Quaternion boxRotation = Camera.main.transform.rotation;

        // Store the results of the BoxCast.
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxHalfExtents, Vector3.forward, boxRotation, swapDistance, swapableLayer);
        Debug.DrawRay(boxCenter, Camera.main.transform.forward * swapDistance, Color.green, 0.5f);
        // Loop through the hits to find the first object with the "Swapable" tag.
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Swapable"))
            {
                // Return the GameObject that was hit.
                return hit.collider.gameObject;
            }
        }
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Manipulatable"))
            {
                return hit.collider.gameObject;
            }
        }

        // If no "Swapable" object was hit, return null.
        return null;
    }

    public void SwapCharacter(GameObject newCharacter)
    {
        if(newCharacter == null) return;
        newCharacter.SetActive(false);
        if (currentCharacter != null) currentCharacter.model.SetActive(true);
        currentCharacter = new Player.CharacterInfo
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
 */