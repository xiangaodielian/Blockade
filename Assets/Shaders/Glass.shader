// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:Standard,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9361,x:34055,y:32666,varname:node_9361,prsc:2|normal-9142-RGB,custl-4249-OUT,alpha-8896-OUT,refract-8860-OUT;n:type:ShaderForge.SFN_Color,id:9710,x:32576,y:32439,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9710,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:8896,x:32765,y:33161,ptovrint:False,ptlb:Transparency,ptin:_Transparency,varname:node_8896,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4754899,max:1;n:type:ShaderForge.SFN_NormalVector,id:1422,x:31399,y:32577,prsc:2,pt:True;n:type:ShaderForge.SFN_LightVector,id:2357,x:31395,y:32897,varname:node_2357,prsc:2;n:type:ShaderForge.SFN_Dot,id:8742,x:31629,y:32644,varname:node_8742,prsc:2,dt:0|A-1422-OUT,B-2357-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2610,x:32067,y:32816,ptovrint:False,ptlb:node_2610,ptin:_node_2610,varname:node_2610,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Add,id:9683,x:32189,y:32650,varname:node_9683,prsc:2|A-718-OUT,B-2610-OUT;n:type:ShaderForge.SFN_Multiply,id:4386,x:32387,y:32650,varname:node_4386,prsc:2|A-9683-OUT,B-2610-OUT;n:type:ShaderForge.SFN_Multiply,id:9610,x:32576,y:32650,varname:node_9610,prsc:2|A-4386-OUT,B-4386-OUT;n:type:ShaderForge.SFN_Multiply,id:498,x:32809,y:32650,varname:node_498,prsc:2|A-9710-RGB,B-9610-OUT;n:type:ShaderForge.SFN_Multiply,id:718,x:31959,y:32650,varname:node_718,prsc:2|A-8742-OUT,B-6974-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:6974,x:31706,y:32814,varname:node_6974,prsc:2;n:type:ShaderForge.SFN_LightColor,id:9026,x:33289,y:32918,varname:node_9026,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4249,x:33483,y:32801,varname:node_4249,prsc:2|A-6716-OUT,B-9026-RGB;n:type:ShaderForge.SFN_ViewReflectionVector,id:6809,x:31395,y:33152,varname:node_6809,prsc:2;n:type:ShaderForge.SFN_Dot,id:2782,x:31706,y:33057,varname:node_2782,prsc:2,dt:1|A-2357-OUT,B-6809-OUT;n:type:ShaderForge.SFN_Power,id:5312,x:32141,y:33072,varname:node_5312,prsc:2|VAL-2782-OUT,EXP-4662-OUT;n:type:ShaderForge.SFN_Exp,id:4662,x:31927,y:33198,varname:node_4662,prsc:2,et:1|IN-2199-OUT;n:type:ShaderForge.SFN_Slider,id:2199,x:31549,y:33303,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_2199,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:8;n:type:ShaderForge.SFN_Slider,id:5250,x:32164,y:33299,ptovrint:False,ptlb:Specular Intensity,ptin:_SpecularIntensity,varname:node_5250,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:3337,x:32443,y:33075,varname:node_3337,prsc:2|A-5312-OUT,B-5250-OUT;n:type:ShaderForge.SFN_Add,id:6716,x:33289,y:32726,varname:node_6716,prsc:2|A-498-OUT,B-3337-OUT,C-7995-OUT;n:type:ShaderForge.SFN_Tex2d,id:9142,x:33062,y:32460,ptovrint:False,ptlb:Normal Map,ptin:_NormalMap,varname:node_9142,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_NormalVector,id:1221,x:32505,y:33447,prsc:2,pt:False;n:type:ShaderForge.SFN_Transform,id:6155,x:32705,y:33447,varname:node_6155,prsc:2,tffrom:0,tfto:3|IN-1221-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3825,x:32928,y:33437,varname:node_3825,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-6155-XYZ;n:type:ShaderForge.SFN_Multiply,id:8860,x:33598,y:33310,varname:node_8860,prsc:2|A-4994-OUT,B-5186-OUT;n:type:ShaderForge.SFN_Slider,id:4994,x:33107,y:33561,ptovrint:False,ptlb:Refraction Scale,ptin:_RefractionScale,varname:node_4994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-0.5,cur:0,max:0.5;n:type:ShaderForge.SFN_ComponentMask,id:165,x:33164,y:33065,varname:node_165,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9142-RGB;n:type:ShaderForge.SFN_Add,id:5186,x:33510,y:33067,varname:node_5186,prsc:2|A-165-OUT,B-3825-OUT;n:type:ShaderForge.SFN_Fresnel,id:2812,x:32702,y:32157,varname:node_2812,prsc:2|NRM-1422-OUT,EXP-9222-OUT;n:type:ShaderForge.SFN_Slider,id:9222,x:32159,y:32165,ptovrint:False,ptlb:Falloff Amount,ptin:_FalloffAmount,varname:node_9222,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:1468,x:33220,y:32147,varname:node_1468,prsc:2|A-2812-OUT,B-9610-OUT;n:type:ShaderForge.SFN_Multiply,id:7995,x:33494,y:32154,varname:node_7995,prsc:2|A-1468-OUT,B-6302-OUT,C-7387-RGB;n:type:ShaderForge.SFN_Slider,id:6302,x:33063,y:31988,ptovrint:False,ptlb:Falloff Intensity,ptin:_FalloffIntensity,varname:node_6302,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:20;n:type:ShaderForge.SFN_Color,id:7387,x:33220,y:32314,ptovrint:False,ptlb:Falloff Color,ptin:_FalloffColor,varname:node_7387,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;proporder:9710-8896-2610-2199-5250-9142-4994-9222-6302-7387;pass:END;sub:END;*/

Shader "Custom/Transparent/Glass" {
    Properties {
        _Color ("Color", Color) = (1,0,0,1)
        _Transparency ("Transparency", Range(0, 1)) = 0.4754899
        _node_2610 ("node_2610", Float ) = 0.5
        _Glossiness ("Glossiness", Range(1, 8)) = 1
        _SpecularIntensity ("Specular Intensity", Range(0, 1)) = 0
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _RefractionScale ("Refraction Scale", Range(-0.5, 0.5)) = 0
        _FalloffAmount ("Falloff Amount", Range(0, 5)) = 0
        _FalloffIntensity ("Falloff Intensity", Range(1, 20)) = 1
        _FalloffColor ("Falloff Color", Color) = (1,1,1,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
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
            uniform sampler2D _GrabTexture;
            uniform float4 _Color;
            uniform float _Transparency;
            uniform float _node_2610;
            uniform float _Glossiness;
            uniform float _SpecularIntensity;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _RefractionScale;
            uniform float _FalloffAmount;
            uniform float _FalloffIntensity;
            uniform float4 _FalloffColor;
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
                float4 screenPos : TEXCOORD5;
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float2 node_3825 = mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_RefractionScale*(_NormalMap_var.rgb.rg+node_3825));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float node_4386 = (((dot(normalDirection,lightDirection)*attenuation)+_node_2610)*_node_2610);
                float node_9610 = (node_4386*node_4386);
                float3 finalColor = (((_Color.rgb*node_9610)+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2(_Glossiness))*_SpecularIntensity)+((pow(1.0-max(0,dot(normalDirection, viewDirection)),_FalloffAmount)*node_9610)*_FalloffIntensity*_FalloffColor.rgb))*_LightColor0.rgb);
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_Transparency),1);
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
            uniform sampler2D _GrabTexture;
            uniform float4 _Color;
            uniform float _Transparency;
            uniform float _node_2610;
            uniform float _Glossiness;
            uniform float _SpecularIntensity;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _RefractionScale;
            uniform float _FalloffAmount;
            uniform float _FalloffIntensity;
            uniform float4 _FalloffColor;
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
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float2 node_3825 = mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_RefractionScale*(_NormalMap_var.rgb.rg+node_3825));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_4386 = (((dot(normalDirection,lightDirection)*attenuation)+_node_2610)*_node_2610);
                float node_9610 = (node_4386*node_4386);
                float3 finalColor = (((_Color.rgb*node_9610)+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2(_Glossiness))*_SpecularIntensity)+((pow(1.0-max(0,dot(normalDirection, viewDirection)),_FalloffAmount)*node_9610)*_FalloffIntensity*_FalloffColor.rgb))*_LightColor0.rgb);
                fixed4 finalRGBA = fixed4(finalColor * _Transparency,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Transparent"
    CustomEditor "ShaderForgeMaterialInspector"
}
