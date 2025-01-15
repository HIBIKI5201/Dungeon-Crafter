using DCFrameWork.MainSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DCFrameWork.SceneSystem {
    public class StoryManager : MonoBehaviour {
        private AudioSource _audioSource;
        private Image _backGround1;
        private Image _backGround2;

        private IAsyncEnumerator<int?> _enumerator;
        private StoryData _storyData;

        private StoryUIManager _storyUIManager;

        [SerializeField]
        private GameObject _creato;
        private SpriteRenderer[] _creatoSprites;
        [SerializeField]
        private GameObject _labiris;
        private SpriteRenderer[] _labirisSprites;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Image[] images = GetComponentsInChildren<Image>();
            _backGround1 = images[0];
            _backGround2 = images[1];

            _creatoSprites = _creato.GetComponentsInChildren<SpriteRenderer>();
            _labirisSprites = _labiris.GetComponentsInChildren<SpriteRenderer>();
        }

        public void Initialize(StoryUIManager storyUIManager)
        {
            _enumerator = PlayStoryContext();
            _storyUIManager = storyUIManager;
        }

        public async void SetStoryData(StoryData storyData)
        {
            _storyData = storyData;
            SetBackGround(storyData.BackGround[0]);
        }

        public void NextText() => _enumerator.MoveNextAsync();

        private async IAsyncEnumerator<int?> PlayStoryContext()
        {
            int count = 0;

#if UNITY_EDITOR
            StorySystem storySystem = GetComponent<StorySystem>();
            storySystem.DebugStory();
#endif

            while (count < _storyData.StoryText.Count) {
                StoryText storyText = _storyData.StoryText[count];

                string[] animations = storyText.Animation.Split();
                await AnimationAsync(animations);

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
                yield return null;
            }
            EndStory();
        }

        private async Task AnimationAsync(string[] animations)
        {
            foreach (string animation in animations) {
                if (string.IsNullOrEmpty(animation)) {
                    continue;
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
            switch (character) {
                case CharacterEnum.Labiris:
                    ColorChange(_labirisSprites, Color.white);
                    ColorChange(_creatoSprites, Color.gray);
                    break;

                case CharacterEnum.Creato:
                    ColorChange(_creatoSprites, Color.white);
                    ColorChange(_labirisSprites, Color.gray);
                    break;

                case CharacterEnum.None:
                    ColorChange(_labirisSprites, Color.gray);
                    ColorChange(_creatoSprites, Color.gray);
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
