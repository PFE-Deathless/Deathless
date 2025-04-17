using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioEntry", menuName = "Scriptable Objects/AudioEntry")]
public class AudioEntry : ScriptableObject
{
	[Header("Resources")]
	public AudioClip audioClip;
	public AudioMixerGroup group;

	[Header("Properties")]
	[Range(0f, 1f)] public float volume = 1f;
	[Range(-3f, 3f)] public float pitch = 1f;
	public bool bypassEffects = false;
	public bool bypassListenerEffects = false;
	public bool loop = false;
}
