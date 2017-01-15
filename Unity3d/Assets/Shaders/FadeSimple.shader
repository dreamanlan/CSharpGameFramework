Shader "Hidden/Custom/FadeSimple" {
    Properties {
        _Color("Main Color", Color) = (0.0,0.0,0.0,0.0)
    }
    SubShader {
        Tags { "Queue" = "Overlay+10000" }
        Pass {
            ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off } 
            Blend DstColor Zero
            CGPROGRAM
              #pragma vertex vert
              #pragma fragment frag
              #pragma fragmentoption ARB_precision_hint_fastest
              
              fixed4 _Color;

              float4 vert(float4 v:POSITION) : SV_POSITION {
                return mul (UNITY_MATRIX_MVP, v);
              }

              fixed4 frag() : COLOR {
                return _Color;
              }
            ENDCG
        }
    }
    Fallback off
}

