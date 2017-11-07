using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ImageEffectAllowedInSceneView]
//[ExecuteInEditMode]
public class PostProcess : MonoBehaviour {

    public Shader shader;
    private Material material;

	// Use this for initialization
	void Start () {
        material = new Material(shader);
	}
	

    public void OnRenderImage(RenderTexture source,RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);

    }
}