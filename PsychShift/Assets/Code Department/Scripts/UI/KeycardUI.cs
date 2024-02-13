using UnityEngine;

[DefaultExecutionOrder(-1)] // Ensures this script runs first
public class KeycardUI : MonoBehaviour
{
    private static KeycardUI instance;

    public static KeycardUI Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    void Awake()
    {
        instance = this;
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}