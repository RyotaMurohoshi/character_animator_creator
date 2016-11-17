using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public static class NoTransitionAnimatorControllerExample
{
    [MenuItem("Assets/CharacterAnimatorCreator/Example/No Transition Create AnimatorController")]
    public static void CreateAnimatorController()
    {
        IEnumerable<AnimationClip> animationClips = Selection.objects.OfType<AnimationClip>();
        if (!animationClips.Any())
        {
            Debug.LogWarning("Please selecting animation clips.");
            return;
        }

        var definition = new NoTransitionAnimatorControllerDefinition
        {
            AnimationClipList = animationClips.ToList(),
            LayerName = "Base Layer",
            ResulutPath = "Assets/Sample.controller",
        };

        NoTransitionAnimatorControllerCreator.CreateAnimatorController(definition);
    }
}
