using UnityEngine;

public class GameController : MonoBehaviour
{
    void Update()
    {
        // Check for "B" button press on a controller
        if (Input.GetButtonDown("Fire1"))  // You can change "Fire1" to the actual input axis/button name
        {
            // Close the game
            Application.Quit();
        }
    }
}