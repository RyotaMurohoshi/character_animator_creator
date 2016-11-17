using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

public static class SingleStateAnimatorControllerCreator
{
    public static RuntimeAnimatorController CreateAnimatorController(SingleStateAnimatorControllerDefinition definition)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(definition.ResulutPath);
        AnimatorControllerLayer layer = animatorController.layers.First();
        layer.name = definition.LayerName;
        AnimatorStateMachine stateMachine = layer.stateMachine;

        foreach (AnimationClip clip in definition.AnimationClipList)
        {
            AnimatorState state = stateMachine.AddState(clip.name);
            state.motion = clip;
        }

        EditorUtility.SetDirty(animatorController);

        return animatorController;
    }
}
