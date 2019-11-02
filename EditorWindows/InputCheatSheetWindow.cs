#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Estecka.EsteckaEditor {
	public sealed class InputCheatSheetWindow : EditorWindow {

		[MenuItem("Estecka/Input Cheat-Sheet")]
		static void Init(){
			EditorWindow.GetWindow<InputCheatSheetWindow> (true, "Input Cheat-Sheet").Show ();
		}

		List<KeyCode> events = new List<KeyCode>(5);

		void Awake(){
			events.Add (KeyCode.None);
			events.Add (KeyCode.None);
			events.Add (KeyCode.None);
			events.Add (KeyCode.None);
		}


		void OnGUI(){
			Event e = Event.current;
			if (e.type == EventType.KeyDown && e.keyCode!= KeyCode.None) {
				events.Add (e.keyCode);
				events.RemoveAt (0);
				Repaint ();
			}

			EditorGUI.BeginDisabledGroup (true);
			foreach (KeyCode k in events)
				EditorGUILayout.EnumPopup (GUIContent.none, k);
			EditorGUI.EndDisabledGroup ();

		}

	} // ENd Window
} // END Namespace
#endif