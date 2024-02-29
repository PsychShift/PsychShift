using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class PositionConstraint : MonoBehaviour
{
    [SerializeField] private bool x;
    [SerializeField] private Transform x_Bound1, x_Bound2;
    [SerializeField] private float x_cushion;
    [SerializeField] private bool y;
    [SerializeField] private Transform y_Bound1, y_Bound2;
    [SerializeField] private float y_cushion;
    [SerializeField] private bool z;
    [SerializeField] private Transform z_Bound1, z_Bound2;
    [SerializeField] private float z_cushion;



    private float minX, maxX, minY, maxY, minZ, maxZ;

    void Awake()
    {
        if (x)
        {
            minX = Mathf.Min(x_Bound1.position.x, x_Bound2.position.x) + x_cushion;
            maxX = Mathf.Max(x_Bound1.position.x, x_Bound2.position.x) - x_cushion;
        }
        if (y)
        {
            minY = Mathf.Min(y_Bound1.position.y, y_Bound2.position.y) + y_cushion;
            maxY = Mathf.Max(y_Bound1.position.y, y_Bound2.position.y) - y_cushion;
        }
        if (z)
        {
            minZ = Mathf.Min(z_Bound1.position.z, z_Bound2.position.z) + z_cushion;
            maxZ = Mathf.Max(z_Bound1.position.z, z_Bound2.position.z) - z_cushion;
        }
    }

    public Vector3 ConstrainedPosition(Vector3 position)
    {
        
        Vector3 constrainedPosition = position;

        constrainedPosition.x = x ? Mathf.Clamp(position.x, minX, maxX) : position.x;

        constrainedPosition.y = y ? Mathf.Clamp(position.y, minY, maxY) : position.y;

        constrainedPosition.z = z ? Mathf.Clamp(position.z, minZ, maxZ) : position.z;

        return constrainedPosition;
    }

    void Update()
    {
        transform.position = ConstrainedPosition(transform.position);
    }

}




public class CustomDampedTransform : RigConstraint<DampedTransformJob, DampedTransformData, DampedTransformJobBinder<DampedTransformData>>
{
   
}
