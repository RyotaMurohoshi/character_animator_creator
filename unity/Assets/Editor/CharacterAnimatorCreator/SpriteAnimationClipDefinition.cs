using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SpriteAnimationClipDefinition
{
	public List<SpriteKeyframeDefinition> SpriteKeyframes { get; set; }

	public WrapMode WrapMode { get; set; }

	public bool IsLoop { get; set; }

	public float FrameRate { get; set; }

	public string ResulutPath { get; set; }
}

public class SpriteKeyframeDefinition
{
	public Sprite Value { get; set; }

	public float Time { get; set; }

	public static explicit operator ObjectReferenceKeyframe (SpriteKeyframeDefinition spriteKeyframe)
	{
		return new ObjectReferenceKeyframe {
			time = spriteKeyframe.Time,
			value = spriteKeyframe.Value
		};
	}
}