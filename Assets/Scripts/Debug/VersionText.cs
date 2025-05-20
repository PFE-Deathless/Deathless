using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI versionText;

	private void Awake()
	{
		versionText.text = Application.version;
	}
}
