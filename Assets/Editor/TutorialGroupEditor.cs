using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TutorialGroup))]
public class TutorialGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TutorialGroup myTarget = (TutorialGroup)target;
        if (GUILayout.Button("Progress", GUILayout.Height(40) ))
        {
            myTarget.GoToNextTutorial();
        }
        
        if (GUILayout.Button("Update Tutorials", GUILayout.Height(40) ))
        {
            myTarget.UpdateTutorials();
        }
        if (GUILayout.Button("Rename Tutorials", GUILayout.Height(40)))
        {
            myTarget.RenameTutorials();
        }
    }
}
