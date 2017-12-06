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
        [Configurable]
        public string PixelSprite = "pixel.png";
        [Configurable]
        public int StartTime;
        [Configurable]
        public int SquareScale = 30;
        [Configurable]
        public int EndDuration = 500;
        [Configurable]
        public OsbEasing Easing;
        [Configurable]
        public Color4 Color = new Color4();
        public override void Generate()
        {
            float PosX = -107 + SquareScale/2;
            float PosY = SquareScale/2;
            float Speed = SquareScale*10;
            
		    while(PosY < 480)
            {
                if(PosX >= 854)
                {
                    PosX = -107 + SquareScale/2;
                    PosY += SquareScale;
                }
                
                var Sprite = GetLayer("Transitions").CreateSprite(PixelSprite, OsbOrigin.Centre);            
                Sprite.Move(StartTime, PosX, PosY);     
                Sprite.Rotate(StartTime, StartTime + EndDuration, 0, Math.PI/2);
                Sprite.ScaleVec(Easing, StartTime, StartTime + EndDuration, SquareScale, SquareScale, 0, 0);
                Sprite.Color(StartTime, Color);

                PosX += SquareScale;


            }
        }
    }
}