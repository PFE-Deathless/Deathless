using UnityEditor;

public class CreateTemplateScript
{
	private const string pathToYourScriptTemplate = "Assets/Scripts/Editor/Templates/TemplateEnemy.cs.txt";

	[MenuItem(itemName: "Assets/Create/Scripting/Enemy Script", isValidateFunction: false, priority: 51)]
	public static void CreateScriptFromTemplate()
	{
		ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToYourScriptTemplate, "NewEnemy.cs");
	}
}