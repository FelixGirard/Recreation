using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public LineRenderer lineLight;
    public LineRendererPool linePool;
    public TextPool textPool;
    private const int MaxRebound = 10;

    private List<Vector3> pointToDraw = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pointToDraw.Clear();
        int layerMask = LayerMask.GetMask("Optic"); 
        int reboundCount = 0;
        Ray ray = new Ray(transform.position + transform.right.normalized * transform.localScale.x / 2, new Vector3(transform.right.x, 0, transform.right.z));
        ray.direction.Scale(new Vector3(1, 0, 1));

        lineLight.positionCount = 1;
        lineLight.SetPosition(0, ray.origin);
        RaycastHit hit;
        while (Physics.Raycast(ray, out hit, 100, layerMask) && reboundCount < MaxRebound)
        {
            ray.origin = hit.point;
            var direction = Vector3.Reflect(ray.direction, hit.normal);
            var normal = hit.normal;
            if (hit.transform.CompareTag("Circle"))
            {
                normal = (hit.point - hit.transform.position).normalized;
                direction = Vector3.Reflect(ray.direction.normalized, normal);
            }

            // Draw the normal
            linePool.DrawLine(new Vector3[] { hit.point, hit.point + normal * 0.25f }, LineType.DASHED);

            Vector3 pointOnIncident = hit.point - ray.direction * 0.06f;
            Vector3 pointOnNormal = hit.point + normal * 0.08f;
            Vector3 pointOnReflected = hit.point + direction * 0.06f;

            // Points for the text
            Vector3 pointOnIncidentFurther = hit.point - ray.direction * 0.08f;
            Vector3 pointOnNormalFurther = hit.point + normal * 0.10f;
            Vector3 pointOnReflectedFurther = hit.point + direction * 0.08f;
            
            linePool.DrawLine(MathHelper.CalculateBezierPoints(pointOnIncident, pointOnNormal, pointOnReflected, 10));

            textPool.DrawText(Vector3.Lerp(pointOnIncidentFurther, pointOnNormalFurther, 0.5f), Vector3.Angle(-ray.direction, normal).ToString("0.##") + "°");
            textPool.DrawText(Vector3.Lerp(pointOnReflectedFurther, pointOnNormalFurther, 0.5f), Vector3.Angle(direction, normal).ToString("0.##") + "°");

            ray.direction = new Vector3(direction.x, 0, direction.z);

            lineLight.positionCount++;
            lineLight.SetPosition(reboundCount + 1, hit.point);
            reboundCount++;
        }
        lineLight.positionCount++;
        lineLight.SetPosition(reboundCount + 1, ray.origin + ray.direction * 100);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pointToDraw.Count; i++)
            Gizmos.DrawSphere(pointToDraw[i], 0.02f);
;    }
}
