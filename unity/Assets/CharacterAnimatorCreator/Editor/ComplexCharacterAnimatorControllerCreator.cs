using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;

public static class ComplexCharacterAnimatorControllerCreator
{
    public static RuntimeAnimatorController CreateAnimatorController(ComplexCharacterAnimatorControllerDefinition definition)
    {
        string hpParameterName = "HpRate";

        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(definition.ResulutPath);
        AnimatorStateMachine stateMachine = animatorController.layers.First().stateMachine;

        animatorController.AddParameter(new AnimatorControllerParameter
        {
            name = hpParameterName,
            defaultFloat = 1.0F,
            type = AnimatorControllerParameterType.Float
        });

        Dictionary<CharacterState, AnimatorState> stateDictionary = definition.AnimationClipDictionary.ToDictionary(it => it.Key, it =>
        {
            CharacterState characterState = it.Key;
            AnimationClip clip = it.Value;
            AnimatorState state = stateMachine.AddState(characterState.ToString());
            state.motion = clip;

            return state;
        });

        stateMachine.defaultState = stateDictionary[CharacterState.Default];

        {
            AnimatorState defaultState = stateDictionary[CharacterState.Default];
            AnimatorState attackState = stateDictionary[CharacterState.Attack];
            AnimatorStateTransition transition = attackState.AddTransition(defaultState);
            transition.hasExitTime = true;
            transition.exitTime = 1.0F;
            transition.duration = 0.0F;
            transition.hasFixedDuration = false;
        }

        {
            AnimatorState defaultState = stateDictionary[CharacterState.Default];
            AnimatorState hitState = stateDictionary[CharacterState.Hit];
            AnimatorStateTransition transition = hitState.AddTransition(defaultState);
            transition.hasExitTime = true;
            transition.exitTime = 1.0F;
            transition.duration = 0.0F;
            transition.hasFixedDuration = false;
        }

        {
            AnimatorState defaultState = stateDictionary[CharacterState.Default];
            AnimatorState pinchState = stateDictionary[CharacterState.Pinch];
            AnimatorStateTransition transition = defaultState.AddTransition(pinchState);
            transition.hasFixedDuration = true;
            transition.duration = 0.0F;
            transition.AddCondition(AnimatorConditionMode.Less, 0.3F, hpParameterName);
        }

        {
            AnimatorState defaultState = stateDictionary[CharacterState.Default];
            AnimatorState pinchState = stateDictionary[CharacterState.Pinch];
            AnimatorStateTransition transition = pinchState.AddTransition(defaultState);
            transition.hasFixedDuration = true;
            transition.duration = 0.0F;
            transition.AddCondition(AnimatorConditionMode.Greater, 0.3F, hpParameterName);
        }

        {
            AnimatorState pinchState = stateDictionary[CharacterState.Pinch];
            AnimatorState deadState = stateDictionary[CharacterState.Dead];
            AnimatorStateTransition transition = pinchState.AddTransition(deadState);
            transition.hasFixedDuration = true;
            transition.duration = 0.0F;
            transition.AddCondition(AnimatorConditionMode.Less, float.Epsilon, hpParameterName);
        }

        {
            AnimatorState deadState = stateDictionary[CharacterState.Dead];
            AnimatorState defaultState = stateDictionary[CharacterState.Default];
            AnimatorStateTransition transition = defaultState.AddTransition(deadState);
            transition.hasFixedDuration = true;
            transition.duration = 0.0F;
            transition.AddCondition(AnimatorConditionMode.Less, float.Epsilon, hpParameterName);
        }

        foreach (AnimationClip animationClip in definition.AnimationClipDictionary.Values)
        {
            AssetDatabase.AddObjectToAsset(animationClip, animatorController);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
        }

        EditorUtility.SetDirty(animatorController);

        return animatorController;
    }
}

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

public class ComplexCharacterAnimatorControllerDefinition
{
    public Dictionary<CharacterState, AnimationClip> AnimationClipDictionary { get; set; }

    public string ResulutPath { get; set; }
}
