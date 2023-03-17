using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPool : MonoBehaviour
{
    public int initialNumber = 10;
    public GameObject prefabText;

    private List<TMPro.TMP_Text> _Text = new List<TMPro.TMP_Text>();
    private int currentTextUsed = 0;
    private int lastTextUsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        _Text.Clear();
        for (int i = 0; i < initialNumber; i++)
            InstantiateNewTextRenderer(i);
    }

    void InstantiateNewTextRenderer(int index)
    {
        var newObject = Instantiate(prefabText, transform);
        newObject.name = index.ToString();
        _Text.Add(newObject.GetComponentInChildren<TMPro.TMP_Text>());
    }

    public void DrawText(Vector3 position, string text)
    {
        if (currentTextUsed >= _Text.Count)
            InstantiateNewTextRenderer(currentTextUsed);

        _Text[currentTextUsed].transform.parent.transform.position = position;
        _Text[currentTextUsed].text = text;
        currentTextUsed++;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = currentTextUsed; i <= System.Math.Min(lastTextUsed, _Text.Count - 1); i++)
            _Text[i].text = "";

        lastTextUsed = currentTextUsed;
        currentTextUsed = 0;
    }
}
