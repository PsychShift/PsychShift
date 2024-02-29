using UnityEngine.Animations.Rigging;
using UnityEngine;

public class IKGrounding : MonoBehaviour
{
    [SerializeField] private Transform footTransformRF, footTransformLF;
    private Transform[] allFootTransforms;
    [SerializeField] private Transform footTargetTransformRF, footTargetTransformLF;
    private Transform[] allTargetTransforms;

    [SerializeField] private GameObject footRigRF, footRigLF;
    private TwoBoneIKConstraint[] allFootIKConstraints;

    [SerializeField] private Animator animator;

    private float[] allFootWeights;

    private LayerMask groundLayerMask;
    private float maxHitDistance = 5f;
    private float addedHeight = 3f;
    private bool[] allGroundSpherecastHits;
    private LayerMask hitLayer;
    private Vector3[] allHitNormals;
    private float angleAboutX;
    private float angleAboutZ;
    private float yOffset = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        allFootTransforms = new Transform[2];
        allFootTransforms[0] = footTransformRF;
        allFootTransforms[1] = footTransformLF;

        allTargetTransforms = new Transform[2];
        allTargetTransforms[0] = footTargetTransformRF;
        allTargetTransforms[1] = footTargetTransformLF;

        allFootIKConstraints = new TwoBoneIKConstraint[2];
        allFootIKConstraints[0] = footRigRF.GetComponent<TwoBoneIKConstraint>();
        allFootIKConstraints[1] = footRigLF.GetComponent<TwoBoneIKConstraint>();

        groundLayerMask = LayerMask.NameToLayer("Ground");

        allGroundSpherecastHits = new bool[3];
        allHitNormals = new Vector3[2];
        allFootWeights = new float[2];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateCharacterFeet();
    }

    private void CheckGroundBelow(out Vector3 hitPoint, out bool gotGroundSpherecastHit, out Vector3 hitNormal, out LayerMask hitLayer,
        out float currentHitDistance, Transform objectTransform, int checkForLayerMask, float maxHitDistance, float addedHeight)
    {
        RaycastHit hit;
        Vector3 startSpherecast = objectTransform.position + new Vector3(0f, addedHeight, 0f);

        if(checkForLayerMask == -1)
        {
            Debug.LogError("Layer does not exist");
            gotGroundSpherecastHit = false;
            currentHitDistance = 0;
            hitLayer = LayerMask.NameToLayer("Player");
            hitNormal = Vector3.up;
            hitPoint = objectTransform.position;
        }
        else
        {
            int layerMask = (1 << checkForLayerMask);
            if(Physics.SphereCast(startSpherecast, .2f, Vector3.down, out hit, maxHitDistance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                hitLayer = hit.transform.gameObject.layer;
                currentHitDistance = hit.distance - addedHeight;
                hitNormal = hit.normal;
                gotGroundSpherecastHit = true;
                hitPoint = hit.point;
            }
            else
            {
                gotGroundSpherecastHit = false;
                currentHitDistance = 0;
                hitLayer = LayerMask.NameToLayer("Player");
                hitNormal = Vector3.up;
                hitPoint = objectTransform.position;
            }
        }
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector, Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal);
    }

    private void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform footTargetTransform, Vector3 hitNormal)
    {
        Vector3 xAxisProjected = ProjectOnContactPlane(footTargetTransform.forward, hitNormal).normalized;
        Vector3 zAxisProjected = ProjectOnContactPlane(footTargetTransform.right, hitNormal).normalized;
        
        angleAboutX = Vector3.SignedAngle(footTargetTransform.forward, xAxisProjected, footTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(footTargetTransform.right, zAxisProjected, footTargetTransform.forward);
    }

    private void RotateCharacterFeet()
    {
        allFootWeights[0] = animator.GetFloat("RF FootWeight");
        allFootWeights[1] = animator.GetFloat("LF FootWeight");

        for(int i = 0; i < allTargetTransforms.Length; i++)
        {
            allFootIKConstraints[i].weight = allFootWeights[i];
            
            CheckGroundBelow(out Vector3 hitPoint, out allGroundSpherecastHits[i], out Vector3 hitNormal, out hitLayer, out _,
                allFootTransforms[i], groundLayerMask, maxHitDistance, addedHeight);
            allHitNormals[i] = hitNormal;

            if(allGroundSpherecastHits[i]) //true
            {
                ProjectedAxisAngles(out angleAboutX, out angleAboutZ, allFootTransforms[i], hitNormal);

                allTargetTransforms[i].position = new Vector3(allFootTransforms[i].position.x, hitPoint.y + yOffset, allFootTransforms[i].position.z);

                allTargetTransforms[i].rotation = allTargetTransforms[i].rotation;

                allTargetTransforms[i].localEulerAngles = new Vector3
                    (allTargetTransforms[i].localEulerAngles.x + angleAboutX, 
                    allTargetTransforms[i].localEulerAngles.y, 
                    allTargetTransforms[i].localEulerAngles.z + angleAboutZ);
                
            }
            else //false
            {
                allTargetTransforms[i].position = allTargetTransforms[i].position;
                allTargetTransforms[i].rotation = allTargetTransforms[i].rotation;
            }
        }
    }
}
