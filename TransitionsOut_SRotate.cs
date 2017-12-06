using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class TransitionsOut_SRotate : StoryboardObjectGenerator
    {
        public enum Style { In, Out }
        [Configurable]
        public string PixelSprite = "sb/pixel.png";
        [Configurable]
        public Style TransitionStyle = Style.In;
        [Configurable]
        public OsbEasing TransitionEasing;
        [Configurable]
        public int StartTime;
        [Configurable]
        public int Duration = 1000;
        [Configurable]
        public int HoldDuration = 0;
        [Configurable]
        public int SquareScale = 30;
        [Configurable]
        public bool FadeInOutTransition = false;
        [Configurable]
        public float FadeInOut = 0;
        [Configurable]
        public float Fade = 1;
        [Configurable]
        public int FadeOutTime = 0;
        [Configurable]
        public bool Additive = false;
        [Configurable]
        public Color4 Color = Color4.White;
        public override void Generate()
        {
            float PosX = -107 + SquareScale / 2;
            float PosY = SquareScale / 2;
            float Speed = SquareScale * 10;

            while (PosY < 480)
            {
                if (PosX >= 854)
                {
                    PosX = -107 + SquareScale / 2;
                    PosY += SquareScale;
                }

                var Sprite = GetLayer("Transitions").CreateSprite(PixelSprite, OsbOrigin.Centre);

                if (TransitionStyle == Style.In)
                {
                    var FadeInTime = Duration;

                    if (FadeInOutTransition)
                    {
                        Sprite.Fade(StartTime - FadeInTime, StartTime, FadeInOut, Fade);
                    }
                    Sprite.Fade(StartTime, StartTime + HoldDuration, Fade, Fade);
                    Sprite.Fade(StartTime + HoldDuration, StartTime + HoldDuration + FadeOutTime, Fade, 0);
                    Sprite.Rotate(StartTime - Duration, StartTime, Math.PI / 2, 0);
                    Sprite.Rotate(StartTime, StartTime, 0, 0);
                    Sprite.ScaleVec(TransitionEasing, StartTime - Duration, StartTime, 0, 0, SquareScale, SquareScale);
                    Sprite.ScaleVec(TransitionEasing, StartTime, StartTime, SquareScale, SquareScale, SquareScale, SquareScale);

                    if (Additive)
                    {
                        Sprite.Additive(StartTime - Duration, StartTime);
                    }
                }

                if (TransitionStyle == Style.Out)
                {
                    if (FadeInOutTransition)
                    {
                        Sprite.Fade(StartTime, StartTime + Duration, Fade, FadeInOut);
                    }
                    Sprite.Fade(StartTime, StartTime + Duration, Fade, Fade);
                    Sprite.Rotate(StartTime, StartTime + HoldDuration, 0, 0);
                    Sprite.Rotate(StartTime + HoldDuration, StartTime + HoldDuration + Duration, Math.PI / 2, 0);
                    Sprite.ScaleVec(TransitionEasing, StartTime, StartTime + HoldDuration, SquareScale, SquareScale,SquareScale, SquareScale);
                    Sprite.ScaleVec(TransitionEasing, StartTime + HoldDuration, StartTime + HoldDuration + Duration, SquareScale, SquareScale, 0, 0);
                    
                    if (Additive)
                    {
                        Sprite.Additive(StartTime, StartTime + Duration);
                    }
                }

                Sprite.Move(StartTime, PosX, PosY);
                Sprite.Color(StartTime, Color);

                PosX += SquareScale;
            }
        }
    }
}