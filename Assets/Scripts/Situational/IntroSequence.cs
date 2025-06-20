using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroSequence : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] float timeToPass = 2f;
	
	[Header("Technical")]
	[SerializeField] string sceneToLoad;
	[SerializeField] Image uiCircle;

	VideoPlayer _player;
	float _elapsedTime = 0f;
	bool _isFinished = false;

	InputsManager _inputManager;

	void Start()
	{
		_player = GetComponent<VideoPlayer>();
		_player.loopPointReached += OnCutSceneFinished;

		_inputManager = GetComponent<InputsManager>();
		_inputManager.SetMap(Map.Menu);
	}

	private void Update()
	{
		if (_inputManager.skip)
		{
			_elapsedTime += Time.deltaTime;
			if (_elapsedTime > timeToPass)
			{
				OnCutSceneFinished(_player);
			}
		}
		else
		{
			_elapsedTime = 0f; ;
		}

		uiCircle.fillAmount = _elapsedTime / timeToPass;
	}

	void OnCutSceneFinished(VideoPlayer vp)
	{
		if (_isFinished)
			return;

		_isFinished = true;
		_player.loopPointReached -= OnCutSceneFinished;
		GameManager.Instance.LoadLevel(sceneToLoad);
	}
}
