using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;
using System.Collections.Generic;

public static class NoTransitionAnimatorControllerCreator
{
    public static RuntimeAnimatorController CreateAnimatorController(NoTransitionAnimatorControllerDefinition definition)
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

public class NoTransitionAnimatorControllerDefinition
{
    public List<AnimationClip> AnimationClipList { get; set; }

    public string LayerName { get; set; }

    public string ResulutPath { get; set; }
}
