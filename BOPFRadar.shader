Shader "Unlit/RadarEffect"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0,0.2,0.8,0.2)
        _SweepColor ("Sweep Color", Color) = (0,0.2,0.8,0.2)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #define mod(x, y) (x - y * floor(x / y))
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            fixed4 _MainColor;
            fixed4 _SweepColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            float3 RadarPing(in float2 uv, in float2 center, in float innerTail, 
               in float frontierBorder, in float timeResetSeconds, 
               in float radarPingSpeed, in float fadeDistance)
            {
                float2 diff = center-uv;
                float r = length(diff);
                float time = mod(_Time.z, timeResetSeconds) * radarPingSpeed;
   
                float circle = 0;
                
                circle += smoothstep(time - innerTail, time, r) * smoothstep(time + frontierBorder,time, r);
	            circle *= smoothstep(fadeDistance, 0.0, r); 
                return float3(circle,circle,circle);
            }
            fixed4 frag (v2f i) : SV_Target
            {
                float2 _uv = i.uv.xy * 2.0;
                _uv += float2(-1.0,-1.0);
                _uv.x *= _ScreenParams.x / _ScreenParams.y; 
                float4 color = _MainColor;
                float fadeDistance = 4.0;
                float resetTimeSec = 8.0;
                float radarPingSpeed = 0.3;
                float2 greenPing = float2(0.0,0.0);
                color.xyz += RadarPing(_uv, greenPing,0.25,0.025,resetTimeSec,radarPingSpeed,fadeDistance) * _SweepColor.xyz;
                
                return color;
            }
            ENDCG
        }
    }
}
