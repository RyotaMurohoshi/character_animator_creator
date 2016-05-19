using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;

public enum CharacterState
{
    Default,
    Pinch,
    Dead,
    Attack,
    Hit,
    Left,
    Right,
    Up,
    Down,
    Walk,
    Win
}

public static class CharacterAnimatorControllerCreator
{
    public static RuntimeAnimatorController CreateAnimatorController(Dictionary<CharacterState, AnimationClip> animationClipDict, string resultPath)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(resultPath);
        AnimatorStateMachine stateMachine = animatorController.layers[0].stateMachine;

        AnimatorControllerParameter hpRateParameter = new AnimatorControllerParameter
        {
            name = "HpRate",
            defaultFloat = 1.0F,
            type = AnimatorControllerParameterType.Float
        };
        animatorController.AddParameter(hpRateParameter);

        Dictionary<CharacterState, AnimatorState> stateDictionary = animationClipDict.ToDictionary(it => it.Key, it =>
        {
            CharacterState characterState = it.Key;
            AnimationClip clip = it.Value;
            AnimatorState state = stateMachine.AddState(characterState.ToString());
            state.motion = clip;

            return state;
        });

        AnimatorState defaultState = stateDictionary[CharacterState.Default];
        stateMachine.defaultState = defaultState;

        {
            AnimatorState attackState = stateDictionary[CharacterState.Attack];
            AnimatorStateTransition transition = attackState.AddTransition(defaultState);
            transition.hasExitTime = true;
            transition.exitTime = 1.0F;
            transition.duration = 0.0F;
            transition.hasFixedDuration = false;
        }

        {
            AnimatorState hitState = stateDictionary[CharacterState.Hit];
            AnimatorStateTransition transition = hitState.AddTransition(defaultState);
            transition.hasExitTime = true;
            transition.exitTime = 1.0F;
            transition.duration = 0.0F;
            transition.hasFixedDuration = false;
        }

        {
            AnimatorState deadState = stateDictionary[CharacterState.Dead];
            AnimatorState pinchState = stateDictionary[CharacterState.Pinch];

            {
                AnimatorStateTransition defaultToPinchTransition = new AnimatorStateTransition
                {
                    destinationState = pinchState,
                    duration = 0.0F,
                    hasFixedDuration = true,
                };
                defaultToPinchTransition.AddCondition(AnimatorConditionMode.Less, 0.3F, hpRateParameter.name);
                defaultState.AddTransition(defaultToPinchTransition);
            }

            {
                AnimatorStateTransition pinchToDefulatTransition = new AnimatorStateTransition
                {
                    destinationState = defaultState,
                    duration = 0.0F,
                    hasFixedDuration = true,
                };
                pinchToDefulatTransition.AddCondition(AnimatorConditionMode.Greater, 0.3F, hpRateParameter.name);
                pinchState.AddTransition(pinchToDefulatTransition);
            }

            {
                AnimatorStateTransition pinchToDeadTransition = new AnimatorStateTransition
                {
                    destinationState = deadState,
                    duration = 0.0F,
                    hasFixedDuration = true,
                };
                pinchToDeadTransition.AddCondition(AnimatorConditionMode.Less, float.Epsilon, hpRateParameter.name);
                pinchState.AddTransition(pinchToDeadTransition);
            }

            {
                AnimatorStateTransition defaultToDeadTransition = new AnimatorStateTransition
                {
                    destinationState = deadState,
                    duration = 0.0F,
                    hasFixedDuration = true,
                };
                defaultToDeadTransition.AddCondition(AnimatorConditionMode.Less, float.Epsilon, hpRateParameter.name);
                defaultState.AddTransition(defaultToDeadTransition);
            }
        }

        foreach (AnimationClip animationClip in animationClipDict.Values)
        {
            AssetDatabase.AddObjectToAsset(animationClip, animatorController);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
        }

        EditorUtility.SetDirty(animatorController);

        return animatorController;
    }
}
