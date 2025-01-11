using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SymphonyFrameWork.CoreSystem
{
    /// <summary>
    /// �V���O���g���̃C���X�^���X�𓝊����ĊǗ�����N���X
    /// </summary>
    //�C���X�^���X���ꎞ�I�ɃV�[�����[�h����؂藣���������ɂ��g�p�ł���B
    public static class SingletonDirector
    {
        [Tooltip("�V���O���g��������C���X�^���X�̃R���e�i")]
        private static GameObject _instance;
        [Tooltip("�V���O���g���o�^����Ă���^�̃C���X�^���X����")]
        private static Dictionary<Type, MonoBehaviour> _singletonObjects = new();

        /// <summary>
        /// �C���X�^���X�R���e�i�������ꍇ�ɐ�������
        /// </summary>
        private static void CreateInstance()
        {
            if (_instance is not null) return;

            //�C���X�^���X�𐶐����Đ�p�̃V�[���ɕێ�����B
            GameObject instance = new GameObject("SingltonDirector");
            Scene systemScene = SceneManager.CreateScene("SymphonySystem");
            SceneManager.MoveGameObjectToScene(instance, systemScene);
            _instance = instance;
        }

        /// <summary>
        /// �����ꂽMonoBehaviour���p������N���X���V���O���g��������
        /// </summary>
        /// <typeparam name="T">�V���O���g��������^</typeparam>
        /// <param name="instance">�V���O���g���C���X�^���X</param>
        /// <returns>�����ɒǉ�������������true�A���s������false</returns>
        public static void SetSinglton<T>(T instance) where T : MonoBehaviour
        {
            CreateInstance();

            // �C���X�^���X�����ɓo�^����Ă���ꍇ�͔j������B
            if (!_singletonObjects.TryAdd(typeof(T), instance))
            {
                Object.Destroy(instance.gameObject);
            }

            //�V���O���g���C���X�^���X�̎q�I�u�W�F�N�g�ɂ��ĕی�B
            Debug.Log($"{typeof(T).Name}�N���X��{instance.name}���V���O���g���o�^����܂���");
            instance.transform.SetParent(_instance.transform);
        }

        /// <summary>
        /// �^�̃N���X���V���O���g�������Ă����ꍇ�̓C���X�^���X��Ԃ�
        /// </summary>
        /// <typeparam name="T">�擾�������V���O���g���C���X�^���X�̌^</typeparam>
        /// <returns>�w�肵���^�̃C���X�^���X</returns>
        public static T GetSingleton<T>() where T : MonoBehaviour
        {
            if (_singletonObjects.TryGetValue(typeof(T), out MonoBehaviour md))
            {
                return md as T;
            }

            Debug.LogError($"{typeof(T).Name} �͓o�^����Ă��܂���B");
            return null;
        }

        /// <summary>
        /// �w�肵���^�̃C���X�^���X��j������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DestroySingleton<T>() where T : MonoBehaviour
        {
            if (_singletonObjects.TryGetValue(typeof(T), out MonoBehaviour md))
            {
                Object.Destroy(md.gameObject);
                _singletonObjects.Remove(typeof(T));
                Debug.Log($"{typeof(T).Name}���j������܂���");
            }
            else
            {
                Debug.Log($"{typeof(T).Name}�̓V���O���g���o�^����Ă��܂���");
            }
        }
    }
}