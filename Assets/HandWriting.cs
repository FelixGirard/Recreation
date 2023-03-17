using UnityEngine;

public class HandWriting : MonoBehaviour
{
    public OVRSkeleton rightHandSkeleton;
    public Whiteboard whiteboard;
    public LineRendererPool linePool;

    // Update is called once per frame
    void Update()
    {
        var phalange = rightHandSkeleton.Bones[((int)OVRSkeleton.BoneId.Hand_IndexTip)];
        float length = 0.03f;
        int layerMask = LayerMask.GetMask("Whiteboard");
        Ray ray = new Ray(phalange.Transform.position, -phalange.Transform.up);
        linePool.DrawLine(new Vector3[] { ray.origin, ray.origin + ray.direction * length });
        whiteboard.PerformRaycast(ray, length, layerMask, "RightHand");
    }
}
