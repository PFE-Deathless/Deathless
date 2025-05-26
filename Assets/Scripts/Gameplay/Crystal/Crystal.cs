using UnityEngine;

public class Crystal : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] HitType.Type[] weaknesses = new HitType.Type[1];
	[SerializeField] float deathDuration = 1f;
	[SerializeField] AnimationCurve deathCurve;
	
	[Header("Technical")]
	[SerializeField] float shakeDuration = 0.3f;
	[SerializeField] Transform mesh;

	private int _healthMax;
	private int _health;
	private HitBar _hitBar;
	private bool _dead = false;
	private float _shakeElapsedTime = 0f;
	private float _deathElapsedTime = 0f;
	private Vector3 _meshOriginalScale;
	private ParticleSystem _ps;
	private CrystalCounter _crystalCounter;

	public HitType.Type CurrentType { get; private set; }

	private void Start()
	{
		_hitBar = GetComponentInChildren<HitBar>();
		_ps = GetComponentInChildren<ParticleSystem>();
		_hitBar.SetTypes(weaknesses, 0);
		CurrentType = weaknesses[0];
		_healthMax = weaknesses.Length;
		_health = _healthMax;
		_meshOriginalScale = mesh.localScale;
	}

	private void Update()
	{
		HandleShake();

		HandleDeath();
	}

	void HandleShake()
	{
		if (_shakeElapsedTime >= 0)
		{
			float strength = (_shakeElapsedTime / shakeDuration) * 1.2f;
			float shakeX = (Mathf.PerlinNoise(Time.time * 30f, 0) - 0.5f) * 2f * strength;
			float shakeZ = (Mathf.PerlinNoise(0, Time.time * 30f) - 0.5f) * 2f * strength;
			_shakeElapsedTime -= Time.deltaTime;

			mesh.localPosition = new(shakeX, 0f, shakeZ);
		}
		else
		{
			mesh.localPosition = Vector3.zero;
		}
	}

	void HandleDeath()
	{
		if (_dead)
		{
			if (_deathElapsedTime < deathDuration)
			{
				float scale = deathCurve.Evaluate(_deathElapsedTime / deathDuration);
				mesh.localScale = _meshOriginalScale * scale;
			}
			else
			{
				_ps.Stop();
				if (_deathElapsedTime > (_ps.main.duration + deathDuration))
					Destroy(gameObject);
			}
			_deathElapsedTime += Time.deltaTime;
		}
	}

	public void SetCounter(CrystalCounter counter)
	{
		_crystalCounter = counter;
	}

	public void TakeDamage()
	{
		_health--;

		// Activate shake
		_shakeElapsedTime = shakeDuration;

		if(_health <= 0)
		{
			Death();
			return;
		}
		
		// Update weaknesses bar
		CurrentType = weaknesses[_healthMax - _health];
		_hitBar.SetTypes(weaknesses, _healthMax - _health);
		_hitBar.Shake();
	}

	void Death()
	{
		Destroy(GetComponentInChildren<Collider>());
		Destroy(_hitBar.gameObject);
		_crystalCounter.RemoveCrystal(this);
		_dead = true;
	}
}
