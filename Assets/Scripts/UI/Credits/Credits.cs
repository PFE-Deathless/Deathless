using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Credits : MonoBehaviour
{
	[Header("Scroll Properties")]
	[SerializeField] float scrollSpeed = 100f;
	[SerializeField] float scrollSpeedMultiplier = 5f;
	[SerializeField] float scrollOffset = 1000f;
	[SerializeField] float timeToPass = 2f;

	[Header("Text Properties")]
	[SerializeField] float spacingBase = 10f;
	[SerializeField] float spacingTitle = 35f;
	[SerializeField] float spacingSubtitle = 20f;
	[SerializeField] float spacingImage = 20f;

	[Header("Technical")]
	[SerializeField] string sceneToLoad;
	[SerializeField] Image uiCircle;
	[SerializeField] string creditTSVFileName = "Credits";
	[SerializeField] GameObject baseTextPrefab;
	[SerializeField] GameObject titleTextPrefab;
	[SerializeField] GameObject subtitleTextPrefab;
	[SerializeField] GameObject imagePrefab;

	Transform _creditParent;
	CreditText[] _creditsTexts;
	float _totalOffset;

	float _elapsedTime = 0f;

	InputsManager _inputManager;

	private void Start()
	{
		//Destroy(GameManager.Instance.gameObject);
		_inputManager = GetComponent<InputsManager>();
		_inputManager.SetMap(Map.Menu);

		CreateCredits();
	}

	private void Update()
	{
		float mult = 1f + (scrollSpeedMultiplier * _inputManager.creditScroll);

		Vector3 pos = _creditParent.position;
		pos.y += scrollSpeed * Time.deltaTime * mult;
		_creditParent.position = pos;

		// Load menu after credits
		if (pos.y > _totalOffset)
		{
			if (GameManager.Instance != null)
				GameManager.Instance.LoadLevel(sceneToLoad);
		}

		// Skip
		if (_inputManager.skip)
		{
			_elapsedTime += Time.deltaTime;
			if (_elapsedTime > timeToPass)
			{
				if (GameManager.Instance != null)
					GameManager.Instance.LoadLevel(sceneToLoad);
			}
		}
		else
		{
			_elapsedTime = 0f; ;
		}

		uiCircle.fillAmount = _elapsedTime / timeToPass;
	}

	void CreateCredits()
	{
		// Load the text
		string path = Path.Combine("Credits", creditTSVFileName);
		string imagePath = Path.Combine("Credits", "Images");

		TextAsset temp = Resources.Load<TextAsset>(path);
		string[] tempArr = temp.text.Split("\r\n");

		_creditsTexts = new CreditText[tempArr.Length - 1];

		for (int i = 0; i < tempArr.Length - 1; i++)
			_creditsTexts[i] = new CreditText(tempArr[i + 1]);

		// Create parent object
		GameObject obj = new("CreditParent");
		obj.transform.parent = transform;
		_creditParent = obj.transform;
		_creditParent.position = new(Screen.width / 2f, (Screen.height / 2f) - scrollOffset);

		float currentOffset = 0f;
		bool isImage;
		TextMeshProUGUI tmp;

		// Instantiate the objects and fill the text/image
		for (int i = 0; i < _creditsTexts.Length; i++)
		{
			isImage = false;

			switch (_creditsTexts[i].type)
			{
				case CreditType.Base:
					obj = Instantiate(baseTextPrefab, _creditParent);
					currentOffset -= spacingBase;
					break;
				case CreditType.Title:
					obj = Instantiate(titleTextPrefab, _creditParent);
					currentOffset -= spacingTitle;
					break;
				case CreditType.Subtitle:
					obj = Instantiate(subtitleTextPrefab, _creditParent);
					currentOffset -= spacingSubtitle;
					break;
				case CreditType.Image:
					obj = Instantiate(imagePrefab, _creditParent);
					currentOffset -= spacingImage;
					isImage = true;
					break;
				case CreditType.Spacing:
					currentOffset -= float.Parse(_creditsTexts[i].content);
					break;
			}

			if (_creditsTexts[i].type != CreditType.Spacing)
			{
				if (isImage)
				{
					Image image = obj.GetComponent<Image>();
					image.sprite = Resources.Load<Sprite>(Path.Combine(imagePath, _creditsTexts[i].content));
					currentOffset -= image.rectTransform.rect.height;
				}
				else
				{
					tmp = obj.GetComponent<TextMeshProUGUI>();
					tmp.text = _creditsTexts[i].content;
					currentOffset -= tmp.rectTransform.rect.height / 2f;
				}

				Vector3 pos = obj.transform.position;
				pos.y = currentOffset;
				obj.transform.position = pos;
			}



			//Debug.Log($"Content (Type : {_creditsTexts[i].type}) [{i}] : {_creditsTexts[i].content}");
		}

		_totalOffset = -(currentOffset);
		Debug.Log("Offset : " + _totalOffset);
	}

	public class CreditText
	{
		public CreditType type;
		public string content;

		public CreditText(string data)
		{
			if (data == null)
			{
				type = CreditType.Base;
				content = "MISSING_TEXT";
			}
			else
			{
				string[] temp = data.Split("\t");

				type = temp[0] switch
				{
					"Base" => CreditType.Base,
					"Title" => CreditType.Title,
					"Subtitle" => CreditType.Subtitle,
					"Image" => CreditType.Image,
					"Spacing" => CreditType.Spacing,
					_ => CreditType.Base,
				};
				content = temp[1];
			}
		}
	}

	public enum CreditType
	{
		Base,
		Title,
		Subtitle,
		Image,
		Spacing,
	}
}
