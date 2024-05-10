using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class SensitivityProcessor : InputProcessor<Vector2>
{
    #if UNITY_EDITOR
    static SensitivityProcessor()
    {
        Initialize();
    }
    #endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<SensitivityProcessor>();
    }

    public static float speed = 1;
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        return value * speed;
    }
}
