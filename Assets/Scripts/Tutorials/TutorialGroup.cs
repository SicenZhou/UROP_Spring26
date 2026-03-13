using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Tutorial Group", menuName = "Tutorial/Tutorial Group")]
public class TutorialGroup : ScriptableObject
{
    public string groupURL;

    public UnityEvent<TutorialObject> TutorialChanged;
    public int tutorialIndex;
    public string lessonName;
    public string overview;
    public TutorialObject lastTutorial;
    public TutorialObject CurrentTutorial
    {
        get { return currentTutorial; }
        set
        {
            if (currentTutorial != null)
                currentTutorial.ExitTutorial();


            currentTutorial = value;
            currentTutorial.EnterTutorial();
            tutorialIndex = tutorialObjects.IndexOf(value);
            //TutorialManager.instance.CurrentTutorialObject = currentTutorial;
            TutorialChanged?.Invoke(currentTutorial);
        }
    }
    [SerializeField]
    TutorialObject currentTutorial;

    public List<TutorialObject> tutorialObjects = new List<TutorialObject>();

    public void Initialize()
    {
        Reset();
        if (tutorialObjects.Count == 0)
        {
            Debug.LogError("No Tutorial objects have been added to group : " + name);
            return;


        }

        foreach (TutorialObject tutorialObject in tutorialObjects)
        {
            // if(tutorialObject as TutorialTestObject == null)
            // tutorialObject.index = tutorialObjects.IndexOf(tutorialObject); 
        }
        CurrentTutorial = tutorialObjects[0];
    }
    public void GoBack()
    {
        if (tutorialIndex - 1 >= 0)
        {
            tutorialIndex--;
            lastTutorial = CurrentTutorial;
            CurrentTutorial = tutorialObjects[tutorialIndex];
        }
    }
    public void GoToNextTutorial()
    {
        // if(CurrentTutorial as TutorialTestObject != null)
        // {
        //     if((CurrentTutorial as TutorialTestObject).Correct == true)
        //     {
        //         CurrentTutorial = (currentTutorial as TutorialTestObject).progressObject;

        //     }
        //     else
        //     {
        //     CurrentTutorial = (currentTutorial as TutorialTestObject).returnObject;

        //     }

        //     tutorialIndex = CurrentTutorial.index;
        //     return;
        // }

        if (tutorialIndex + 1 < tutorialObjects.Count)
        {
            tutorialIndex++;
            lastTutorial = CurrentTutorial;
            CurrentTutorial = tutorialObjects[tutorialIndex];
        }

    }
    /*    public void GoToTutorial(TutorialObject tutorialObject)
        {
            CurrentTutorial = tutorialObject;
        }*/

    private void Reset()
    {
        tutorialIndex = 0;
        lastTutorial = null;
        foreach (TutorialObject obj in tutorialObjects)
        {
            obj.Reset();
        }

    }

#if UNITY_EDITOR
    public void UpdateTutorials()
    {
        TutorialLoader.CreateTutorials(groupURL, Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)));
    }
    public void RenameTutorials()
    {
        int i = 0;
        foreach (TutorialObject obj in tutorialObjects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            AssetDatabase.RenameAsset(assetPath, i.ToString("D3"));

            obj.name = i.ToString("D3");
            obj.index = i;
            i++;
        }
        AssetDatabase.SaveAssets();
    }
#endif
}
