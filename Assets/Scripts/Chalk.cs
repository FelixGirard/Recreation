using UnityEngine;

public class Chalk : MonoBehaviour
{
    public Whiteboard whiteboard; // Board to draw on
    public LineRendererPool linePool;
    public string name;

    // Update is called once per frame
    void Update()
    {
        float halfLength = transform.localScale.y * 1.05f;
        int layerMask = LayerMask.GetMask("Whiteboard");
        Ray ray = new Ray(transform.position, -transform.up);
        linePool.DrawLine(new Vector3[] { ray.origin, ray.origin + ray.direction * halfLength });
        whiteboard.PerformRaycast(ray, halfLength, layerMask, name);
    }
}
