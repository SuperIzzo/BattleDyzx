Shader "Hidden/PostShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraMotionVectorsTexture;

            float rand(float2 co, float s)
            {
                return frac((sin(dot(co.xy, float2(12.345 * _Time.w * s, 67.890 * _Time.w * s))) * 12345.67890 + _Time.w));
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 mot = tex2D(_CameraMotionVectorsTexture,i.uv);
                //add motion vectors directly to UV position for sampling color
                fixed2 cell = fixed2(120, 80)*8;
                float r1 = rand(trunc(i.uv * cell)/cell,1);
                float r2 = rand(trunc(i.uv * cell)/cell,2);
                float xr = (r1 - 0.5) * 0.002;
                float yr = (r2 - 0.5) * 0.002;
                float n = (r1 - 0.5) * 0.015;
                float z = pow(frac(i.uv.x * 12000), frac(i.uv.y * 8000))*0.02;
                fixed4 col = tex2D(_MainTex, i.uv + mot.rg * r1) + fixed4(n, n, n, 0) - fixed4(z,z*0.1,-0.1*z,0);
                fixed4 col2 = tex2D(_MainTex, i.uv + fixed2(xr,yr));                
                return lerp(col,col2, r1*r2* 0.4);
            }
            ENDCG
        }
    }
}
