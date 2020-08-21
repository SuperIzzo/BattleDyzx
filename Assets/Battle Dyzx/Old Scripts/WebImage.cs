using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebImage : MonoBehaviour {
    [SerializeField]
    string url;


    IEnumerator Start()
    {
        // Start a download of the given URL
        WWW www = new WWW(url);

        // Wait for download to complete
        yield return www;

        // assign texture
        Image image = GetComponent<Image>();
        image.overrideSprite = Sprite.Create(
            www.texture, 
            new Rect( 0, 0, www.texture.width, www.texture.height), 
            new Vector2( 0.5f, 0.5f ) );

        //renderer.material.mainTexture = www.texture;
    }
}
