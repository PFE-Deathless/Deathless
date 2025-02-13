using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeWall : MonoBehaviour
{
	public Camera _camera;
    public GameObject player;
	public Material wallMaterial;
    public Material transparentMaterial;
	public float sphereCastRadius = 5f;

	private List<GameObject> wallsHit = new List<GameObject>();

    void Start()
    {
        
    }


    void Update()
    {
		StartCoroutine(HitWalls());
	}
		

	IEnumerator HitWalls()
	{
		RaycastHit[] hits;
		//hits = Physics.CapsuleCastAll(_camera.transform.position, player.transform.position, capsuleCastRadius, transform.forward, float.MaxValue, LayerMask.GetMask("Wall"));
		hits = Physics.SphereCastAll(_camera.transform.position, sphereCastRadius, _camera.transform.TransformDirection(Vector3.forward), float.MaxValue, LayerMask.GetMask("Wall"));
		//hits = Physics.RaycastAll(_camera.transform.position, transform.forward, float.MaxValue, LayerMask.GetMask("Wall"));
		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			Renderer rend = hit.transform.GetComponent<Renderer>();
			if (rend)
			{
				rend.material = transparentMaterial;
			}
		}

		yield return new WaitForSeconds(.1f);
		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			Renderer rend = hit.transform.GetComponent<Renderer>();
			if (rend)
			{
				rend.material = wallMaterial;
			}
		}
		yield return StartCoroutine(HitWalls());

	}    
    
}
