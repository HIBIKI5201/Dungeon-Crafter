using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SymphonyFrameWork.Editor
{
    public static class SymphonyTask
    {
        public static async void BackGroundThreadAction(Action action)
        {
            //�o�b�N�O���E���h�Ɉڍs����
            await Awaitable.BackgroundThreadAsync();
            action.Invoke();
            Debug.Log($"{action}�̎��s���I�����܂���");
        }

        public static async Task WaitUntil(Func<bool> action)
        {
            //true���Ԃ��Ă���܂Ń��[�v����
            while (!action.Invoke())
            {
                await Awaitable.NextFrameAsync();
            }
        }
    }
}