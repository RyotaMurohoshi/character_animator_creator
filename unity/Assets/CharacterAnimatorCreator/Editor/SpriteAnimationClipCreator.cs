using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public static class SpriteAnimationClipCreator
{
    public static AnimationClip Create(SpriteAnimationClipDefinition spriteAnimationDefinition)
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
            .Select(spriteKeyframe => spriteKeyframe.ToObjectReferenceKeyframe())
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

    public ObjectReferenceKeyframe ToObjectReferenceKeyframe()
    {
        return new ObjectReferenceKeyframe
        {
            time = this.Time,
            value = this.Value
        };
    }
}
