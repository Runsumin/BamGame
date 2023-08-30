using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace J2y.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // JActorRenderer_Sprites 
    //
    //	@Feature (Sprites Renderer)
    //  2D Spirtes Animation Renderer
    // SpriteAnimationChangeButton
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    [CustomEditor(typeof(JActorRenderer))]
    public class SpriteAnimationChangeButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            JActorRenderer generator = (JActorRenderer)target;
            if (GUILayout.Button("SpriteAnimationChange"))
            {
                generator.Sprite2D_ChangeAnimationState();
            }
        }
    }

    //[RequireComponent(typeof(SpriteRenderer))]
    public partial class JActorRenderer : JActorComponent
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Enum] 애니메이션 진행 방향
        public enum PlayBackType
        {
            Forward = 1,
            Backward = -1,
        }
        #endregion

        #region [Enum] 애니메이션 타입
        public enum AnimationType
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // NestedClass
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] 스프라이트 프레임 정보
        [Serializable]
        public class SpriteFrame
        {
            public Sprite sprite;
            public int Frames;

            public SpriteFrame(Sprite sprite, int frames = 1)
            {
                this.sprite = sprite;
                this.Frames = frames;
            }
        }
        #endregion

        #region [Nested] 스프라이트 프레임 배열
        [Serializable]
        public class SpriteFrameArray
        {
            public string StateName;
            public SpriteFrame[] FrameArray;
        }
        #endregion

        #region [Nested] Setting
        [Serializable]
        public class NSpritesAnimation
        {
            public bool PlayOnEnabled = true;                                           // 대기상태에서 플레이
            public bool Loop = true;                                                    // 애니메이션 루핑
            public int StartFrame;                                                      // 시작 프레임 지정
            public PlayBackType PlayBack = PlayBackType.Forward;                        // 플레이타입
            public List<SpriteFrameArray> FramesList = new List<SpriteFrameArray>();    // 애니메이션 데이터
        }
        public NSpritesAnimation SpriteAnimationSetting = new NSpritesAnimation();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Varitable] Sprite
        public SpriteRenderer spriteRenderer;
        public string[] UserSetState;
        public string NowState;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Sprites Information
        public bool IsPlaying { get; set; }
        public bool IsPaused { get; set; }
        public bool IsStoped { get; set; }
        public int CurrentFrameIndex { get; set; }
        public int CurrentSpriteFrameIndex { get; set; }
        public int CurrentStateIndex { get; set; }
        public SpriteFrame CurrentSpriteFrame => SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray[CurrentSpriteFrameIndex];
        public Sprite Sprite
        {
            get => spriteRenderer.sprite;
            set => spriteRenderer.sprite = value;
        }
        public int StartFrame
        {
            get => SpriteAnimationSetting.StartFrame;
            set => SpriteAnimationSetting.StartFrame = Sprite2D_HasFrames() ? Mathf.Clamp(value, 0, SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray.Length - 1) : 0;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] OnEnable
        //public override void OnEnable()
        //{
        //    base.OnEnable();
        //    Stop();
        //    if (SpriteAnimationSetting.PlayOnEndabled) Play();
        //}
        #endregion

        #region [Init] Start
        public void Sprites_Init()
        {
            if(string.IsNullOrEmpty(NowState))
            {
                NowState = UserSetState[0];
            }
        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Renderer
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Renderer] Reset
        protected override void Reset() => spriteRenderer = GetComponent<SpriteRenderer>();
        #endregion    

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Sprites Animation
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Animation] Play
        public void Sprite2D_Play()
        {
            IsPlaying = true;
            IsPaused = false;
            IsStoped = false;
        }
        #endregion

        #region [Animation] Pause
        public void Sprite2D_Pause()
        {
            IsPaused = true;
            IsPlaying = false;
            IsStoped = false;
        }
        #endregion

        #region [Animation] Stop
        public void Sprite2D_Stop()
        {
            IsPlaying = false;
            IsStoped = true;

            switch (SpriteAnimationSetting.PlayBack)
            {
                case PlayBackType.Forward:
                    CurrentFrameIndex = -1;
                    CurrentSpriteFrameIndex = 0;
                    Sprite = Sprite2D_HasFrames() ? SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray[0].sprite : null;
                    break;

                case PlayBackType.Backward:
                    Sprite2D_ResetIndexesBackwards();
                    var lastFrameIndex = SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray.Length - 1;
                    Sprite = Sprite2D_HasFrames() ? SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray[lastFrameIndex].sprite : null;
                    break;
            }
        }
        #endregion

        #region [Animation] Restart
        public void Sprite2D_Restart()
        {
            Sprite2D_Stop();
            Sprite2D_Play();
        }
        #endregion

        #region [Animation] CanPlay
        public bool Sprite2D_CanPlay() => !IsPlaying && Sprite2D_HasFrames();
        #endregion

        #region [Animation] HasFrames
        public bool Sprite2D_HasFrames() => SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray.Length > 0;
        #endregion

        #region [Animation] IsRenderingSprite
        public bool Sprite2D_IsRenderingSprite() => Sprite != null;
        #endregion

        #region [Animation] PlayNextFrame
        public void Sprite2D_PlayNextFrame()
        {
            //if (!IsPlaying) return;

            if (SpriteAnimationSetting.PlayBack == PlayBackType.Forward) Sprite2D_RenderNextFrame();
            else if (SpriteAnimationSetting.PlayBack == PlayBackType.Backward) Sprite2D_RenderPreviousFrame();
        }
        #endregion

        #region [Animation] RenderPreviousFrame
        public void Sprite2D_RenderPreviousFrame()
        {
            if (!Sprite2D_HasFrames()) return;

            IsStoped = false;
            var hasPreviousFrame = --CurrentFrameIndex < 0;

            if (hasPreviousFrame)
            {
                CurrentFrameIndex = CurrentSpriteFrame.Frames - 1;

                if (CurrentSpriteFrameIndex > 0) CurrentSpriteFrameIndex--;
                else
                {
                    if (SpriteAnimationSetting.Loop) Sprite2D_ResetIndexesBackwards();
                    else Sprite2D_Pause();
                }

                Sprite = CurrentSpriteFrame.sprite;
            }
        }
        #endregion

        #region [Animation] RenderNextFrame
        public void Sprite2D_RenderNextFrame()
        {
            if (!Sprite2D_HasFrames()) return;

            IsStoped = false;
            var hasNextFrame = ++CurrentFrameIndex >= CurrentSpriteFrame.Frames;

            if (hasNextFrame)
            {
                CurrentFrameIndex = 0;
                var hasNextSprite = CurrentSpriteFrameIndex < SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray.Length - 1;

                if (hasNextSprite) CurrentSpriteFrameIndex++;
                else
                {
                    if (SpriteAnimationSetting.Loop) CurrentSpriteFrameIndex = StartFrame;
                    else Sprite2D_Pause();
                }

                Sprite = CurrentSpriteFrame.sprite;
            }
        }
        #endregion

        #region [Animation] ResetIndexesBackwards
        private void Sprite2D_ResetIndexesBackwards()
        {
            var hasStartFrame = StartFrame != 0;
            CurrentSpriteFrameIndex = hasStartFrame ? StartFrame : SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray.Length - 1;
            SpriteFrame lastFrame = CurrentSpriteFrame;
            CurrentFrameIndex = lastFrame.Frames - 1;
        }
        #endregion

        #region [Animation] RemoveAllFrames
        public void Sprite2D_RemoveAllFrames() => SpriteAnimationSetting.FramesList[CurrentStateIndex].FrameArray = new SpriteFrame[0];
        #endregion

        #region [Animation] ChangeAnimationState
        public void Sprite2D_ChangeAnimationState()
        {
            CurrentStateIndex = Sprite2D_GetCurrentAnimationIndex();
            CurrentSpriteFrameIndex = 0;
        }
        #endregion

        #region [Animation] GetNowAnimationIndex
        public int Sprite2D_GetCurrentAnimationIndex()
        {
            int index = 0;
            foreach(var name in SpriteAnimationSetting.FramesList)
            {
                if (name.StateName == NowState)
                    break;
                else
                    index++;
            }
            return index;
        }
        #endregion
    }

}