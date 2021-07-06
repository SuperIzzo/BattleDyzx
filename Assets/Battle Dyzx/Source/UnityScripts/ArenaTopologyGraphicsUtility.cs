using UnityEngine;

using BattleDyzx;

public static class ArenaTopologyGraphicsUtility
{
    public static Color SampleHeightAsColor( this IArenaReliefTopology toplogy, float x, float y )
    {
        float height = toplogy.SampleElevation( x, y );
        height = height / toplogy.depth;
        return new Color( height, height, height );
    }

    public static Color SampleNormalAsColor( this IArenaNormalTopology toplogy, float x, float y )
    {
        // TODO: Mobile uses usual normal map format, Desktop uses r = y, a = x
        // This is the desktop version
        Vector3D normal = toplogy.SampleNormal( x, y );
        float xNormal = ( normal.x + 1 ) / 2;
        float yNormal = ( normal.y + 1 ) / 2;
        Color color = new Color(xNormal, yNormal, normal.z, 1.0f );
        return color;
    }

    public static Texture2D CreateHeightMapTexture( this IArenaReliefTopology toplogy )
    {
        Texture2D texture = new Texture2D( (int) toplogy.width, (int) toplogy.height );
        for( int x = 0; x < texture.width; x++ )
        {
            for( int y = 0; y < texture.width; y++ )
            {
                Color heightColor = toplogy.SampleHeightAsColor( x, y );
                texture.SetPixel( x, y, heightColor );
            }
        }
        texture.Apply();
        return texture;
    }

    public static Texture2D CreateNormalMapTexture( this IArenaNormalTopology toplogy )
    {
        Texture2D texture = new Texture2D( (int) toplogy.width, (int) toplogy.height );
        for( int x = 0; x < texture.width; x++ )
        {
            for( int y = 0; y < texture.width; y++ )
            {
                Color normalColor = toplogy.SampleNormalAsColor( x, y );
                texture.SetPixel( x, y, normalColor );
            }
        }
        texture.Apply();
        return texture;
    }
}