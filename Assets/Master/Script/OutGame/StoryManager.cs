using DCFrameWork.MainSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DCFrameWork.SceneSystem {
    public class StoryManager : MonoBehaviour {
        private AudioSource _audioSource;
        private Image _backGround1;
        private Image _backGround2;
        private Image _blackFade;

        private IAsyncEnumerator<int?> _enumerator;
        private bool _isPlay;
        private StoryData _storyData;

        private StoryUIManager _storyUIManager;

        [SerializeField]
        private GameObject _creato;
        private CharacterComponents _creatoComponents;
        [SerializeField]
        private GameObject _labiris;
        private CharacterComponents _labirisComponents;

        private struct CharacterComponents {
            public SpriteRenderer[] SpriteRenderers;
            public AudioSource AudioSource;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Image[] images = GetComponentsInChildren<Image>();
            _backGround1 = images[0];
            _backGround2 = images[1];
            _blackFade = images[2];

            _backGround2.color = new Color(1, 1, 1, 0);

            SetCharacterComponent(ref _creatoComponents, _creato);
            SetCharacterComponent(ref _labirisComponents, _labiris);

            void SetCharacterComponent(ref CharacterComponents character, GameObject target)
            {
                if (target is null) {
                    Debug.LogError($"�^�[�Q�b�g��������܂���");
                    return;
                }

                character = new CharacterComponents();
                character.SpriteRenderers = target.GetComponentsInChildren<SpriteRenderer>();
                if (character.SpriteRenderers == null) {
                    Debug.LogError($"{target.name}��SpriteRenderer��������܂���");
                }

                character.AudioSource = target.GetComponent<AudioSource>();
                if (character.AudioSource == null) {
                    Debug.LogError($"{target.name}��AudioSource��������܂���");
                }
            }
        }

        public void Initialize(StoryUIManager storyUIManager)
        {
            _enumerator = PlayStoryContext();
            _storyUIManager = storyUIManager;
        }

        public void SetStoryData(StoryData storyData)
        {
            _storyData = storyData;
            if (storyData.BackGround[0] != null) {
                _backGround1.sprite = storyData.BackGround[0];
            }
        }

        public void NextText()
        {
            if (!_isPlay) {
                _enumerator.MoveNextAsync();
            }
        }

        private async IAsyncEnumerator<int?> PlayStoryContext()
        {
            int count = 0;

#if UNITY_EDITOR
            StorySystem storySystem = GetComponent<StorySystem>();
            storySystem.DebugStory();
#endif

            while (count < _storyData.StoryText.Count) {
                _isPlay = true;
                StoryText storyText = _storyData.StoryText[count];

                string[] animations = storyText.Animation.Split();
                Task[] tasks = animations.Select(s => AnimationAsync(s)).ToArray();
                await Task.WhenAll(tasks);

                //�L�����N�^�[�𔻒�
                CharacterEnum character = storyText.Character switch {
                    "�N���G�g" => CharacterEnum.Creato,
                    "�H�H�H" => CharacterEnum.Creato,
                    "���r���X" => CharacterEnum.Labiris,
                    _ => CharacterEnum.None,
                };

                //�e�L�X�g�{�b�N�X���X�V
                _storyUIManager.TextBoxUpdate(storyText.Character, storyText.Text);

                //�T�E���h���Đ�
                if (storyText.AudioClip != null) {
                    AudioSource source = character switch {
                        CharacterEnum.Creato => _creatoComponents.AudioSource,
                        CharacterEnum.Labiris => _labirisComponents.AudioSource,
                        CharacterEnum.None => _audioSource
                    };

                    if (source is not null) {
                        PlayCharacterVoice(source, storyText.AudioClip);
                    }
                }

                //�����N���G�g�����r���X�Ȃ�n�C���C�g
                CharacterHighlight(character);

                count++;

                Debug.Log($"<b>{storyText.Character}</b> {storyText.Animation}\n{storyText.Text}");
                _isPlay = false;
                yield return null;
            }
            EndStory();


            void PlayCharacterVoice(AudioSource source, AudioClip clip)
            {
                //�O�̃{�C�X�̍Đ����~�߂�
                _audioSource.Stop();
                _creatoComponents.AudioSource.Stop();
                _labirisComponents.AudioSource.Stop();
                
                source.clip = clip;
                source.Play();
            }
        }

        private async Task AnimationAsync(string animation)
        {
            if (string.IsNullOrEmpty(animation)) {
                return;
            }

            //�A�j���[�V�����̎�ނ��擾
            string animationContext = new string(animation
                                .TakeWhile(c => c != '[')
                                .ToArray());

            //�A�j���[�V�������Ƃ̏���
            switch (animationContext) {
                case "�����G�o��":
                    string character = GetIndexer();
                    Debug.Log($"�����G�o�� <b>{character}</b>");
                    break;
                case "�Ó]����":
                    Debug.Log("�Ó]����");
                    await BlackFade(1);
                    break;
                case "�u���b�N�A�E�g":
                    Debug.Log("�u���b�N�A�E�g");
                    await BlackFade(3);
                    break;

                case "Jump":
                    Debug.Log("Jump");
                    break;
                default:
                    Debug.LogWarning($"<b><color=yellow>{animationContext}</color></b>�Ƃ����A�j���[�V�����͂���܂���");
                    break;
            }

            string GetIndexer()
            {
                //[]�ň͂��Ă��镔�����擾
                const string pattern = @"\[(.*?)\]";
                var matchCollection = Regex.Matches(animation, pattern);
                string indexer = string.Empty;
                if (matchCollection.Count > 0) {
                    indexer = matchCollection[0].Groups[1].Value;
                }
                return indexer;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind">1�̓t�F�[�h�C���A2�̓t�F�[�h�A�E�g�A3�̓u���b�N�A�E�g</param>
        private async Task BlackFade(int kind)
        {
            CancellationToken token = destroyCancellationToken;

            const float fadeSpeed = 20;

            switch (kind) {
                case 1:
                    for (int i = 0; i < fadeSpeed; i++) {
                        await Awaitable.FixedUpdateAsync(token);

                        Color color = _blackFade.color;
                        color.a -= 1f / fadeSpeed;
                        _blackFade.color = color;
                    }
                    break;

                case 2:
                    for (int i = 0; i < fadeSpeed; i++) {
                        await Awaitable.FixedUpdateAsync(token);

                        Color color = _blackFade.color;
                        color.a += 1f / fadeSpeed;
                        _blackFade.color = color;
                    }

                    break;

                case 3:
                    _blackFade.color = _blackFade.color + new Color(0, 0, 0, 1);
                    break;

                default:
                    Debug.LogWarning($"{kind}�Ƃ����t�F�[�h�͂���܂���");
                    break;
            }
        }

        private async Task FadeBackGround(Sprite sprite)
        {
            _backGround2.sprite = sprite;

            //�w�i1���t�F�[�h�A�E�g����
            while (_backGround1.color.a > 0) {
                Color newColor = _backGround1.color;
                newColor.a -= 0.5f * Time.deltaTime;
                _backGround1.color = newColor;
                await Awaitable.NextFrameAsync();
            }

            //�w�i1��V����sprite�ɕς��Ĕw�i2�Ɠ���ւ�
            _backGround1.sprite = sprite;
            _backGround1.color = Color.white;
        }

        private void CharacterHighlight(CharacterEnum character)
        {
            var labirisSprites = _labirisComponents.SpriteRenderers;
            var creatoSprites = _creatoComponents.SpriteRenderers;
            switch (character) {
                case CharacterEnum.Labiris:
                    ColorChange(labirisSprites, Color.white);
                    ColorChange(creatoSprites, Color.gray);
                    break;

                case CharacterEnum.Creato:
                    ColorChange(creatoSprites, Color.white);
                    ColorChange(labirisSprites, Color.gray);
                    break;

                case CharacterEnum.None:
                    ColorChange(labirisSprites, Color.gray);
                    ColorChange(creatoSprites, Color.gray);
                    break;
            }

            void ColorChange(SpriteRenderer[] renderers, Color color)
            {
                foreach (SpriteRenderer renderer in renderers) {
                    renderer.color = color;
                }
            }
        }

        private void EndStory()
        {
            var system = GameBaseSystem.sceneSystem as StorySystem;
            system.EndStory();
        }

        public bool SetAudioVolume(float volume)
        {
            if (volume < 0 || 1 < volume) {
                return false;
            }

            _audioSource.volume = volume;
            return true;
        }

        private enum CharacterEnum {
            None = 0,
            Creato = 1 << 1,
            Labiris = 1 << 2,
        }
    }
}
