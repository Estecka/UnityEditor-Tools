using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Reflection;

namespace Estecka.EsteckaEditor {
	public class IconAssignationWindow : EditorWindow {
		static MethodInfo CopyMonoScriptIconToImporters = typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static|BindingFlags.NonPublic);
		static MethodInfo SetIconForObject = typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static|BindingFlags.NonPublic);

		[SerializeField] string iconName;
		[SerializeField] GUIContent icon;

		static readonly string iconSuffix = " icon";
		static readonly float
			iconMinSize = 19,
			iconMaxSize = 67;


		static void AssignIcon(Object target, GUIContent icon){
			if (target == null || icon == null)
				throw new System.ArgumentNullException ();
			Texture2D tex = icon.image as Texture2D;
			if (tex == null) {
				Debug.LogError ("Invalid Icon format : Not a Texture2D");
				return;
			}

			SetIconForObject.Invoke(null, new[]{target, tex});

			MonoScript monoScript = target as MonoScript;
			if (monoScript){
				CopyMonoScriptIconToImporters.Invoke(null, new []{ monoScript });
			}

		}//



		[MenuItem("Estecka/Icon Assignation")]
		static void Init(){
			EditorWindow.GetWindow<IconAssignationWindow> (true, "Icon Assigner").Show ();
		}//

		public void OnGUI(){
			Selection.activeObject = EditorGUILayout.ObjectField ("Target Object", Selection.activeObject, typeof(Object), false);

			EditorGUI.BeginChangeCheck ();
			iconName = EditorGUILayout.DelayedTextField ("Icon Name", iconName);

			if (EditorGUI.EndChangeCheck()){
				if (!iconName.EndsWith(iconSuffix))
					iconName += iconSuffix;

				try {
					icon = EditorGUIUtility.IconContent (iconName);
				}
				catch (System.Exception e){
					Debug.LogError (e);
				}
			}

			if (icon == null)
				EditorGUILayout.LabelField ("No icon found");
			else {
				Rect pos;

				pos = EditorGUILayout.GetControlRect (false, iconMaxSize);
				EditorGUI.LabelField (pos, icon);
				pos.x += iconMaxSize;
				pos.height = iconMinSize;
				EditorGUI.LabelField (pos, icon);

				if (Selection.activeObject && GUILayout.Button ("Perform Assignation"))
					AssignIcon (Selection.activeObject, icon);

			}
		}//


	} // END Window
}// ENd Namespace