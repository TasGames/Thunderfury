// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Rantis/NeonCore"
{
	Properties
	{
		_MainTex("BaseTexture", 2D) = "white" {}
		_CoreColor("CoreColor", Color) = (0,0.2980392,1,0)
		_OuterColor("OuterColor", Color) = (1,0,0,0)
		_Emission("Emission", Color) = (1,1,1,0)
		_CoreSize("Core Size", Range( 0 , 5)) = 0.6
		_DissolveGuide("Dissolve Guide", 2D) = "white" {}
		_BurnRamp("Burn Ramp", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range( 0 , 1)) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 255
			WriteMask 80
			Comp Always
			Pass Replace
			Fail Keep
			ZFail Keep
		}
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			float3 worldNormal;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _Emission;
		uniform float4 _OuterColor;
		uniform float4 _CoreColor;
		uniform float _CoreSize;
		uniform float _DissolveAmount;
		uniform sampler2D _DissolveGuide;
		uniform float4 _DissolveGuide_ST;
		uniform sampler2D _BurnRamp;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode186 = tex2D( _MainTex, uv_MainTex );
			float4 lerpResult229 = lerp( tex2DNode186 , float4( 0,0,0,0 ) , _Emission);
			o.Albedo = lerpResult229.rgb;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult3 = dot( i.viewDir , ( ase_normWorldNormal * _CoreSize ) );
			float4 lerpResult4 = lerp( _OuterColor , _CoreColor , dotResult3);
			float clampResult197 = clamp( (-4 + (( (-0.6 + (( 1.0 - 0.05 ) - 0) * (0.6 - -0.6) / (1 - 0)) + tex2DNode186.r ) - 0) * (4 - -4) / (1 - 0)) , 0 , 1 );
			float temp_output_198_0 = ( 1.0 - clampResult197 );
			float4 temp_cast_1 = (temp_output_198_0).xxxx;
			float4 lerpResult203 = lerp( lerpResult4 , temp_cast_1 , temp_output_198_0);
			float2 uv_DissolveGuide = i.uv_texcoord * _DissolveGuide_ST.xy + _DissolveGuide_ST.zw;
			float temp_output_178_0 = ( (-0.6 + (( 1.0 - _DissolveAmount ) - 0) * (0.6 - -0.6) / (1 - 0)) + tex2D( _DissolveGuide, uv_DissolveGuide ).r );
			float clampResult180 = clamp( (-4 + (temp_output_178_0 - 0) * (4 - -4) / (1 - 0)) , 0 , 1 );
			float temp_output_181_0 = ( 1.0 - clampResult180 );
			float2 appendResult182 = (float2(temp_output_181_0 , 0));
			float4 temp_output_184_0 = ( temp_output_181_0 * tex2D( _BurnRamp, appendResult182 ) );
			float4 lerpResult185 = lerp( lerpResult203 , temp_output_184_0 , temp_output_184_0.r);
			o.Emission = ( _Emission * lerpResult185 ).rgb;
			o.Alpha = 1;
			clip( temp_output_178_0 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15001
36;314;1288;616;-801.274;1757.479;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;172;-806.6996,-211.7798;Float;False;908.2314;498.3652;Dissolve - Opacity Mask;5;174;176;177;175;178;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-766.7411,-76.85318;Float;False;Property;_DissolveAmount;Dissolve Amount;7;0;Create;True;0;0;False;0;0;0.153;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;175;-612.8168,-173.3652;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;177;-554.239,11.92946;Float;True;Property;_DissolveGuide;Dissolve Guide;5;0;Create;True;0;0;False;0;None;4932715fe7b99d74ca96a53a2af1a66e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;176;-374.3687,-174.2031;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;201;1220.292,500.5305;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;173;142,-34.95716;Float;False;814.5701;432.0292;Burn Effect - Emission;1;179;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;191;1488.687,316.9724;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-44.28893,-180.0135;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;192;1704.874,230.1733;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;179;143.5647,148.1273;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;186;1244.174,710.143;Float;True;Property;_MainTex;BaseTexture;0;0;Create;False;0;0;False;0;None;1b3b45149ffdc8d4caaa3fef441a4512;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;195;2037.764,383.5991;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;180;324.9161,17.55008;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;604.9982,-508.8259;Float;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;194;2282.821,260.5087;Float;False;814.5701;432.0292;Burn Effect - Emission;3;198;197;196;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;204;585.2015,-264.5588;Float;False;Property;_CoreSize;Core Size;4;0;Create;True;0;0;False;0;0.6;0.65;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;170;896.0436,-644.2444;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.6;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;2;606.9777,-785.9495;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;196;2292.859,499.6321;Float;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;181;474.678,18.0128;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;701.9702,-1821.859;Float;False;Property;_OuterColor;OuterColor;2;0;Create;True;0;0;False;0;1,0,0,0;0,1,0.8758622,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;197;2507.36,225.6348;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;829.2538,-1181.692;Float;False;Property;_CoreColor;CoreColor;1;0;Create;True;0;0;False;0;0,0.2980392,1,0;1,0,0.9310346,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;3;1145.562,-741.7384;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;182;590.1019,187.4579;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;183;758.8325,-6.800598;Float;True;Property;_BurnRamp;Burn Ramp;6;0;Create;True;0;0;False;0;None;d675983047cc59d40958671a38fdf5c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;198;2742.346,230.5956;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;2027.547,-913.4008;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;203;2836.343,-358.3398;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;2405.227,-172.3308;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;185;3317.902,-566.3426;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;223;3299.973,-294.5778;Float;False;Property;_Emission;Emission;3;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;4021.941,-690.0499;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;229;4001.123,-94.72096;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4341.479,-116.8751;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Rantis/NeonCore;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;True;255;False;-1;255;False;-1;80;False;-1;7;False;-1;3;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;SrcAlpha;OneMinusSrcAlpha;8;SrcAlpha;One;OFF;OFF;0;False;0.001;1,1,1,0;VertexOffset;False;False;Cylindrical;False;Relative;0;;8;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;175;0;174;0
WireConnection;176;0;175;0
WireConnection;191;0;201;0
WireConnection;178;0;176;0
WireConnection;178;1;177;1
WireConnection;192;0;191;0
WireConnection;179;0;178;0
WireConnection;195;0;192;0
WireConnection;195;1;186;1
WireConnection;180;0;179;0
WireConnection;170;0;1;0
WireConnection;170;1;204;0
WireConnection;196;0;195;0
WireConnection;181;0;180;0
WireConnection;197;0;196;0
WireConnection;3;0;2;0
WireConnection;3;1;170;0
WireConnection;182;0;181;0
WireConnection;183;1;182;0
WireConnection;198;0;197;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;4;2;3;0
WireConnection;203;0;4;0
WireConnection;203;1;198;0
WireConnection;203;2;198;0
WireConnection;184;0;181;0
WireConnection;184;1;183;0
WireConnection;185;0;203;0
WireConnection;185;1;184;0
WireConnection;185;2;184;0
WireConnection;226;0;223;0
WireConnection;226;1;185;0
WireConnection;229;0;186;0
WireConnection;229;2;223;0
WireConnection;0;0;229;0
WireConnection;0;2;226;0
WireConnection;0;10;178;0
ASEEND*/
//CHKSM=370A959ADF9840D95249E9E983473C91B615B3CC