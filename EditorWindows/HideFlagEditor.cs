#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Estecka.EsteckaEditor {
	public class HideFlagEditor : EditorWindow {
		[System.Flags] private enum PseudoHideFlags {
			HideInHierarchy 	= HideFlags.HideInHierarchy, 	// 0x1
			HideInInspector 	= HideFlags.HideInInspector, 	// 0x2
			DontSaveInEditor 	= HideFlags.DontSaveInEditor,	// 0x4
			NotEditable 		= HideFlags.NotEditable, 		// 0x8
			DontSaveInBuild 	= HideFlags.DontSaveInBuild, 	// 0x10
			DontUnloadUnusedAsset = HideFlags.DontUnloadUnusedAsset, // 0x20
		}

		// -- METHODS --
		static List<GameObject> found = new List<GameObject> ();
		void FindHiddenObjects(bool safe = true){
			foreach (GameObject go in safe ? GameObject.FindObjectsOfType<GameObject>() : Resources.FindObjectsOfTypeAll<GameObject>())
				if (go.transform && (go.hideFlags & HideFlags.HideInHierarchy) != HideFlags.None)
					found.Add (go);
		}
		void SafeAdd(GameObject go){
			if (go && !found.Contains (go))
				found.Add (go);
		}


		// -- ROUTINES --
		[MenuItem("Estecka/HideFlag Editor")]
		static void Init(){ 
			var win = EditorWindow.GetWindow<HideFlagEditor> (title: "HideFlag Editor", utility: true);
			win.Show (); 
			win.FindHiddenObjects ();
		}
		Vector2 scroll;
		public void OnGUI(){
			Rect position, labelPos, controlPos, xPos;

			EditorGUILayout.Space ();
			if (GUILayout.Button ("Add Current Selection"))
				foreach (GameObject go in Selection.gameObjects)
					SafeAdd(go);

			position = EditorGUILayout.GetControlRect ();
			position.width *= 1/2f;
			if (GUI.Button(position, "Add hidden objects"))
				FindHiddenObjects ();
			position.x += position.width;
			if (GUI.Button(position, "Add ALL hidden objects (unsafe)"))
				FindHiddenObjects (safe:false);



			
			EditorGUILayout.Space ();
			if (found.Count == 0) {
				EditorGUILayout.HelpBox ("No hidden objects were found.", MessageType.Info);
			} 
			else {
				if (GUILayout.Button ("Clear list"))
					found.Clear ();
				scroll = GUILayout.BeginScrollView (scroll);
				
				for (int i=0; i<found.Count; i++)
					if (found[i] != null ) {
						GameObject go = found [i];

						position = EditorGUILayout.GetControlRect ();
						xPos = position;
						xPos.width = EditorGUIUtility.singleLineHeight;
						position.x 	   += xPos.width;
						position.width -= xPos.width;

						labelPos = position;
						labelPos.width = EditorGUIUtility.labelWidth;

						controlPos = position;
						controlPos.x 	 += labelPos.width;
						controlPos.width -= labelPos.width;

						// Remove Button
						if (GUI.Button (xPos, "X")) {
							found.RemoveAt (i);
							i--;
							continue;
						}

						// GameObject Label
						labelPos.width += 16;
						EditorGUI.BeginDisabledGroup ((go.hideFlags & HideFlags.HideInHierarchy) != HideFlags.None);
						EditorGUI.ObjectField (labelPos, go, typeof(GameObject), true);
						EditorGUI.EndDisabledGroup ();

						// HideFlag Popup
						controlPos.width *= 2/3f;
						go.hideFlags = (HideFlags)EditorGUI.EnumFlagsField (controlPos, GUIContent.none, (PseudoHideFlags)go.hideFlags);

						// Display Hex Value
						controlPos.x += controlPos.width;
						controlPos.width *= 1/2f;
						EditorGUI.LabelField(controlPos, "0x"+((uint)go.hideFlags).ToString("x"));
					}
				GUILayout.EndScrollView ();
			}
		}
		
	} // END Windows
} // END Namespace
#endif