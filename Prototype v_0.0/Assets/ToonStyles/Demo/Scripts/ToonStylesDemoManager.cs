using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Showcases the shaders of the Toon Styles Shader Pack.
/// </summary>
public class ToonStylesDemoManager : MonoBehaviour
{
	public Texture[] ramps;
	public Texture[] lookups;
		
	private DemoSet[] sets;
	private List<Material> materials;
	
	void Start()
	{
		sets = new DemoSet[]
		{
			new DemoSet(new Color(1,1,1,0.5f), new Color(0,0,0,0.75f), Color.black, Color.black, ramps[0], lookups[0], lookups[12], 0.5f, 0.5f, 0.2f, 2, 4, -0.5f, 3, 58),
			new DemoSet(new Color(0.8f,0.85f,0.95f,0.3f), new Color(0.3f,0.1f,0.05f,0.9f), new Color(0.7f, 0.2f, 0.1f, 0.7f), Color.black, ramps[1], lookups[1], lookups[13], 0.25f, 0.25f, 0.25f, 2, 3, 0.25f, 0.5f, 30),
			new DemoSet(new Color(0.8f,0.65f,0.65f,0.9f), new Color(0f,0.15f,0.05f,0.15f), new Color(0.4f, 0.3f, 0f, 0.7f), Color.black, ramps[2], lookups[2], lookups[1], 0.65f, 0.65f, 0.15f, 0.75f, 4, -0.75f, 5f, 40),
			new DemoSet(new Color(0.7f,0.9f,0.65f,0.6f), new Color(0.05f,0.05f,0.25f,0.75f), new Color(0.4f, 0.3f, 0.9f, 0.9f), Color.black, ramps[3], lookups[3], lookups[4], 0.55f, 0.45f, 0.4f, 4f, 6, -0.9f, 12f, 60),
			new DemoSet(new Color(0.6f,0.45f,0.15f,0.4f), new Color(0.4f,0.2f,0f,0.85f), new Color(0.9f, 0.9f, 0.9f, 1f), Color.black, ramps[4], lookups[4], lookups[10], 0.45f, 0.55f, 0.25f, 5f, 8, 0.1f, 7f, 50),
		};
		
		materials = new List<Material>();
		AddMaterials(transform, materials);
	}
	
	void AddMaterials(Transform item, List<Material> matList)
	{
		if (item.GetComponent<Renderer>() != null && item.GetComponent<Renderer>().material != null && !matList.Contains(item.GetComponent<Renderer>().material))
		{
			matList.Add(item.GetComponent<Renderer>().material);
		}
		
		for (int i = 0; i < item.childCount; i++)
		{
			AddMaterials(item.GetChild(i), matList);
		}
	}
	
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Alpha1)) SetDemoSet(0);
		if (Input.GetKeyUp(KeyCode.Alpha2)) SetDemoSet(1);
		if (Input.GetKeyUp(KeyCode.Alpha3)) SetDemoSet(2);
		if (Input.GetKeyUp(KeyCode.Alpha4)) SetDemoSet(3);
		if (Input.GetKeyUp(KeyCode.Alpha5)) SetDemoSet(4);
		
		if (Input.GetKeyUp(KeyCode.Space)) SetDemoSet(-1);
		
	}
	
	void OnGUI()
	{
		if (!Application.isEditor && (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android))
		{
			if (GUI.Button(new Rect(Screen.width - 240, 0, 40, 40), "1"))
			{
				SetDemoSet(0);
			}
			if (GUI.Button(new Rect(Screen.width - 200, 0, 40, 40), "2"))
			{
				SetDemoSet(1);
			}
			if (GUI.Button(new Rect(Screen.width - 160, 0, 40, 40), "3"))
			{
				SetDemoSet(2);
			}
			if (GUI.Button(new Rect(Screen.width - 120, 0, 40, 40), "4"))
			{
				SetDemoSet(3);
			}
			if (GUI.Button(new Rect(Screen.width - 80, 0, 40, 40), "5"))
			{
				SetDemoSet(4);
			}
			if (GUI.Button(new Rect(Screen.width - 40, 0, 40, 40), "?"))
			{
				SetDemoSet(-1);
			}
		}
	}
	
	void SetDemoSet(int setIndex)
	{
		if (setIndex >= sets.Length) return;
		
		DemoSet newSet;
		
		if (setIndex < 0)
		{
			newSet = new DemoSet(ramps, lookups);
		}
		else
		{
			newSet = sets[setIndex];
		}
		
		for (int i = 0; i < materials.Count; i++)
		{
			if (materials[i].HasProperty("_Color")) materials[i].SetColor("_Color", newSet._Color);
			if (materials[i].HasProperty("_Color1")) materials[i].SetColor("_Color1", newSet._Color1);
			
			if (materials[i].HasProperty("_Ramp")) materials[i].SetTexture("_Ramp", newSet._Ramp);
			if (materials[i].HasProperty("_Lookup")) materials[i].SetTexture("_Lookup", newSet._Lookup);
			if (materials[i].HasProperty("_ColorControl")) materials[i].SetTexture("_ColorControl", newSet._ColorControl);
			
			if (materials[i].HasProperty("_Threshold")) materials[i].SetFloat("_Threshold", newSet._Threshold);
			if (materials[i].HasProperty("_LightBalance")) materials[i].SetFloat("_LightBalance", newSet._LightBalance);
			if (materials[i].HasProperty("_Offset")) materials[i].SetFloat("_Offset", newSet._Offset);
			if (materials[i].HasProperty("_Scale")) materials[i].SetFloat("_Scale", newSet._Scale);
			if (materials[i].HasProperty("_Levels")) materials[i].SetFloat("_Levels", newSet._Levels);
			if (materials[i].HasProperty("_Brightness")) materials[i].SetFloat("_Brightness", newSet._Brightness);
			if (materials[i].HasProperty("_Contrast")) materials[i].SetFloat("_Contrast", newSet._Contrast);
			if (materials[i].HasProperty("_Shine")) materials[i].SetFloat("_Shine", newSet._Shine);
			
			RenderSettings.ambientLight = newSet.ambientLight;
		}
	}
	
	private struct DemoSet
	{
		public Color _Color;
		public Color _Color1;
		public Color _OutlineColor;
		public Color ambientLight;
		public Texture _ColorControl;
		public Texture _Ramp;
		public Texture _Lookup;
		public float _Threshold;
		public float _LightBalance;
		public float _Offset;
		public float _Scale;
		public float _Levels;
		public float _Brightness;
		public float _Contrast;
		public float _Shine;
		
		public DemoSet(Color _Color, Color _Color1, Color _OutlineColor, Color ambientLight, Texture _Ramp, Texture _Lookup,
			Texture _ColorControl, float _Threshold, float _LightBalance, float _Offset, float _Scale,
			float _Levels, float _Brightness, float _Contrast, float _Shine)
		{
			this._Color = _Color;
			this._Color1 = _Color1;
			this._OutlineColor = _OutlineColor;
			this._Ramp = _Ramp;
			this._Lookup = _Lookup;
			this._Threshold = _Threshold;
			this._LightBalance = _LightBalance;
			this._Offset = _Offset;
			this._Scale = _Scale;
			this._Levels = _Levels;
			this._Brightness = _Brightness;
			this._Contrast = _Contrast;
			this._Shine = _Shine;
			this.ambientLight = ambientLight;
			this._ColorControl = _ColorControl;
		}
		
		public DemoSet(Texture[] rampPool, Texture[] lookupPool)
		{
			_Color = new Color(Random.value, Random.value, Random.value, Random.value);
			_Color1 = new Color(Random.value, Random.value, Random.value, Random.value);
			_OutlineColor = new Color(Random.value, Random.value, Random.value, Random.value);
			ambientLight = new Color(Random.value, Random.value, Random.value, Random.value);
			
			_Ramp = rampPool[Random.Range(0, rampPool.Length)];
			_Lookup = lookupPool[Random.Range(0, lookupPool.Length)];
			_ColorControl = lookupPool[Random.Range(0, lookupPool.Length)];
			
			_Threshold = Random.value * 0.8f + 0.1f;
			_LightBalance = Random.value * 0.8f + 0.1f;
			_Offset = Random.value * 1.6f - 0.8f;
			_Scale = Random.value * 3 + 0.5f;
			_Levels = Random.value * 6 + 2;
			_Brightness = Random.value - 0.5f;
			_Contrast = Random.value * 3 + 0.2f;
			_Shine = Random.value * 41 + 20;
		}
	}
}
