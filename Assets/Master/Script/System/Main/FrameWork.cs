using System.Collections;
using UnityEngine;

namespace DCFrameWork.MainSystem
{
    public static class FrameWork
    {
        /// <summary>
        /// 条件がfalseな場合にコメントをデバッグログに出力する
        /// </summary>
        /// <param name="comment">ログのコメント</param>
        /// <returns>条件が揃っているか</returns>
        public static bool CheckLog(this bool func, string comment)
        {
            if (!func)
            {
                Debug.Log(comment);
            }
            return func;
        }

        public static IEnumerator PausableWaitForSecond(float time)
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}