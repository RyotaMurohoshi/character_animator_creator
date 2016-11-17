using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

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
        AnimationClip animClip = new AnimationClip
        {
            name = spriteAnimationDefinition.Name,
            frameRate = spriteAnimationDefinition.FrameRate,
            wrapMode = spriteAnimationDefinition.WrapMode
        };

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

public class SpriteAnimationClipDefinition
{
    public List<SpriteKeyframeDefinition> SpriteKeyframes { get; set; }
    public WrapMode WrapMode { get; set; }
    public bool IsLoop { get; set; }
    public float FrameRate { get; set; }
    public string Name { get; set; }
}

public class SpriteKeyframeDefinition
{
    public Sprite Value { get; set; }
    public float Time { get; set; }

    public static explicit operator ObjectReferenceKeyframe(SpriteKeyframeDefinition spriteKeyframe)
    {
        return new ObjectReferenceKeyframe
        {
            time = spriteKeyframe.Time,
            value = spriteKeyframe.Value
        };
    }
}
