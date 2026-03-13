using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
[CreateAssetMenu(fileName = "Tutorial Object", menuName = "Tutorial/Tutorial Object")]
public class TutorialObject : ScriptableObject
{
    public void OnEnable()
    {

    }

    #region Events
    public delegate void TutorialCompleted(bool completed);
    public TutorialCompleted TutorialCompletedEvent;

    public UnityEvent TutorialEnteredEvent;
    
    public UnityEvent TutorialExitedEvent;

    #endregion

    //public TutorialConditional tutorialConditional;


    public bool CanGoBack = true;
    public bool Completed
    {
        get {return completed;}
        set 
        {
            completed = value;
            TutorialCompletedEvent?.Invoke(completed);
        }
    }
    [SerializeField]
    bool completed;
    public string title;
    public string description;
    public int index;
    //public CameraContext cameraContext;
    
    public virtual void EnterTutorial()
    {
        Reset();

        //Debug.Log(name);

        TutorialEnteredEvent?.Invoke();
        // if (tutorialConditional != null)
        // {
        //     tutorialConditional.active = true;

        //     tutorialConditional.Initialize(this);
        // }
        // else 
        // {
        //     Completed = true;
        // }
    }
    public virtual void ExitTutorial()
    {
        // if (tutorialConditional != null)
        // {
        //     tutorialConditional.active = false;

        //     tutorialConditional.Nuetralize();
        // }
        TutorialExitedEvent?.Invoke();
    }

    public virtual void CheckCondition()
    {
        // if (tutorialConditional != null)
        //     tutorialConditional.CheckConditional();
    }

    public void ConditionalCompleted(bool condition)
    {
        //if(TutorialManager.instance.CurrentTutorialObject == this)
        //Completed = condition;

        //Debug.Log(name);
    }
    public virtual void Reset()
    {
        Completed = false;

        //tutorialConditional?.Reset();
    }
    public void TestEnter()
    {
        // if(TutorialManager.instance)
        // {
        //     TutorialManager.instance.container.currentTutorialGroup.CurrentTutorial = this;
        // }
    }
}
