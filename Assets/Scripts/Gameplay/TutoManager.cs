using UnityEngine;
using System.Collections;

public class TutoManager : MonoBehaviour
{
	public static TutoManager Instance { get; private set; }

	// R�f�rences
	[Header("UIs")]
	[SerializeField] GameObject[] infoUI;

	[Header("Autres objets a d�sactiver")]
	[SerializeField] GameObject dummy;
	[SerializeField] GameObject porteAOuvrir;
	[SerializeField] GameObject[] ennemisFin;
	[SerializeField] GameObject porteDeFin;





	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	void Start()
	{
		// d�sactive toute les UI au lancement de la sc�ne
		for (int i = 0; i < infoUI.Length; i++)
		{
			infoUI[i].gameObject.SetActive(false);
		}
		// Active la premi�re UI
		StartCoroutine(Affichage());
	}

	void Update()
	{
		// Quand le dummy il meurt la premi�re fois �a ouvre la suite
		if (dummy.GetComponent<Dummy>().Died == true) DummyDies();

		// Quand le joueur a tu� tout les ennemis pour d�v�rouiller le porte finale
		if (ennemisFin[0] == null && ennemisFin[1] == null && ennemisFin[2] == null && ennemisFin[3] == null) EndTuto();

	}

	public void Activate(int _id, bool state)
	{
		infoUI[_id].SetActive(state);
		if (infoUI[_id].TryGetComponent(out IActivable activable))
			activable.Activate();
	}

	IEnumerator Affichage()
	{
		yield return new WaitForSeconds(0.75f);
		infoUI[0].SetActive(true);
	}

	void DummyDies()
	{
		if (porteAOuvrir == null)
			return;

		//porteAOuvrir.SetActive(false);
		infoUI[1].SetActive(false);
		if (porteAOuvrir.TryGetComponent(out IActivable activable))
			activable.Activate();

	}

	void EndTuto()
	{
		if (porteDeFin == null)
			return;

		infoUI[5].SetActive(false);
		infoUI[6].SetActive(true);

		if (porteDeFin.TryGetComponent(out IActivable activable))
			activable.Activate();

	}

}
