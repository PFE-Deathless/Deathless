using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	[Header("Technical")]
	[SerializeField] AudioMixer audioMixer;
	[SerializeField] AudioSource globalAudioSource;

	public AudioEntry test;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			PlayOneShot(test);
			MixerGroup m = MixerGroup.Master;
			Debug.Log("test :\n" + m.ToString() + "Volume");
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			SetVolume(MixerGroup.Master, 0.8f);
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			SetVolume(MixerGroup.Master, 0.3f);
		}
	}

	void SetGlobalAudioSourceParams(AudioEntry entry)
	{
		globalAudioSource.outputAudioMixerGroup = entry.group;
		globalAudioSource.pitch = entry.pitch;
		globalAudioSource.volume = entry.volume;
		globalAudioSource.bypassEffects = entry.bypassEffects;
		globalAudioSource.bypassEffects = entry.bypassListenerEffects;
		globalAudioSource.loop = entry.loop;
	}

	public void SetVolume(MixerGroup group, float volume)
	{
		audioMixer.SetFloat(group.ToString() + "Volume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
	}

	public void PlayOneShot(AudioEntry entry)
	{
		SetGlobalAudioSourceParams(entry);
		globalAudioSource.PlayOneShot(entry.audioClip);
	}

	public enum MixerGroup
	{
		Master,
		Music,
		SFX,
		Interface,
		Ambience
	}
}
