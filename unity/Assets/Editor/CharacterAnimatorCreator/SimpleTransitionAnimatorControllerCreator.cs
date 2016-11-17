using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

public static class SimpleTransitionAnimatorControllerCreator
{
    public static RuntimeAnimatorController CreateAnimatorController(SimpleTransitionAnimatorControllerDefinition definition)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(definition.ResulutPath);
        AnimatorControllerLayer layer = animatorController.layers.First();
        layer.name = definition.LayerName;
        AnimatorStateMachine stateMachine = layer.stateMachine;

        AnimatorState defaultState = stateMachine.AddState("Default");
        defaultState.motion = definition.DefaultAnimationClip;

        AnimatorState pinchState = stateMachine.AddState("Pinch");
        pinchState.motion = definition.PinchAnimationClip;

        AnimatorState hitState = stateMachine.AddState("Hit");
        hitState.motion = definition.HitAnimationClip;

        AnimatorState walkState = stateMachine.AddState("Walk");
        walkState.motion = definition.WalkAnimationClip;

        AnimatorControllerParameter hpRateParameter = new AnimatorControllerParameter
        {
            name = "HpRate",
            defaultFloat = 1.0F,
            type = AnimatorControllerParameterType.Float
        };
        animatorController.AddParameter(hpRateParameter);

        stateMachine.defaultState = defaultState;

        {
            AnimatorStateTransition defaultToPinchTransition = new AnimatorStateTransition
            {
                destinationState = pinchState,
                hasExitTime = false,
                duration = 0.0F,
            };
            defaultToPinchTransition.AddCondition(AnimatorConditionMode.Less, 0.3F, hpRateParameter.name);
            defaultState.AddTransition(defaultToPinchTransition);
        }

        {
            AnimatorStateTransition pinchToDefulatTransition = new AnimatorStateTransition
            {
                destinationState = defaultState,
                hasExitTime = false,
                duration = 0.0F,
            };
            pinchToDefulatTransition.AddCondition(AnimatorConditionMode.Greater, 0.3F, hpRateParameter.name);
            pinchState.AddTransition(pinchToDefulatTransition);
        }

        {
            AnimatorStateTransition transition = hitState.AddTransition(defaultState);
            transition.hasExitTime = true;
            transition.exitTime = 1.0F;
            transition.duration = 0.0F;
        }

        EditorUtility.SetDirty(animatorController);

        return animatorController;
    }
}

public class SimpleTransitionAnimatorControllerDefinition
{
    public AnimationClip DefaultAnimationClip { get; set; }

    public AnimationClip PinchAnimationClip { get; set; }

    public AnimationClip HitAnimationClip { get; set; }

    public AnimationClip WalkAnimationClip { get; set; }

    public string LayerName { get; set; }

    public string ResulutPath { get; set; }
}
