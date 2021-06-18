using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequence : MonoBehaviour
{

    public virtual void Play(Action onEndCallback) { }

    public virtual void Backwards(Action onEndCallback) { }

}
