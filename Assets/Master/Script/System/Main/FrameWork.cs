using System.Collections;
using UnityEngine;

namespace DCFrameWork.MainSystem
{
    public static class FrameWork
    {
        /// <summary>
        /// ������false�ȏꍇ�ɃR�����g���f�o�b�O���O�ɏo�͂���
        /// </summary>
        /// <param name="comment">���O�̃R�����g</param>
        /// <returns>�����������Ă��邩</returns>
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