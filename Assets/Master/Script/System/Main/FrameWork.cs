using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DCFrameWork.MainSystem
{
    public static class FrameWork
    {
        /// <summary>
        /// 条件がtrueな場合にコメントをデバッグログに出力する
        /// </summary>
        /// <param name="comment">ログのコメント</param>
        /// <returns>条件が揃っているか</returns>
        public static bool CheckLog(this bool func, string comment)
        {
            if (func)
            {
                Debug.LogWarning(comment);
            }
            return func;
        }

        public static IEnumerator PausableWaitForSecond(float time)
        {
            float timer = 0;
            while (timer < time)
            {
                if(!GameBaseSystem.IsPause)
                    timer += Time.deltaTime;
                yield return null;
            }
        }
        public static async Task PausableWaitForSecondAsync(float time, CancellationToken token = default)
        {
            float timer = 0;
            while (timer < time)
            {
                if (!GameBaseSystem.IsPause)
                {
                    timer += Time.deltaTime;
                }
                try
                {
                    await Awaitable.NextFrameAsync(token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }
}