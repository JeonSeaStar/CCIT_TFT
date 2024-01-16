Shader "Custom/Aura"
{
    Properties
    {
      [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
      _EffectColor("Effect Color", Color) = (1,1,1,1)
      _EffectWidth("Effect Width", float) = 1
    }

        SubShader
      {
        Tags
        {
          "Queue" = "Transparent"
          "IgnoreProjector" = "True"
          "RenderType" = "Transparent"
          "PreviewType" = "Plane"
          "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #include "UnityCG.cginc"

          struct appdata_t
          {
            float4 vertex   : POSITION;
            float2 texcoord : TEXCOORD0;
          };

          struct v2f
          {
            float4 vertex   : SV_POSITION;
            float2 texcoord  : TEXCOORD0;
          };

          v2f vert(appdata_t IN)
          {
            v2f OUT;
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.texcoord = IN.texcoord;

            return OUT;
          }

          sampler2D _MainTex;
          float _EffectWidth;
          fixed4 _EffectColor;
          float4 _MainTex_TexelSize;
          fixed4 frag(v2f IN) : SV_Target
          {
            float2 fragTexCoord = IN.texcoord;
            fixed4 col = tex2D(_MainTex, fragTexCoord);

            float2 ps = _MainTex_TexelSize.xy;
            float a;
            float maxa = col.a;
            float mina = col.a;

            a = tex2D(_MainTex, fragTexCoord + float2(0.0, -_EffectWidth) * ps).a;
            maxa = max(a, maxa);
            mina = min(a, mina);

            a = tex2D(_MainTex, fragTexCoord + float2(0.0, _EffectWidth) * ps).a;
            maxa = max(a, maxa);
            mina = min(a, mina);

            a = tex2D(_MainTex, fragTexCoord + float2(-_EffectWidth, 0.0) * ps).a;
            maxa = max(a, maxa);
            mina = min(a, mina);

            a = tex2D(_MainTex, fragTexCoord + float2(_EffectWidth, 0.0) * ps).a;
            maxa = max(a, maxa);
            mina = min(a, mina);

            col.rgb *= col.a;
            col.rgb += _EffectColor.rgb * (maxa - mina);

            return col;
          }
          ENDCG
        }
      }
}
