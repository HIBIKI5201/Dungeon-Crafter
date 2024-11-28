using UnityEditor.Build.Content;
using System;
using UnityEngine;
using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;

namespace DCFrameWork
{
    public class StageSelectManager 
    {
        public void StageSelect(SceneKind kind)
        {
            GameBaseSystem.mainSystem.LoadScene(kind);
        }
        public void StageSelect(SceneKind kind,StoryData storydata)
        {
            GameBaseSystem.mainSystem.LoadScene<StorySystem>(kind,x => x.SetStoryData(storydata));
        }
    }
}
