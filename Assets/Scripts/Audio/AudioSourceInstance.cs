using System.Collections;
using UnityEngine;

public class AudioSourceInstance : MonoBehaviour
{
	private AudioSource _audioSource;
	private bool _isActive;
	private AudioManager.MixerGroup _mixerGroup;

	public bool IsActive => _isActive;
	public AudioManager.MixerGroup MixerGroup => _mixerGroup;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void SetMixerGroup(AudioManager.MixerGroup group)
	{
		_mixerGroup = group;
		_audioSource.outputAudioMixerGroup = AudioManager.Instance.GetMixerGroup(group);
	}

	public void PlayAudio(AudioEntry entry)
	{
		PlayAudio(entry, Vector3.zero);
	}

	public void PlayAudio(AudioEntry entry, Vector3 position)
	{
		// Set parameters
		_audioSource.clip = entry.audioClip;
		_audioSource.pitch = entry.pitch + Random.Range(-entry.randomPitchDelta, entry.randomPitchDelta);
		_audioSource.volume = entry.volume;
		_audioSource.bypassEffects = entry.bypassEffects;
		_audioSource.bypassEffects = entry.bypassListenerEffects;
		_audioSource.loop = entry.loop;

		float delay = entry.audioClip.length;

		// Teleport Instance to the desired location
		transform.position = position;

		// Play the audio
		_audioSource.Play();

		// Start delay count to return to pool
		StartCoroutine(ReturnToPool(delay));
	}

	IEnumerator ReturnToPool(float delay)
	{
		yield return new WaitForSeconds(delay);
		yield return null; // Wait an ewtra frame to avoid clicks

		AudioManager.Instance.ReturnASIToPool(this);
	}
}
