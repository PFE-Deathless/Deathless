using UnityEngine;

public class WallTransparency : MonoBehaviour
{
	private Material wallMaterial;
	public Renderer wallRenderer;

	void Start()
	{
		wallMaterial = wallRenderer.GetComponent<MeshRenderer>().material;
	}

	void Update()
	{
		if (PlayerController.Instance != null)
			wallMaterial.SetVector("_PlayerPos", PlayerController.Instance.transform.position + Vector3.up * 3f);
	}

}
