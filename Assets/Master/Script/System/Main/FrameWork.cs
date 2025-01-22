using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DCFrameWork.MainSystem
{
    public static class FrameWork
    {
        /// <summary>
        /// ������true�ȏꍇ�ɃR�����g���f�o�b�O���O�ɏo�͂���
        /// </summary>
        /// <param name="comment">���O�̃R�����g</param>
        /// <returns>�����������Ă��邩</returns>
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