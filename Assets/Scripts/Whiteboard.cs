using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;

    [HideInInspector]
    public RenderTexture rt;

    public Material material;

    private Dictionary<string, bool> activePens = new Dictionary<string, bool>();
    private Vector3 lastRelativeHit;
    // Start is called before the first frame update
    void Start()
    {
        rt = new RenderTexture(width, height, 0);
        rt.wrapMode = TextureWrapMode.Repeat;
        rt.enableRandomWrite = true;
        rt.filterMode = FilterMode.Point;
        rt.useMipMap = false;
        rt.Create();

        RenderTexture.active = rt;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = rt;

        material = new Material(Shader.Find("Pen Blit"));
        transform.GetComponent<Renderer>().material.mainTexture = rt;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            PerformRaycast(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    public void PerformRaycast(Ray ray, float maxDistance = 100, int layerMask = 0, string name = "")
    {
        if (!activePens.ContainsKey(name))
            activePens.Add(name, false);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            Vector3 boardSize = new Vector3(transform.localScale.x * 10, transform.localScale.z * 10, transform.localScale.y * 10);
            // Point relative to the bottom left corner
            Vector3 bottomLeftCorner = transform.position - transform.right * (boardSize.x / 2) - transform.forward * (boardSize.y / 2);
            Vector3 relativeHit = hit.point - bottomLeftCorner;
            Vector3 relativeX = Vector3.Project(relativeHit, transform.right);
            Vector3 relativeY = Vector3.Project(relativeHit, transform.forward);

            relativeHit.x = 1 - (relativeX.magnitude / boardSize.x);
            relativeHit.y = 1 - (relativeY.magnitude / boardSize.y);


            if (activePens[name])
                material.SetVector("_start", lastRelativeHit);
            else
            {
                material.SetVector("_start", relativeHit);
                activePens[name] = true;
            }

            material.SetVector("_end", relativeHit);
            lastRelativeHit = relativeHit;
            Graphics.Blit(Texture2D.whiteTexture, rt, material);
        }
        else
            activePens[name] = false;
    }
}
