//using UnityEngine;

//public class ProjectileCurve : MonoBehaviour
//{
//    public QuadraticCurve curve;
//    public float speed;

//    private float sampleTime;

//    void Start()
//    {
//        sampleTime = 0f;
//    }

//    void Update ()
//    {
//        sampleTime += Time.deltaTime * speed;
//        transform.position = curve.GetCurve().Evaluate(sampleTime);
//        transform.forward = curve.GetCurve().Evaluate(sampleTime + 0.001f) - transform.position;

//        if (sampleTime >= 1f)
//        {
//            Debug.Log("boom");
//            Destroy(gameObject);
//        }
//    }
//}
