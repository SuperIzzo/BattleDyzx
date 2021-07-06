Shader "Custom/Dyzx"
{
	Properties 
    {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}
        _Glossiness( "Smoothness", Range( 0,1 ) ) = 0.5
        _Metallic( "Metallic", Range( 0,1 ) ) = 0.0
        _SpinBlur( "Spin Blur", float ) = 0.0
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _SpinBlur;
        sampler2D _MainTex;

		struct Input
        {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutputStandard o)
        {
            const int NUM_ROTATIONS = 20;

            fixed4 finalColor = float4(0, 0, 0, 0);
            float2 center = float2( 0.5, 0.5 );
            float2 relPos = IN.uv_MainTex - center;

            float progression = 0.0;

            for( int i = 0; i<NUM_ROTATIONS; i++ )
            {
                float angleFract = -_SpinBlur * float(i) / float( NUM_ROTATIONS );
            
                float cs = cos( angleFract );
                float sn = sin( angleFract );
                float2 newPos = float2( relPos.x*cs - relPos.y*sn, relPos.x*sn + relPos.y*cs );
                newPos += center;

                fixed4 tex = tex2D( _MainTex, newPos);     
                float rate = pow( float( i ), 1.2 ) * (tex.a + 0.4);
                finalColor += tex * rate;

                progression = progression + rate;
            }

			// Albedo comes from a texture tinted by color
			fixed4 c = (finalColor/progression) * _Color;

			o.Albedo = c.rgb;
            o.Alpha = c.a;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
