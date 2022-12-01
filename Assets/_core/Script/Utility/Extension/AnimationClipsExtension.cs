using UnityEngine;

namespace _core.Script.Utility.Extension
{
    public static class AnimationClipsExtension
    {
        //get animation clip by name
        public static AnimationClip GetClip(this AnimationClip[] animationClips,string clipName)
        {
            foreach (var animationClip in animationClips)
            {
                if (animationClip.name.Equals(clipName))
                {
                    return animationClip;
                }
            }
            return null;
        }   
    }
}