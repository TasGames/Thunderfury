// By Mochie
// Free for public usage and editing (just don't change the name, it'll break things)
// Version 1.0

Shader "Mochie/Mochie's Uber Shader (Opaque)" {
    Properties {
        
            // Basic
            [Enum(BASIC,0, SHADED,1, PBR,2)]_Mode("Mode", Int) = 0
            [Enum(OFF,0, BACK,2)]_Culling("Culling", Int) = 2
            _Color("", Color) = (1,1,1,1)
            _MainTex("", 2D) = "white" {}
            _EmissTex("", 2D) = "white" {}
            [Toggle]_AdvancedEmiss("", Int) = 0
            _EmissMask("", 2D) = "white" {}
            _XScroll("", Range(-2,2)) = 0
            _YScroll("", Range(-2,2)) = 0
            [HDR]_EmissCol("", Color) = (0,0,0,1)
            [Toggle]_ReactToggle("", Int) = 0
            _Crossfade("", Range(0,0.2)) = 0.1
            _ReactThresh("", Range(0,1)) = 0.5
            _Saturation("", Range(0,2)) = 1

            // Non-basic
            [Toggle]_UseStatic("", Int) = 0
            _StaticDir("", Vector) = (0,1,0.75,0)
            _RimCol("", Color) = (1,1,1)
            _RimStrength("", Range(0,1)) = 0
            _RimWidth("", Range (0,1)) = 0.5
            _RimEdge("", Range(0,0.5)) = 0
            _NormalStrength("", Range(-2,2)) = 1
            _NormalTex("", 2D) = "bump" {}

            // Toon Shaded
            _Shadow("", Range(0,1)) = 0.5
            _Direct("", Range(1,99.99)) = 50
            _Gradient("", Range(3, 100)) = 50

            // PBR
            [Toggle(DETAIL)]_Detail("", Int) = 0
            _DetailStrength("", Range(-2,2)) = 1
            _DetailNormal("", 2D) = "bump" {}
            _Metallic("", Range(0,1)) = 0
            _MetallicTex("", 2D) = "white" {}
            _Roughness("", Range(0,1)) = 0.5
            _RoughnessTex("", 2D) = "white" {}
            _AOStrength("", Range(0,1)) = 1
            _AOTex("", 2D) = "white" {}
            _HeightTex("", 2D) = "white" {}
            _HeightStrength("", Range(0,0.1)) = 0
            [Toggle(HEIGHT)]_Height("", Int) = 0
            [Toggle(MARCH)]_March("", Int) = 0
            [Toggle(REFLCUBE)]_UseReflCube("", Int) = 0
            _ReflCube("", CUBE) = "white" {}
    }

    SubShader {
        Tags {"RenderType"="Opaque" "Queue"="Geometry"}
        Cull [_Culling]
        Pass {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature PBR
            #pragma shader_feature SHADED
            #pragma shader_feature DETAIL
            #pragma shader_feature HEIGHT
            #pragma shader_feature MARCH
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 5.0
            #include "UnityShaderVariables.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityLightingCommon.cginc"
            #include "Autolight.cginc"

            sampler2D _MainTex, _EmissTex, _EmissMask;
            float4 _MainTex_ST, _EmissTex_ST, _Color, _EmissCol;
            float _Saturation, _ReactThresh, _Crossfade, _XScroll, _YScroll;
            int _ReactToggle, _UseReflCube, _AdvancedEmiss;

            #ifdef SHADED
                float  _Shadow, _Gradient, _Direct;
            #endif

            #ifdef PBR
                sampler2D _MetallicTex, _RoughnessTex, _AOTex;
                samplerCUBE _ReflCube;
                float3 specularTint;
                float omr, _Roughness, _Metallic, _AOStrength;
                #ifdef DETAIL
                    sampler2D _DetailNormal;
                    float4 _DetailNormal_ST;
                    float _DetailStrength;
                #endif
                #ifdef HEIGHT
                    sampler2D _HeightTex;
                    float _HeightStrength;
                #endif
            #endif

            #if defined(SHADED) || defined(PBR)
                sampler2D _NormalTex;
                float3 _RimCol, _StaticDir;
                float _RimStrength, _RimWidth, _RimEdge, _NormalStrength;
                int _UseStatic;
            #endif
            
			struct appdata {
				float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                #if defined(SHADED) || defined(PBR)
                    float4 tangent : TANGENT;
                    float3 normal : NORMAL;
                #endif
			};

            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                #if defined(SHADED) || defined(PBR)
                    float3 worldPos : TEXCOORD1;
                    float4 tangent : TEXCOORD2;
                    float3 normal : NORMAL;
                    float3 binormal : TEXCOORD3;
                #endif
                #ifdef PBR
                    #ifdef DETAIL
                        float2 uvd : TEXCOORD4;
                    #endif
                    #ifdef HEIGHT
                        float3 tangentViewDir : TEXCOORD5;
                    #endif
                    LIGHTING_COORDS(8,9)
                #endif
                UNITY_FOG_COORDS(10)
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _EmissTex);
                o.uv.z += (_Time.y*_XScroll)*_AdvancedEmiss;
                o.uv.w += (_Time.y*_YScroll)*_AdvancedEmiss;
                #if defined(SHADED) || defined(PBR)
                    o.normal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
                    o.binormal = cross(o.normal, o.tangent.xyz) * (v.tangent.w * unity_WorldTransformParams.w);
                #endif
                #ifdef PBR
                    #ifdef DETAIL
                        o.uvd = TRANSFORM_TEX(v.uv, _DetailNormal);
                    #endif
                    #ifdef HEIGHT
                        v.tangent.xyz = normalize(v.tangent.xyz);
			            v.normal = normalize(v.normal);
                        float3x3 objectToTangent = float3x3(v.tangent.xyz, (cross(v.normal, v.tangent.xyz) * v.tangent.w), v.normal);
                        o.tangentViewDir = mul(objectToTangent, ObjSpaceViewDir(v.vertex));
                    #endif
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                #endif
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            float4 frag (v2f i) : SV_Target {

                #if defined(PBR) && defined(HEIGHT)
                    i.tangentViewDir = normalize(i.tangentViewDir);
                    i.tangentViewDir.xy /= (i.tangentViewDir.z + 0.42);
                    #ifndef MARCH
                        float height = tex2D(_HeightTex, i.uv.xy);
                        height -= 0.5;
                        height *= _HeightStrength;
                        float2 uvOffset = i.tangentViewDir.xy * height;
                    #else
                        float2 uvOffset = 0;
                        float stepSize = 0.1;
                        float2 uvDelta = i.tangentViewDir.xy * (stepSize * _HeightStrength);
                        float stepHeight = 1;
                        float surfaceHeight = tex2D(_HeightTex, i.uv.xy);
                        float2 prevUVOffset = uvOffset;
                        float prevStepHeight = stepHeight;
                        float prevSurfaceHeight = surfaceHeight;

                        [unroll(10)]
                        for (int j = 1; j < 10 && stepHeight > surfaceHeight; j++){
                            prevUVOffset = uvOffset;
                            prevStepHeight = stepHeight;
                            prevSurfaceHeight = surfaceHeight;
                            uvOffset -= uvDelta;
                            stepHeight -= stepSize;
                            surfaceHeight = tex2D(_HeightTex, i.uv.xy+uvOffset);
                        }
                        
                        float prevDifference = prevStepHeight - prevSurfaceHeight;
                        float difference = surfaceHeight - stepHeight;
                        float t = prevDifference / (prevDifference + difference);
                        uvOffset = prevUVOffset - uvDelta * t;
                    #endif
                    i.uv.xy += uvOffset;
                    #ifdef DETAIL
                        i.uvd += uvOffset * (_DetailNormal_ST.xy / _MainTex_ST.xy);
                    #endif
                #endif

                float4 albedo = tex2D(_MainTex, i.uv.xy);
                albedo *= _Color;
                albedo.rgb = lerp(dot(albedo.rgb, float3(0.3,0.59,0.11)), albedo.rgb, _Saturation);
                float4 diffuse = albedo;

                float3 emiss = tex2D(_EmissTex, i.uv.zw);
                emiss = lerp(dot(emiss, float3(0.3,0.59,0.11)), emiss, _Saturation);
                emiss *= _EmissCol;
                if (_AdvancedEmiss == 1)
                    emiss.rgb *= tex2D(_EmissMask, i.uv.xy);
                
                float3 lightCol = saturate(ShadeSH9(float4(0,0,0,1)) + _LightColor0);
                #ifdef PBR
                    lightCol = saturate(lightCol);
                #endif 
                #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT) 
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
                #else
                    float3 lightDir = _WorldSpaceLightPos0.xyz;
                #endif

                #if defined(SHADED) || defined(PBR)
                    if (_UseStatic)
                        lightDir = _StaticDir;
                    #if UNITY_SINGLE_PASS_STEREO
                        float3 cameraPos = (unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1])*0.5;
                    #else
                        float3 cameraPos = _WorldSpaceCameraPos;
                    #endif
                    float3 viewDir = normalize(cameraPos - i.worldPos);
                    float3 halfVector = normalize(lightDir + viewDir);
                    float3 normalTex = UnpackScaleNormal(tex2D(_NormalTex, i.uv.xy), _NormalStrength);
                    #ifdef DETAIL
                        float3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormal, i.uvd), _DetailStrength);
                        float3 tangentNormals = BlendNormals(normalTex, detailNormal);
                    #else
                        float3 tangentNormals = normalize(normalTex);
                    #endif
                    float3 binormal = i.binormal;
                    float3 normalDir = normalize(tangentNormals.x * i.tangent + tangentNormals.y * binormal + tangentNormals.z *i.normal);
                    float NdotL = DotClamped(normalDir, lightDir);

                    // Rim lighting
                    float rimDot = abs(dot(viewDir, normalDir));
                    float rim = pow((1-rimDot), (1-_RimWidth) * 10);
                    rim = smoothstep(_RimEdge, (1-_RimEdge), rim);
                    #ifdef PBR
                        albedo.rgb = lerp(albedo.rgb, _RimCol, (rim*_RimStrength));
                    #else
                        diffuse.rgb = lerp(diffuse.rgb, _RimCol, (rim*_RimStrength));
                    #endif
                #endif
                
                // Toon Shading
                #ifdef SHADED
                    [branch]if (!any(_LightColor0) && _UseStatic == 0)
                        _Shadow = 1;
                    else
                        _Shadow = 1-_Shadow;
                    _Direct = min(_Direct, _Gradient-0.0001);
                    lightCol = lightCol*(saturate(floor(NdotL * _Gradient) / (_Gradient-_Direct)) + _Shadow);
                    diffuse.rgb *= lightCol;
                #endif

                // PBR Shading
                #ifdef PBR

                    float metallic = tex2D(_MetallicTex, i.uv.xy) * _Metallic;
                    float roughness = tex2D(_RoughnessTex, i.uv.xy) * _Roughness;
                    float smoothness = 1-roughness;
                    roughness *= 1.7-0.7*roughness;
                    float ao = lerp(1, tex2D(_AOTex, i.uv.xy).g, _AOStrength);
                    float3 reflectionDir = reflect(-viewDir, normalDir);
                    
                    // Direct Lighting
                    UnityLight light;
                    light.color = lightCol * LIGHT_ATTENUATION(i);
                    light.dir = lightDir;

                    // Indirect Lighting
                    UnityIndirect indirectLight;
                    indirectLight.diffuse = max(0, ShadeSH9(float4(normalDir,1))) * ao;
                    float3 reflections = float3(0,0,0);
                    [branch]if (_UseReflCube == 1){
                        reflections = texCUBElod(_ReflCube, float4(reflectionDir, roughness * UNITY_SPECCUBE_LOD_STEPS)) * ao;
                    }
                    else {
                        float4 envSample = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflectionDir, roughness * UNITY_SPECCUBE_LOD_STEPS);
                        reflections = DecodeHDR(envSample, unity_SpecCube0_HDR) * ao;
                    }
                    indirectLight.specular = reflections;

                    albedo.rgb = DiffuseAndSpecularFromMetallic(albedo, metallic, specularTint, omr);
                    diffuse = UNITY_BRDF_PBS(albedo, specularTint.rgb, omr, smoothness, normalDir, viewDir, light, indirectLight);
                #endif

                #if !defined(SHADED) && !defined(PBR)
                    diffuse.rgb *= lightCol;
                #endif

                #if !defined(PBR)
                    float lightColor = saturate((lightCol.r + lightCol.g + lightCol.b)/3);
                #else  
                    float3 ild = indirectLight.diffuse;
                    float3 lc = light.color;
                    float lightColor = saturate((ild.r + ild.g + ild.b + lc.r + lc.g + lc.b)/6);
                #endif

                float2 threshold = saturate(float2(_ReactThresh-_Crossfade, _ReactThresh+_Crossfade));
                float3 brightness = smoothstep(threshold.x, threshold.y, lightColor)*_ReactToggle*_AdvancedEmiss;
                float3 emission = lerp(emiss, 0, brightness);
                diffuse.rgb += emission;
                UNITY_APPLY_FOG(i.fogCoord, diffuse);
				return diffuse;
            }
            ENDCG
        }

        Pass {
            Tags {"RenderType"="Transparent" "LightMode"="ForwardAdd"}
            Blend One One
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature PBR
            #pragma shader_feature SHADED
            #pragma shader_feature DETAIL
            #pragma shader_feature HEIGHT
            #pragma shader_feature MARCH
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma target 5.0
            #include "UnityShaderVariables.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityLightingCommon.cginc"
            #include "Autolight.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST, _Color;
            float _Saturation;
            int _UseReflCube;

            #ifdef SHADED
                float _Shadow, _Gradient, _Direct;
            #endif

            #ifdef PBR
                sampler2D _MetallicTex, _RoughnessTex, _AOTex;
                samplerCUBE _ReflCube;
                float3 specularTint;
                float omr, _Roughness, _Metallic, _AOStrength;
                #ifdef DETAIL
                    sampler2D _DetailNormal;
                    float4 _DetailNormal_ST;
                    float _DetailStrength;
                #endif
                #ifdef HEIGHT
                    sampler2D _HeightTex;
                    float _HeightStrength;
                #endif
            #endif

            #if defined(SHADED) || defined(PBR)
                sampler2D _NormalTex;
                float3 _RimCol, _StaticDir;
                float _RimStrength, _RimWidth, _RimEdge, _NormalStrength;
                int _UseStatic;
            #endif
            
			struct appdata {
				float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                #if defined(SHADED) || defined(PBR)
                    float4 tangent : TANGENT;
                    float3 normal : NORMAL;
                #endif
			};

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                #if defined(SHADED) || defined(PBR)
                    float4 tangent : TEXCOORD2;
                    float3 normal : NORMAL;
                    float3 binormal : TEXCOORD3;
                #endif
                #ifdef PBR
                    #ifdef DETAIL
                        float2 uvd : TEXCOORD4;
                    #endif
                    #ifdef HEIGHT
                        float3 tangentViewDir : TEXCOORD5;
                    #endif
                #endif
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                #if defined(SHADED) || defined(PBR)
                    o.normal = UnityObjectToWorldNormal(v.normal);
                    o.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
                    o.binormal = cross(o.normal, o.tangent.xyz) * (v.tangent.w * unity_WorldTransformParams.w);
                #endif
                #ifdef PBR
                    #ifdef DETAIL
                        o.uvd = TRANSFORM_TEX(v.uv, _DetailNormal);
                    #endif
                    #ifdef HEIGHT
                        v.tangent.xyz = normalize(v.tangent.xyz);
			            v.normal = normalize(v.normal);
                        float3x3 objectToTangent = float3x3(v.tangent.xyz, (cross(v.normal, v.tangent.xyz) * v.tangent.w), v.normal);
                        o.tangentViewDir = mul(objectToTangent, ObjSpaceViewDir(v.vertex));
                    #endif
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            float4 frag (v2f i) : SV_Target {

                #if defined(PBR) && defined(HEIGHT)
                    i.tangentViewDir = normalize(i.tangentViewDir);
                    i.tangentViewDir.xy /= (i.tangentViewDir.z + 0.42);
                    #ifndef MARCH
                        float height = tex2D(_HeightTex, i.uv.xy);
                        height -= 0.5;
                        height *= _HeightStrength;
                        float2 uvOffset = i.tangentViewDir.xy * height;
                    #else
                        float2 uvOffset = 0;
                        float stepSize = 0.1;
                        float2 uvDelta = i.tangentViewDir.xy * (stepSize * _HeightStrength);
                        float stepHeight = 1;
                        float surfaceHeight = tex2D(_HeightTex, i.uv.xy);
                        float2 prevUVOffset = uvOffset;
                        float prevStepHeight = stepHeight;
                        float prevSurfaceHeight = surfaceHeight;

                        [unroll(10)]
                        for (int j = 1; j < 10 && stepHeight > surfaceHeight; j++){
                            prevUVOffset = uvOffset;
                            prevStepHeight = stepHeight;
                            prevSurfaceHeight = surfaceHeight;
                            uvOffset -= uvDelta;
                            stepHeight -= stepSize;
                            surfaceHeight = tex2D(_HeightTex, i.uv.xy+uvOffset);
                        }

                        float prevDifference = prevStepHeight - prevSurfaceHeight;
                        float difference = surfaceHeight - stepHeight;
                        float t = prevDifference / (prevDifference + difference);
                        uvOffset = prevUVOffset - uvDelta * t;
                    #endif
                    i.uv.xy += uvOffset;
                    #ifdef DETAIL
                        i.uvd += uvOffset * (_DetailNormal_ST.xy / _MainTex_ST.xy);
                    #endif
                #endif

                float4 albedo = tex2D(_MainTex, i.uv);
                albedo *= _Color;
                albedo.rgb = lerp(dot(albedo.rgb, float3(0.3,0.59,0.11)), albedo.rgb, _Saturation);
                float4 diffuse = albedo;

                float3 lightCol = _LightColor0 * LIGHT_ATTENUATION(i);
                #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT) 
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
                #else
                    float3 lightDir = _WorldSpaceLightPos0.xyz;
                #endif

                #if defined(SHADED) || defined(PBR)
                    #if UNITY_SINGLE_PASS_STEREO
                        float3 cameraPos = (unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1])*0.5;
                    #else
                        float3 cameraPos = _WorldSpaceCameraPos;
                    #endif
                    float3 viewDir = normalize(cameraPos - i.worldPos);
                    float3 halfVector = normalize(lightDir + viewDir);
                    float3 normalTex = UnpackScaleNormal(tex2D(_NormalTex, i.uv.xy), _NormalStrength);
                    #ifdef DETAIL
                        float3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormal, i.uvd), _DetailStrength);
                        float3 tangentNormals = BlendNormals(normalTex, detailNormal);
                    #else
                        float3 tangentNormals = normalize(normalTex);
                    #endif
                    float3 binormal = i.binormal;
                    float3 normalDir = normalize(tangentNormals.x * i.tangent + tangentNormals.y * binormal + tangentNormals.z *i.normal);
                    float NdotL = DotClamped(normalDir, lightDir);

                    // Rim lighting
                    float rimDot = abs(dot(viewDir, normalDir));
                    float rim = pow((1-rimDot), (1-_RimWidth) * 10);
                    rim = smoothstep(_RimEdge, (1-_RimEdge), rim);
                    #ifdef PBR
                        albedo.rgb = lerp(albedo.rgb, _RimCol, (rim*_RimStrength));
                    #else
                        diffuse.rgb = lerp(diffuse.rgb, _RimCol, (rim*_RimStrength));
                    #endif
                #endif
                
                // Toon Shading
                #ifdef SHADED
                    [branch]if (!any(_LightColor0) && _UseStatic == 0)
                        _Shadow = 1;
                    else
                        _Shadow = 1-_Shadow;
                    _Direct = min(_Direct, _Gradient-0.0001);
                    lightCol = lightCol*(saturate(floor(NdotL * _Gradient) / (_Gradient-_Direct)) + _Shadow);
                    diffuse.rgb *= lightCol;
                #endif

                // PBR Shading
                #ifdef PBR
                    float metallic = tex2D(_MetallicTex, i.uv) * _Metallic;
                    float roughness = tex2D(_RoughnessTex, i.uv) * _Roughness;
                    float smoothness = 1-roughness;
                    roughness *= 1.7-0.7*roughness;
                    float ao = lerp(1, tex2D(_AOTex, i.uv).g, _AOStrength);
                    float3 reflectionDir = reflect(-viewDir, normalDir);
                    
                    // Direct Lighting
                    UnityLight light;
                    light.color = lightCol;
                    light.dir = lightDir;

                    // Indirect Lighting
                    UnityIndirect indirectLight;
                    indirectLight.diffuse = max(0, ShadeSH9(float4(normalDir,1))) * ao;
                    float3 reflections = float3(0,0,0);
                    [branch]if (_UseReflCube == 1){
                        reflections = texCUBElod(_ReflCube, float4(reflectionDir, roughness * UNITY_SPECCUBE_LOD_STEPS)) * ao;
                    }
                    else {
                        float4 envSample = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflectionDir, roughness * UNITY_SPECCUBE_LOD_STEPS);
                        reflections = DecodeHDR(envSample, unity_SpecCube0_HDR) * ao;
                    }
                    indirectLight.specular = reflections * LIGHT_ATTENUATION(i);

                    albedo.rgb = DiffuseAndSpecularFromMetallic(albedo, metallic, specularTint, omr);
                    diffuse = UNITY_BRDF_PBS(albedo, specularTint.rgb, omr, smoothness, normalDir, viewDir, light, indirectLight);
                #endif

                #if !defined(SHADED) && !defined(PBR)
                    diffuse.rgb *= lightCol;
                #endif
                UNITY_APPLY_FOG(i.fogCoord, diffuse);
				return diffuse;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
    CustomEditor "MUSEditor"
}