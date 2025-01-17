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

            _creatoComponents = new CharacterComponents();
            _creatoComponents.SpriteRenderers = _creato.GetComponentsInChildren<SpriteRenderer>();
            _creatoComponents.AudioSource = _creato.GetComponent<AudioSource>();

            _labirisComponents = new CharacterComponents();
            _labirisComponents.SpriteRenderers = _labiris.GetComponentsInChildren<SpriteRenderer>();
            _labirisComponents.AudioSource = _labiris.GetComponent<AudioSource>();
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
                SetBackGround(storyData.BackGround[0]);
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

                //サウンドとUIを更新
                _storyUIManager.TextBoxUpdate(storyText.Character, storyText.Text);
                if (storyText.AudioClip != null) {
                    _audioSource.PlayOneShot(storyText.AudioClip);
                }

                //もしクリエトかラビリスならハイライト
                CharacterEnum character = storyText.Character switch {
                    "クリエト" => CharacterEnum.Creato,
                    "ラビリス" => CharacterEnum.Labiris,
                    _ => CharacterEnum.None,
                };
                CharacterHighlight(character);

                count++;

                Debug.Log($"<b>{storyText.Character}</b> {storyText.Animation}\n{storyText.Text}");
                _isPlay = false;
                yield return null;
            }
            EndStory();
        }

        private async Task AnimationAsync(string animation)
        {
            if (string.IsNullOrEmpty(animation)) {
                return;
            }

            //アニメーションの種類を取得
            string animationContext = new string(animation
                                .TakeWhile(c => c != '[')
                                .ToArray());

            //アニメーションごとの処理
            switch (animationContext) {
                case "立ち絵出現":
                    string character = GetIndexer();
                    Debug.Log($"立ち絵出現 <b>{character}</b>");
                    break;
                case "暗転解除":
                    Debug.Log("暗転解除");
                    await BlackFade(1);
                    break;
                case "ブラックアウト":
                    Debug.Log("ブラックアウト");
                    await BlackFade(3);
                    break;

                case "Jump":
                    Debug.Log("Jump");
                    break;
                default:
                    Debug.LogWarning($"<b><color=yellow>{animationContext}</color></b>というアニメーションはありません");
                    break;
            }

            string GetIndexer()
            {
                //[]で囲ってある部分を取得
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
        /// <param name="kind">1はフェードイン、2はフェードアウト、3はブラックアウト</param>
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
                    Debug.LogWarning($"{kind}というフェードはありません");
                    break;
            }
        }

        private void SetBackGround(Sprite sprite)
        {
            _backGround1.sprite = sprite;
        }

        private async Task FadeBackGround(Sprite sprite)
        {
            _backGround2.sprite = sprite;

            //背景1をフェードアウトする
            while (_backGround1.color.a > 0) {
                Color newColor = _backGround1.color;
                newColor.a -= 0.5f * Time.deltaTime;
                _backGround1.color = newColor;
                await Awaitable.NextFrameAsync();
            }
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
