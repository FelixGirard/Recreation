using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LineType { SOLID, DASHED }
public class LineRendererPool : MonoBehaviour
{
    public int initialNumber = 10;
    public GameObject prefabLine;
    public GameObject prefabDashedLine;

    private List<LineRenderer> _LineRenderers = new List<LineRenderer>();
    private List<LineRenderer> _DashedLineRenderers = new List<LineRenderer>();
    private int currentLineUsed = 0;
    private int lastLineUsed = 0;

    private int currentDashedLineUsed = 0;
    private int lastDashedLineUsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        _LineRenderers.Clear();
        _DashedLineRenderers.Clear();
        for (int i = 0; i < initialNumber; i++)
            InstantiateNewLineRenderer(i);
    }

    void InstantiateNewLineRenderer(int index) {
        var newObject = Instantiate(prefabLine, transform);
        newObject.name = index.ToString();
        _LineRenderers.Add(newObject.GetComponent<LineRenderer>());

        var newDashedObject = Instantiate(prefabDashedLine, transform);
        newDashedObject.name = index.ToString();
        _DashedLineRenderers.Add(newDashedObject.GetComponent<LineRenderer>());
    }

    public void DrawLine(Vector3[] points, LineType lineType = LineType.SOLID)
    {
        if (currentLineUsed >= _LineRenderers.Count || currentDashedLineUsed >= _DashedLineRenderers.Count)
            InstantiateNewLineRenderer(currentLineUsed);

        if (lineType == LineType.SOLID)
        {
            _LineRenderers[currentLineUsed].positionCount = points.Length;
            _LineRenderers[currentLineUsed].SetPositions(points);
            currentLineUsed++;
        }
        else if (lineType == LineType.DASHED)
        {
            _DashedLineRenderers[currentDashedLineUsed].positionCount = points.Length;
            _DashedLineRenderers[currentDashedLineUsed].SetPositions(points);
            float width = _DashedLineRenderers[currentDashedLineUsed].startWidth;
            _DashedLineRenderers[currentDashedLineUsed].material.mainTextureScale = new Vector2(1f / width / 3, 1.0f);
            currentDashedLineUsed++;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = currentLineUsed; i <= lastLineUsed; i++)
        {
            _LineRenderers[i].material.mainTextureScale = Vector2.one;
            _LineRenderers[i].positionCount = 2;
            _LineRenderers[i].SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        }
        lastLineUsed = currentLineUsed;
        currentLineUsed = 0;

        for (int i = currentDashedLineUsed; i <= lastDashedLineUsed; i++)
        {
            _DashedLineRenderers[i].material.mainTextureScale = Vector2.one;
            _DashedLineRenderers[i].positionCount = 2;
            _DashedLineRenderers[i].SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        }
        lastDashedLineUsed = currentDashedLineUsed;
        currentDashedLineUsed = 0;
    }
}
