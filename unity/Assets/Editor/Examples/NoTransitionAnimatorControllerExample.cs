using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public static class NoTransitionAnimatorControllerExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Example/No Transition Create AnimatorController")]
    public static void Execute()
    {
        List<AnimationClip> animationClips = Selection.objects.OfType<AnimationClip>().ToList();
        if (!animationClips.Any())
        {
            Debug.LogWarning("Please selecting animation clips.");
            return;
        }

        NoTransitionAnimatorControllerDefinition definition = new NoTransitionAnimatorControllerDefinition
        {
            AnimationClipList = animationClips,
            ResulutPath = "Assets/Sample.controller",
        };

        NoTransitionAnimatorControllerCreator.Create(definition);
    }
}
