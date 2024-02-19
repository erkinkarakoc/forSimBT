Shader "Erkin/ProjectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent-400"}
        Cull Front
        ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 screenPos : TEXCOORD0;
                
                float4 position : SV_POSITION;
                float3 ray : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D_float _CameraDepthTexture;

            float3 getProjectedObjectPos(float2 screenPos, float3 worldRay)
            {
	
	            float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPos);
	            depth = Linear01Depth (depth) * _ProjectionParams.z;
	            worldRay = normalize(worldRay);
                worldRay /= dot(worldRay, -UNITY_MATRIX_V[2].xyz);
                float3 worldPos = _WorldSpaceCameraPos + worldRay * depth;
	            float3 objectPos =  mul (unity_WorldToObject, float4(worldPos,1)).xyz;
	            clip(0.5 - abs(objectPos));
                objectPos += 0.5;
                return objectPos;
}

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.position = UnityWorldToClipPos(worldPos);
                o.ray = worldPos - _WorldSpaceCameraPos;
                o.screenPos = ComputeScreenPos(o.position);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
               
                float2 screenUv = i.screenPos.xy / i.screenPos.w;
                float2 uv = getProjectedObjectPos(screenUv, i.ray).xz;
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
