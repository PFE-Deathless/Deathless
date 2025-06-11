using UnityEngine;

public class EnemyProtectedDamageParticle : MonoBehaviour
{
    [SerializeField] float animationSpeed = 30f;
    [SerializeField] AnimationCurve animationCurve;

    float _elapsedTime = 0f;

    bool _started = false;
    float _animationDuration;
    Transform _start;
    Transform _end;

    private void Update()
    {
        if (_started)
        {
            if (_started)
            {
                if (_elapsedTime < _animationDuration)
                {
                    transform.position = Vector3.Lerp(_start.position, _end.position, animationCurve.Evaluate(_elapsedTime / _animationDuration));
                    _elapsedTime += Time.deltaTime;
                }
                else
                {
                    GetComponentInChildren<ParticleSystem>().Stop();
                    Destroy(gameObject, 1f);
                }
            }
        }
    }

    public void Setup(Transform start, Transform end)
    {
        _start = start;
        _end = end;
        _animationDuration = Vector3.Distance(_start.position, _end.position) / animationSpeed;

        _started = true;
    }
}
