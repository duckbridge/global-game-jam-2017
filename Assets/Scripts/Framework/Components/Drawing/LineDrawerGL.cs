using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawerGL : LineDrawer {

	// Use this for initialization
	void Start () {
		base.Start();
		
		if(GetComponent<Camera>() == null) {
			Debug.Log("[WARM] this script should be attached to the camera!");
		}

		mat = new Material(
			"Shader \"Lines/Colored Blended\" {" +
			"SubShader { Pass { " +
			"    Blend SrcAlpha OneMinusSrcAlpha " +
			"    ZWrite Off Cull Off Fog { Mode Off } " +
			"    BindChannels {" +
			"      Bind \"vertex\", vertex Bind \"color\", color }" +
			"} } }" 

		);

		mat.hideFlags = HideFlags.HideAndDontSave;
		mat.shader.hideFlags = HideFlags.HideAndDontSave;
	}
	
	void OnPostRender() {
		GL.PushMatrix();

		//mat.SetTexture("_MainTex", texture);
		//mat.SetTextureScale("_MainTex", new Vector3(10,10,10));
		//mat.SetTexture("_BumpMap", texture);
		//mat.SetTexture("_Cube", texture);

		mat.SetPass(0);
		
		GL.LoadOrtho();
		GL.Begin(GL.LINES);
		GL.Color(Color.red);

		for(int i = 0 ; i < lines.Count ; i++) {
			
			Vector3 cameraRelativeStart = GetComponent<Camera>().WorldToScreenPoint(lines[i].start);
			Vector3 cameraRelativeEnd = GetComponent<Camera>().WorldToScreenPoint(lines[i].end);

			if(isLogging) {
				Debug.Log("camera relative start : " + cameraRelativeStart.x/Screen.width + " " + cameraRelativeStart.y/Screen.height + " " + cameraRelativeStart.z);
				Debug.Log("camera relative end : " + cameraRelativeEnd.x/Screen.width + " " + cameraRelativeEnd.y/Screen.height + " " + cameraRelativeStart.z);
			}
  			
			GL.Vertex3(cameraRelativeStart.x/Screen.width, cameraRelativeStart.y/Screen.height, 0);
			GL.Vertex3(cameraRelativeEnd.x/Screen.width, cameraRelativeEnd.y/Screen.height, 0);

		}
		
		GL.End();
		GL.PopMatrix();
	}
}
