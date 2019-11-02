#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;


namespace Estecka.EsteckaEditor {
	public class RegexTester : EditorWindow {

		[MenuItem("Estecka/Regex")]
		static public void Init(){
			EditorWindow.GetWindow<RegexTester>(true, "Regex tester").Show(true);
		}


		string pattern = @"^(.*)$";
		string input = "";
		RegexOptions options;

		MatchCollection matches = null;

		void OnGUI(){
			EditorGUI.BeginChangeCheck();
			pattern = EditorGUILayout.DelayedTextField("Pattern", pattern);
			options = (RegexOptions)EditorGUILayout.EnumFlagsField("Options", options);
			input = EditorGUILayout.TextArea(input);

			if (EditorGUI.EndChangeCheck() || matches == null)
				matches = Regex.Matches(input, pattern, options);
			if (matches != null){
				int baseIndent = EditorGUI.indentLevel;
				foreach(Match m in matches){
					EditorGUILayout.LabelField(m.ToString());
					EditorGUI.indentLevel ++;
					foreach(Group g in m.Groups)
						EditorGUILayout.LabelField(g.Value);

					EditorGUI.indentLevel = baseIndent;
					EditorGUILayout.Separator();
				}
			}
		}

	} // END Window
} // END Namespace
#endif