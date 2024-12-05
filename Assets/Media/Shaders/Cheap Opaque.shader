
Shader "Cheap Opaque" {
 
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)

    }
 
    SubShader {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}

        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Lambert vertex:vert addshadow

            
            sampler2D _MainTex;
            fixed4 _Color;

                //Structs
                struct Input {
                    float2 uv_MainTex;
                };

                // Vertex Manipulation Function
                void vert (inout appdata_full i) {

                }

                // Surface Shader
                void surf (Input IN, inout SurfaceOutput o) {
                    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                    o.Albedo = c.rgb;
                    o.Alpha = c.a;
                }

        ENDCG
        }
     
    Fallback "Diffuse"
} 
