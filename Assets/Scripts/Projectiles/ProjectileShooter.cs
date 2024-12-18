using UnityEditor;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
	public Type type = Type.Forward;
	public ProjectileObject projectile;
	public Transform origin;

	[Header("Curve")]
	[Tooltip("Only for curve type")] public float time;

	public enum Type
	{
		Forward,
		Homing,
		Curve,
	}

	public void ShootProjectile()
	{
		GameObject obj = Instantiate(projectile.gameObject, origin.position, origin.rotation);
		obj.AddComponent<Rigidbody>();
		ProjectileMovement p = obj.AddComponent<ProjectileMovement>();
		p.Setup(projectile);

		switch (type)
		{
			case Type.Forward:
				p.Forward();
				break;
			case Type.Homing:
				p.Homing();
				break;
			case Type.Curve:
				p.Curve(time, GetComponent<QuadraticCurve>().GetCurve());
				break;
		}
	}

	// BAP BIP BOUP CA MARCHE PAS AYAYA

	//[CustomEditor(typeof(ProjectileShooter))]
	//public class ProjectileShooterEditor : Editor
	//{
	//	public override void OnInspectorGUI()
	//	{
	//		ProjectileShooter projectileShooter = target as ProjectileShooter;

 //           DrawDefaultInspector();
	//		//DrawPropertiesExcluding(serializedObject.FindProperty("type"));
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
 //           // Conditionally show the 'curve' field only if 'type' is 'Curve'
 //           if (serializedObject.FindProperty("type").enumValueIndex == (int)Type.Curve)
 //           {
 //               // Display the curve field if the type is 'Curve'
 //               EditorGUILayout.PropertyField(serializedObject.FindProperty("curve"));
 //           }

 //           serializedObject.ApplyModifiedProperties();

 //       }
	//}
}
