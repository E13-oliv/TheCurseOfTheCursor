// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/Default_OverlayNoZ_difference"
 {
     Properties
     {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _Color ("Tint", Color) = (1,1,1,1)
          
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
  
         _ColorMask ("Color Mask", Float) = 15
     }
  
     SubShader
     {
         Tags
         { 
             "Queue"="Overlay" 
             "IgnoreProjector"="True" 
             "RenderType"="Transparent" 
             "PreviewType"="Plane"
             "CanUseSpriteAtlas"="True"
         }
          
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
  
         Cull Off
         Lighting Off
         ZWrite Off
         ZTest Off
         Blend SrcAlpha OneMinusSrcAlpha
         ColorMask [_ColorMask]


         Blend OneMinusDstColor OneMinusSrcAlpha //invert blending, so long as FG color is 1,1,1,1
         BlendOp Add
  
         Pass
         {
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
              
             struct appdata_t
             {
                 float4 vertex   : POSITION;
                 float4 color    : COLOR;
                 float2 texcoord : TEXCOORD0;
             };
  
             struct v2f
             {
                 float4 vertex   : SV_POSITION;
                 fixed4 color    : COLOR;
                 half2 texcoord  : TEXCOORD0;
             };

             struct vertexInput
             {
                 float4 vertex: POSITION;
                 float4 color : COLOR;
             };

             struct fragmentInput
             {
                 float4 pos : SV_POSITION;
                 float4 color : COLOR0;
             };

             fixed4 _Color;
             fixed4 _TextureSampleAdd; //Added for font color support
  
             v2f vert(appdata_t IN)
             {
                 v2f OUT;
                 OUT.vertex = UnityObjectToClipPos(IN.vertex);
                 OUT.texcoord = IN.texcoord;
 #ifdef UNITY_HALF_TEXEL_OFFSET
                 OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
 #endif
                 OUT.color = IN.color * _Color;
                 return OUT;
             }
  
             sampler2D _MainTex;
  
             fixed4 frag(v2f IN) : SV_Target
             {
             half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;  //Added for font color support
             clip (color.a - 0.01);
             return color;
             }

                 fragmentInput vert(vertexInput i)
             {
                 fragmentInput o;
                 o.pos = UnityObjectToClipPos(i.vertex);
                 o.color = _Color;
                 return o;
             }

             half4 frag(fragmentInput i) : COLOR
             {
                 return i.color;
             }

         ENDCG
         }
     }
 }