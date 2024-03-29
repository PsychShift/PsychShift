using UnityEngine;
using UnityEditor;
using System.IO;

public class UtilitiesEditor
{
    [MenuItem("Utilities/L")]
    public static void L()
    {
        int rand = UnityEngine.Random.Range(0, 3);
        switch (rand)
        {
            case 0 :
            Debug.Log("I am Kevin Kong");
            break;
            case 1 :
            Debug.Log("Carson is fine, i'm just complaining a lot.");
            break;
            case 2 :
            Debug.Log("I lied, thread ripper is bad.");
            break;
            default :
            Debug.Log("I am Kevin Kong");
            break;
        }
    }
}


