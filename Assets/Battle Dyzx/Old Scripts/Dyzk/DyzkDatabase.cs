using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyzkLoader
{
    static bool _cacheImages = true;
    static Dictionary<string, DyzkData> _cachedData;
    static Dictionary<string, Texture2D> _cachedImages;

    public Texture2D texture  { get; private set; }
    public DyzkData  dyzkData { get; private set; }

    public string imageURI { get; private set; }
    public bool isTextureLoaded { get; private set; }
    public bool isDataLoaded { get; private set; }


    public DyzkLoader()
    {
        this.imageURI = imageURI;
    }

    public static DyzkData GetDyzkData( string imageURI )
    {
        if( _cachedData.ContainsKey( imageURI ) )
        {
            return _cachedData[ imageURI ];
        }
        else
        {
            Debug.LogError( "Dyzk data for \"" + imageURI + "\" not loaded" );
            return new DyzkData();
        }
    }


    public IEnumerator Load( string imageURI, bool loadImage = false )
    {
        Texture2D texture;

        if( loadImage )
        {

        }
                
        if( _cachedData.ContainsKey( imageURI ) )
        {
            yield break;
        }
        else
        {
            WWW www = new WWW(imageURI);

            yield return www;

            if( !string.IsNullOrEmpty( www.error ) )
            {
                Debug.LogError( www.error );
            }
            else
            {
                DyzkAnalysis analysis = new DyzkAnalysis();
                analysis.AnalyzeTexture( www.texture );

                _cachedData[imageURI] = analysis.dyzkData;

                if( _cacheImages )
                    _cachedImages[imageURI] = www.texture;
            }
        }
    }

    public void Load( Texture2D texture )
    {

    }
}
