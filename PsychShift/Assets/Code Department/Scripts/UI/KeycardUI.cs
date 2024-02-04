using UnityEngine;

public class KeycardUI : MonoBehaviour
{
    private static KeycardUI instance;

    public static KeycardUI Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
