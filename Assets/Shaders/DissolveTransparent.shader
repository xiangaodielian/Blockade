// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:Standard,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:14,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33966,y:33088,varname:node_9361,prsc:2|normal-612-RGB,emission-4810-OUT,custl-1247-OUT,alpha-6455-OUT,clip-7583-OUT;n:type:ShaderForge.SFN_NormalVector,id:8665,x:31828,y:32630,prsc:2,pt:True;n:type:ShaderForge.SFN_LightVector,id:5917,x:31810,y:32839,varname:node_5917,prsc:2;n:type:ShaderForge.SFN_Dot,id:2297,x:32366,y:32684,varname:node_2297,prsc:2,dt:1|A-8665-OUT,B-5917-OUT;n:type:ShaderForge.SFN_Multiply,id:1247,x:33337,y:32861,varname:node_1247,prsc:2|A-3827-OUT,B-4911-RGB;n:type:ShaderForge.SFN_LightColor,id:4911,x:33253,y:33032,varname:node_4911,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:7707,x:32366,y:32828,varname:node_7707,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9807,x:32572,y:32695,varname:node_9807,prsc:2|A-2297-OUT,B-7707-OUT;n:type:ShaderForge.SFN_Color,id:3889,x:32360,y:32341,ptovrint:False,ptlb:Diffuse Tint,ptin:_DiffuseTint,varname:node_3889,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:9769,x:32825,y:32597,varname:node_9769,prsc:2|A-5732-OUT,B-9807-OUT;n:type:ShaderForge.SFN_ViewReflectionVector,id:6396,x:31828,y:33192,varname:node_6396,prsc:2;n:type:ShaderForge.SFN_Dot,id:1094,x:32162,y:33306,varname:node_1094,prsc:2,dt:1|A-5917-OUT,B-6396-OUT;n:type:ShaderForge.SFN_Power,id:7420,x:32388,y:33306,varname:node_7420,prsc:2|VAL-1094-OUT,EXP-736-OUT;n:type:ShaderForge.SFN_Exp,id:736,x:32307,y:33509,varname:node_736,prsc:2,et:1|IN-9549-OUT;n:type:ShaderForge.SFN_Slider,id:9549,x:31916,y:33520,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_9549,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7377418,max:1;n:type:ShaderForge.SFN_Multiply,id:1800,x:32635,y:33306,varname:node_1800,prsc:2|A-7420-OUT,B-8366-OUT;n:type:ShaderForge.SFN_Slider,id:8366,x:32503,y:33544,ptovrint:False,ptlb:Specular Intensity,ptin:_SpecularIntensity,varname:node_8366,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5383583,max:1;n:type:ShaderForge.SFN_Add,id:3827,x:33054,y:32840,varname:node_3827,prsc:2|A-9769-OUT,B-1800-OUT;n:type:ShaderForge.SFN_Slider,id:873,x:32222,y:33995,ptovrint:False,ptlb:Blend Amount,ptin:_BlendAmount,varname:node_873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5513676,max:1;n:type:ShaderForge.SFN_Multiply,id:7031,x:32497,y:33821,varname:node_7031,prsc:2|A-3069-R,B-873-OUT;n:type:ShaderForge.SFN_Tex2d,id:3069,x:32283,y:33804,ptovrint:False,ptlb:Dissolve Tex,ptin:_DissolveTex,varname:node_3069,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:9338,x:32663,y:33994,ptovrint:False,ptlb:node_9338,ptin:_node_9338,varname:node_9338,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_Multiply,id:7503,x:32701,y:33821,varname:node_7503,prsc:2|A-7031-OUT,B-9338-OUT;n:type:ShaderForge.SFN_Add,id:8452,x:32894,y:33821,varname:node_8452,prsc:2|A-7503-OUT,B-873-OUT;n:type:ShaderForge.SFN_Power,id:7783,x:33070,y:33821,varname:node_7783,prsc:2|VAL-8452-OUT,EXP-283-OUT;n:type:ShaderForge.SFN_ValueProperty,id:283,x:32894,y:34012,ptovrint:False,ptlb:node_283,ptin:_node_283,varname:node_283,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:50;n:type:ShaderForge.SFN_ConstantClamp,id:7583,x:33253,y:33821,varname:node_7583,prsc:2,min:0,max:1|IN-7783-OUT;n:type:ShaderForge.SFN_If,id:8622,x:33266,y:33396,varname:node_8622,prsc:2|A-461-OUT,B-7783-OUT,GT-5415-OUT,EQ-9615-OUT,LT-9615-OUT;n:type:ShaderForge.SFN_Slider,id:461,x:32898,y:33377,ptovrint:False,ptlb:Edge Width,ptin:_EdgeWidth,varname:node_461,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:80.5303,max:100;n:type:ShaderForge.SFN_ValueProperty,id:5415,x:33055,y:33475,ptovrint:False,ptlb:node_5415,ptin:_node_5415,varname:node_5415,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:9615,x:33055,y:33560,ptovrint:False,ptlb:node_9615,ptin:_node_9615,varname:node_9615,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:8914,x:33256,y:33246,ptovrint:False,ptlb:Edge Color,ptin:_EdgeColor,varname:node_8914,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.8,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:2689,x:33429,y:33256,varname:node_2689,prsc:2|A-8914-RGB,B-8622-OUT;n:type:ShaderForge.SFN_Multiply,id:4810,x:33652,y:33342,varname:node_4810,prsc:2|A-2689-OUT,B-1900-OUT;n:type:ShaderForge.SFN_Slider,id:1900,x:33313,y:33575,ptovrint:False,ptlb:Edge Glow,ptin:_EdgeGlow,varname:node_1900,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:3;n:type:ShaderForge.SFN_Tex2d,id:6576,x:32359,y:32152,ptovrint:False,ptlb:Diffuse Tex,ptin:_DiffuseTex,varname:node_6576,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5732,x:32561,y:32247,varname:node_5732,prsc:2|A-6576-RGB,B-3889-RGB,C-6990-OUT;n:type:ShaderForge.SFN_Tex2d,id:612,x:33718,y:32660,ptovrint:False,ptlb:Normal Map,ptin:_NormalMap,varname:node_612,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:6455,x:33429,y:33159,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_6455,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:6990,x:32203,y:32524,ptovrint:False,ptlb:GlowScale,ptin:_GlowScale,varname:node_6990,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;proporder:6576-3889-612-9549-8366-873-3069-9338-283-461-5415-9615-8914-1900-6455-6990;pass:END;sub:END;*/

Shader "Custom/Transparent/DissolveTransparent" {
    Properties {
        _DiffuseTex ("Diffuse Tex", 2D) = "white" {}
        _DiffuseTint ("Diffuse Tint", Color) = (1,1,1,1)
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Glossiness ("Glossiness", Range(0, 1)) = 0.7377418
        _SpecularIntensity ("Specular Intensity", Range(0, 1)) = 0.5383583
        _BlendAmount ("Blend Amount", Range(0, 1)) = 0.5513676
        _DissolveTex ("Dissolve Tex", 2D) = "white" {}
        [HideInInspector]_node_9338 ("node_9338", Float ) = 4
        [HideInInspector]_node_283 ("node_283", Float ) = 50
        _EdgeWidth ("Edge Width", Range(0, 100)) = 80.5303
        [HideInInspector]_node_5415 ("node_5415", Float ) = 1
        [HideInInspector]_node_9615 ("node_9615", Float ) = 0
        _EdgeColor ("Edge Color", Color) = (0,0.8,1,1)
        _EdgeGlow ("Edge Glow", Range(1, 3)) = 1
        _Opacity ("Opacity", Range(0, 1)) = 1
        _GlowScale ("GlowScale", Range(0, 10)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _DiffuseTint;
            uniform float _Glossiness;
            uniform float _SpecularIntensity;
            uniform float _BlendAmount;
            uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
            uniform float _node_9338;
            uniform float _node_283;
            uniform float _EdgeWidth;
            uniform float _node_5415;
            uniform float _node_9615;
            uniform float4 _EdgeColor;
            uniform float _EdgeGlow;
            uniform sampler2D _DiffuseTex; uniform float4 _DiffuseTex_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Opacity;
            uniform float _GlowScale;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _DissolveTex_var = tex2D(_DissolveTex,TRANSFORM_TEX(i.uv0, _DissolveTex));
                float node_7783 = pow((((_DissolveTex_var.r*_BlendAmount)*_node_9338)+_BlendAmount),_node_283);
                clip(clamp(node_7783,0,1) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
////// Emissive:
                float node_8622_if_leA = step(_EdgeWidth,node_7783);
                float node_8622_if_leB = step(node_7783,_EdgeWidth);
                float3 emissive = ((_EdgeColor.rgb*lerp((node_8622_if_leA*_node_9615)+(node_8622_if_leB*_node_5415),_node_9615,node_8622_if_leA*node_8622_if_leB))*_EdgeGlow);
                float4 _DiffuseTex_var = tex2D(_DiffuseTex,TRANSFORM_TEX(i.uv0, _DiffuseTex));
                float3 finalColor = emissive + ((((_DiffuseTex_var.rgb*_DiffuseTint.rgb*_GlowScale)*(max(0,dot(normalDirection,lightDirection))*attenuation))+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2(_Glossiness))*_SpecularIntensity))*_LightColor0.rgb);
                fixed4 finalRGBA = fixed4(finalColor,_Opacity);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _DiffuseTint;
            uniform float _Glossiness;
            uniform float _SpecularIntensity;
            uniform float _BlendAmount;
            uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
            uniform float _node_9338;
            uniform float _node_283;
            uniform float _EdgeWidth;
            uniform float _node_5415;
            uniform float _node_9615;
            uniform float4 _EdgeColor;
            uniform float _EdgeGlow;
            uniform sampler2D _DiffuseTex; uniform float4 _DiffuseTex_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _Opacity;
            uniform float _GlowScale;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _DissolveTex_var = tex2D(_DissolveTex,TRANSFORM_TEX(i.uv0, _DissolveTex));
                float node_7783 = pow((((_DissolveTex_var.r*_BlendAmount)*_node_9338)+_BlendAmount),_node_283);
                clip(clamp(node_7783,0,1) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _DiffuseTex_var = tex2D(_DiffuseTex,TRANSFORM_TEX(i.uv0, _DiffuseTex));
                float3 finalColor = ((((_DiffuseTex_var.rgb*_DiffuseTint.rgb*_GlowScale)*(max(0,dot(normalDirection,lightDirection))*attenuation))+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2(_Glossiness))*_SpecularIntensity))*_LightColor0.rgb);
                fixed4 finalRGBA = fixed4(finalColor * _Opacity,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _BlendAmount;
            uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
            uniform float _node_9338;
            uniform float _node_283;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _DissolveTex_var = tex2D(_DissolveTex,TRANSFORM_TEX(i.uv0, _DissolveTex));
                float node_7783 = pow((((_DissolveTex_var.r*_BlendAmount)*_node_9338)+_BlendAmount),_node_283);
                clip(clamp(node_7783,0,1) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Standard"
    CustomEditor "ShaderForgeMaterialInspector"
}
