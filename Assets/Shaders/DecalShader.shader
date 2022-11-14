Shader "Unlit/DecalShader"
{
    Properties
    {
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 clipPos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float3 worldPosToCamera : TEXCOORD1;
            };

            fixed4 _Color;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _CameraDepthTexture; // Reminder: add DepthTextureMode.Depth flag in GetComponent<Camera>().depthTextureMode to enable this

            float3 getProjectedObjectPos(float2 screenUv, float3 worldPosToCamera)
            {
                // get depth [0, far plane]
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenUv);
                depth = Linear01Depth(depth) * _ProjectionParams.z;

                // "parallelize" the worldPosToCamera ray
                worldPosToCamera = normalize(worldPosToCamera);
                worldPosToCamera /= dot(worldPosToCamera, -UNITY_MATRIX_V[2].xyz); // UNITY_MATRIX_V: View Matrix; index 2 is equivalent to the camera forward vector

                float3 worldPos = _WorldSpaceCameraPos + depth * worldPosToCamera;
                float3 objectPos = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;

                // assuming we use a 1x1 Cube mesh, we clip all pixels that land outside of the cube area; i.e. clip all outside of [-0.5, 0.5]
                clip(0.5 - abs(objectPos));
                objectPos += 0.5;
                return objectPos;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.clipPos = UnityWorldToClipPos(worldPos);
                o.screenPos = ComputeScreenPos(o.clipPos);
                o.worldPosToCamera = worldPos - _WorldSpaceCameraPos;
                return o;
            }

            fixed4 frag(v2f i): SV_Target
            {
                float2 screenUv = i.screenPos.xy / i.screenPos.w;
                float2 uv = getProjectedObjectPos(screenUv, i.worldPosToCamera);

                fixed4 col = tex2D(_MainTex, uv);
                col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
