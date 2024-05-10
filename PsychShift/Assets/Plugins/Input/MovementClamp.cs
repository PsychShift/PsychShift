using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class MovementClamp : InputProcessor<Vector2>
{
    #if UNITY_EDITOR
    static MovementClamp()
    {
        Initialize();
    }
    #endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<MovementClamp>();
    }

    public float max = 200;
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        //value.x = Mathf.Clamp(value.x, -max, max);
        //value.y = Mathf.Clamp(value.y, -max, max);
        return value;
    }
}