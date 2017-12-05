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
    public class ParticleFlip : StoryboardObjectGenerator
    {
        [Configurable]
        public string SpritePath;
        [Configurable]
        public int Start;
        [Configurable]
        public int End;
        public override void Generate()
        {
            var beat = Beatmap.GetTimingPointAt(Start).BeatDuration;
		    int randSize;
            for(double i = Start; i < End; i += beat/8)
            {
                randSize = Random(3, 10);
                var Sprite = GetLayer("Particles").CreateSprite(SpritePath, OsbOrigin.Centre);
                Sprite.MoveX(i, Random(-107, 854));
                Sprite.MoveY(i, i + randSize*1000, 500, -20);
                Sprite.Fade(i, i + 100, 0, 1);
                Sprite.Fade(i + randSize*1000, i + randSize*1000, 1, 0);
                Sprite.ColorHsb(i, 0, 0, Random(0.0, 1.0));
                Sprite.Rotate(Start, End, 0, Random(-Math.PI, Math.PI));
                
                for(double i2 = Start; i2 < End; i2 += beat*10)
                {
                    Sprite.ScaleVec(i2, i2 += beat*Random(5, 10), randSize, randSize, -randSize, randSize);
                    
                }
            }
            
        }
    }
}
