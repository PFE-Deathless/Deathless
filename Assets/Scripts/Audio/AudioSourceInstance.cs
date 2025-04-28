using UnityEngine;

public class AudioSourceInstance : MonoBehaviour
{
	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}


}
