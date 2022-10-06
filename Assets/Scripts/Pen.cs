using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;

    [HideInInspector]
    public RenderTexture rt;

    private Material material;

    private bool isMouseButtonDown = false;
    private Vector3 lastRelativeHit;
    // Start is called before the first frame update
    void Start()
    {
        rt = new RenderTexture(width, height, 24);
        rt.wrapMode = TextureWrapMode.Repeat;
        rt.enableRandomWrite = true;
        rt.filterMode = FilterMode.Point;
        rt.useMipMap = false;
        rt.Create();

        material = GetComponent<Renderer>().material;
        transform.parent.GetComponent<Renderer>().material.mainTexture = rt;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 boardSize = transform.lossyScale * 10;
                Vector3 relativeHit = hit.point - hit.transform.position + boardSize * 0.5f;
                relativeHit.x = 1 - (relativeHit.y / boardSize.y);
                relativeHit.y = 1 - (relativeHit.y / boardSize.y);

                if (isMouseButtonDown)
                {
                    material.SetVector("_start", lastRelativeHit);
                }
                else
                {
                    material.SetVector("_start", relativeHit);
                    isMouseButtonDown = true;
                }

                material.SetVector("_end", relativeHit);
                lastRelativeHit = relativeHit;
                Graphics.Blit(Texture2D.whiteTexture, rt, material);
            }
            else
                isMouseButtonDown = false;
        }
        else
            isMouseButtonDown = false;
    }
}
