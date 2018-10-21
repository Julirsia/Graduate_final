using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Showcases the different ramp textures included in the Toon Styles Shader Pack.
/// </summary>
public class RampsDemo : MonoBehaviour
{
	bool isSwitched = false;
	List<Renderer> renderers;
	
	void Start ()
	{
		renderers = new List<Renderer>();
		
		AddRenderers(transform);
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 210, 10, 200, 40), "Switch to " + (isSwitched ? "Lookup" : "ColorRamp")))
		{
			Switch();
		}
	}
	
	void Switch()
	{
		isSwitched = !isSwitched;
		
		if (isSwitched)
		{
			Texture ramp;
			
			for (int i = 0; i < renderers.Count; i++)
			{
				ramp = renderers[i].material.GetTexture("_Lookup");
				renderers[i].material.shader = Shader.Find("ToonStyles/ColorRamp/Diffuse");
				renderers[i].material.SetTexture("_Ramp", ramp);
			}
		}
		else
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].material.shader = Shader.Find("ToonStyles/Lookup/Diffuse");
			}
		}
		
		Debug.Log(isSwitched);
	}

	void AddRenderers(Transform parent)
	{
		if (parent.GetComponent<Renderer>() != null) renderers.Add(parent.GetComponent<Renderer>());
		
		for (int i = 0; i < parent.childCount; i++)
		{
			AddRenderers(parent.GetChild(i));
		}
	}
}
