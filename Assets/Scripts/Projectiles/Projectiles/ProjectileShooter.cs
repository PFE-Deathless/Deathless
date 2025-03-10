using UnityEditor;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
	public Type type = Type.Forward;
	public ProjectileObject projectile;
	public Transform origin;

	[HideInInspector] public Transform target;

	public enum Type
	{
		Forward,
		Homing,
		Curve,
	}

	public void ShootProjectile()
	{
		if (origin == null)
			origin = transform;
		GameObject obj = Instantiate(projectile.gameObject, origin.position, origin.rotation, GameManager.Instance.ProjectileParent);
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
