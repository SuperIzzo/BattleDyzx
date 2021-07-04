using BattleDyzx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyzkInspectorUI : MonoBehaviour
{
    [SerializeField]
    DyzkDatabase data;

    [SerializeField]
    Image image;

    [SerializeField]
    Text text;

    int index = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    public void Next()
    {
        index++;
        if (index >= data.dyzkTextures.Count)
        {
            index = 0;
        }

        UpdateUI();
    }
    public void Prev()
    {
        index--;
        if (index < 0)
        {
            index = data.dyzkTextures.Count - 1;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        var texture = data.dyzkTextures[index];

        image.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));

        DyzkImageAnalysis analysis = new DyzkImageAnalysis(new Texture2DImageData(texture), 0.05f);
        text.text =
            "Radius: " + Math.Round(analysis.maxRadius * 100,2) + "cm\n" +
            "Area: " + Math.Round(analysis.area * 100 * 100, 2) + "cm2\n" +
            "Weight: " + Math.Round(analysis.weight * 1000) + "g\n" +
            "Balance: " + Math.Ceiling(analysis.balance * 100) + "%\n" +
            "Saw: " + Math.Ceiling(analysis.saw * 100) + "%\n";            
    }
}
