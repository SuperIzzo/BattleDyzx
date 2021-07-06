using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WebImage : MonoBehaviour {
    [SerializeField]
    string url;


    IEnumerator Start()
    {
        var webRequest = UnityWebRequestTexture.GetTexture(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

            Image image = GetComponent<Image>();
            image.overrideSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }
    }
}
