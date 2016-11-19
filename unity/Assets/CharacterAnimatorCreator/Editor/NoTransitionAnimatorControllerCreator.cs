using UnityEngine;
using UnityEditor.Animations;
using System.Linq;
using System.Collections.Generic;

public static class NoTransitionAnimatorControllerCreator
{
    public static RuntimeAnimatorController Create(NoTransitionAnimatorControllerDefinition definition)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(definition.ResulutPath);
        AnimatorStateMachine stateMachine = animatorController.layers.First().stateMachine;

        foreach (AnimationClip clip in definition.AnimationClipList)
        {
            AnimatorState state = stateMachine.AddState(clip.name);
            state.motion = clip;
        }

        return animatorController;
    }
}

public class NoTransitionAnimatorControllerDefinition
{
    public List<AnimationClip> AnimationClipList { get; set; }

    public string ResulutPath { get; set; }
}
