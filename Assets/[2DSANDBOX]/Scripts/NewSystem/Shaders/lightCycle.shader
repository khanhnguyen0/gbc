Shader "Example/Light Cycle"
{
    Properties
	{
      _MainTex ("Texture", 2D) = "white" {}
	  _tCol ("Top Colour", Color) = (1.0, 1.0, 1.0)
	  _bCol ("Bottom Colour", Color) = (1.0, 1.0, 1.0)
	  _Amount ("Ext", Range(-1,1)) = 0.5
    }

    SubShader
	{
      Tags { "RenderType" = "Opaque" }

      CGPROGRAM
      #pragma surface surf Lambert vertex:vert

      struct Input
	  {
          float2 uv_MainTex;
      };

	  float _Amount;
	  fixed4 col;

      sampler2D _MainTex;
	  fixed4 _tCol;
	  fixed4 _bCol;

	  void vert (inout appdata_full v) 
	  {
	      if(v.vertex.y > 0.0)
		  {
		    col = _tCol;
		  }
		  else
		  {
		    col = _tCol;
		  }

          v.vertex.xyz += v.normal * _Amount;
      }

      void surf (Input IN, inout SurfaceOutput o)
	  {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * col;
      }

      ENDCG

    }
	 
    Fallback "Diffuse"
  }