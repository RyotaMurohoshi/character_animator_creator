using UnityEngine;
using UnityEditor.Animations;
using System.Linq;

public static class SimpleCharacterAnimatorControllerCreator
{
    public static RuntimeAnimatorController Create(
        SimpleCharacterAnimatorControllerDefinition definition)
    {
        AnimatorController animatorController =
            AnimatorController.CreateAnimatorControllerAtPath(definition.ResulutPath);
        AnimatorStateMachine stateMachine =
            animatorController.layers.First().stateMachine;

        AnimatorState defaultState = stateMachine.AddState("Default");
        defaultState.motion = definition.DefaultAnimationClip;

        AnimatorState pinchState = stateMachine.AddState("Pinch");
        pinchState.motion = definition.PinchAnimationClip;

        AnimatorState hitState = stateMachine.AddState("Hit");
        hitState.motion = definition.HitAnimationClip;

        AnimatorState walkState = stateMachine.AddState("Walk");
        walkState.motion = definition.WalkAnimationClip;

        AnimatorControllerParameter hpRateParameter =
            new AnimatorControllerParameter
            {
                name = "HpRate",
                defaultFloat = 1.0F,
                type = AnimatorControllerParameterType.Float
            };
        animatorController.AddParameter(hpRateParameter);

        stateMachine.defaultState = defaultState;

        {
            AnimatorStateTransition transition = new AnimatorStateTransition
            {
                destinationState = defaultState,
                hasExitTime = true,
                exitTime = 1.0F,
                duration = 0.0F
            };
            hitState.AddTransition(transition);
        }

        {
            AnimatorStateTransition transition = new AnimatorStateTransition
            {
                destinationState = pinchState,
                hasExitTime = false,
                duration = 0.0F,
            };
            transition.AddCondition(AnimatorConditionMode.Less, 0.3F, hpRateParameter.name);
            defaultState.AddTransition(transition);
        }

        {
            AnimatorStateTransition transition = new AnimatorStateTransition
            {
                destinationState = defaultState,
                hasExitTime = false,
                duration = 0.0F,
            };
            transition.AddCondition(AnimatorConditionMode.Greater, 0.3F, hpRateParameter.name);
            pinchState.AddTransition(transition);
        }

        return animatorController;
    }
}

public class SimpleCharacterAnimatorControllerDefinition
{
    public AnimationClip DefaultAnimationClip { get; set; }

    public AnimationClip PinchAnimationClip { get; set; }

    public AnimationClip HitAnimationClip { get; set; }

    public AnimationClip WalkAnimationClip { get; set; }

    public string ResulutPath { get; set; }
}
