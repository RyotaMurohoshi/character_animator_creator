using UnityEngine;
using UnityEditor;
using System.Linq;

public static class SpriteAnimationClipCreator
{
    public static AnimationClip CreateAnimationClip(SpriteAnimationClipDefinition spriteAnimationDefinition)
    {
        AnimationClip animClip = ConvertToAnimationClip(spriteAnimationDefinition);

        AssetDatabase.CreateAsset(animClip, spriteAnimationDefinition.Name);

        EditorUtility.SetDirty(animClip);

        return animClip;
    }

    public static AnimationClip ConvertToAnimationClip(SpriteAnimationClipDefinition spriteAnimationDefinition)
    {
        AnimationClip animClip = new AnimationClip();
        animClip.name = spriteAnimationDefinition.Name;
        animClip.frameRate = spriteAnimationDefinition.FrameRate;
        animClip.wrapMode = spriteAnimationDefinition.WrapMode;

        if (spriteAnimationDefinition.IsLoop)
        {
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(animClip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(animClip, settings);
        }

        EditorCurveBinding editorCurveBinding = EditorCurveBinding.PPtrCurve(string.Empty, typeof(SpriteRenderer), "m_Sprite");

        ObjectReferenceKeyframe[] keyframes = spriteAnimationDefinition.SpriteKeyframes
            .Select(spriteKeyframe => (ObjectReferenceKeyframe)spriteKeyframe)
            .ToArray();

        AnimationUtility.SetObjectReferenceCurve(animClip, editorCurveBinding, keyframes);

        return animClip;
    }
}

