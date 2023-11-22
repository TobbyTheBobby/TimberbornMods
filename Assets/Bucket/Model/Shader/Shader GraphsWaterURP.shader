Shader "Shader Graphs/WaterURP" {
	Properties {
		_Color ("Albedo Color", Vector) = (1,1,1,1)
		[NoScaleOffset] _MainTex ("Albedo", 2D) = "black" {}
		_EmissionColor ("Emission Color", Vector) = (0,0,0,0)
		_MainTexTiling ("Albedo Tiling", Float) = 1
		_Grayscale ("Grayscale", Range(0, 1)) = 0
		[NoScaleOffset] _DetailAlbedoTex ("Detail Albedo", 2D) = "grey" {}
		_DetailAlbedoTexTiling ("Detail Albedo Tiling", Float) = 1
		[NoScaleOffset] [Normal] _BumpMap1 ("Normal Map 1", 2D) = "bump" {}
		_BumpMap1Tiling ("Normal Map 1 Tiling", Float) = 1
		_BumpMap1Strength ("Normal Map 1 Strength", Float) = 1
		_BumpMap1Speed ("Normal Map 1 Speed", Vector) = (1,0,0,0)
		[NoScaleOffset] _BumpMap2 ("Normal Map 2", 2D) = "white" {}
		_BumpMap2Tiling ("Normal Map 2 Tiling", Float) = 1
		_BumpMap2Strength ("Normal Map 2 Strength", Float) = 1
		_BumpMap2Speed ("Normal Map 2 Speed", Vector) = (0,1,0,0)
		_MetallicMapScale ("Metallic Scale", Float) = 1
		_GlossMapScale ("Smoothness Scale", Float) = 1
		[NoScaleOffset] _MetallicGlossMap ("Metallic (Red) and Smoothness (Alpha)", 2D) = "white" {}
		_MetallicGlossMapTiling ("Metallic (Red) and Smoothness (Alpha) Tiling", Float) = 1
		_FoamColor ("Foam Color", Vector) = (1,1,1,1)
		_FoamDepth ("Foam Depth", Float) = 0.3
		_FoamOffset ("Foam Offset", Float) = 0.1
		_FoamIntensity ("Foam Intensity", Float) = 1
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}