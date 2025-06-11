using UnityEngine;

public class SoulParticleFeedback : MonoBehaviour
{
    [SerializeField] float animationSpeed = 3f;
    [SerializeField] Transform particleTransform;
    [SerializeField] AnimationCurve animationCurve;

    Vector3 _start;
    Vector3 _end;

    float _elapsedTime = 0f;
    float _animationDuration;
    bool _started = false;


    void Update()
    {
        if (_started)
        {
            if (_elapsedTime < _animationDuration)
            {
                particleTransform.position = Vector3.Lerp(_start, _end, animationCurve.Evaluate(_elapsedTime / _animationDuration));
                _elapsedTime += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject, 1f);
            }
        }
    }

    public void Setup(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
        _animationDuration = Vector3.Distance(_start, _end) / animationSpeed;

        _started = true;
    }
}
