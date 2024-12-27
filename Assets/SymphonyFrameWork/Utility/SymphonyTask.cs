using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SymphonyFrameWork.Editor
{
    public static class SymphonyTask
    {
        public static async void BackGroundThreadAction(Action action)
        {
            //バックグラウンドに移行する
            await Awaitable.BackgroundThreadAsync();
            action.Invoke();
            Debug.Log($"{action}の実行が終了しました");
        }

        public static async Task WaitUntil(Func<bool> action)
        {
            //trueが返ってくるまでループする
            while (!action.Invoke())
            {
                await Awaitable.NextFrameAsync();
            }
        }
    }
}