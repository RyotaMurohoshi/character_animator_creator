using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public static class SingleStateAnimatorControllerCreator
{
    public static RuntimeAnimatorController CreateAnimatorController(SingleStateAnimatorControllerDefinition definition)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(definition.ResulutPath);
        AnimatorStateMachine stateMachine = animatorController.layers[0].stateMachine;

        foreach (AnimationClip clip in definition.AnimationClipList)
        {
            AnimatorState state = stateMachine.AddState(clip.name);
            state.motion = clip;
        }

        EditorUtility.SetDirty(animatorController);

        return animatorController;
    }
}
