using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{
    public Transform StartPos;
    public Transform Target;
    public Transform Control;
    public Transform player;

    private float ControlPosx;
    private float ControlPosz;

    void Start()
    {
        ControlPosx = Control.position.x;
        ControlPosz = Control.position.z;
    }
    void Update()
    {
        Target.position = player.position;
        ControlPosx = (Target.position.x + StartPos.position.x)/2;
        ControlPosz = (Target.position.z + StartPos.position.z)/2;
        Control.position = new Vector3(ControlPosx, Control.position.y, ControlPosz);
    }

    public Vector3 evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(StartPos.position, Control.position, t);
        Vector3 cb = Vector3.Lerp(Control.position, Target.position, t);
        return Vector3.Lerp(ac, cb, t);
    }

    private void OnDrawGizmos()
    {
        if (StartPos == null || Target == null || Control == null)
        {
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(evaluate(i / 20f), 0.1f);
        }
    }
}
