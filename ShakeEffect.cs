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
    public class ShakeEffect : StoryboardObjectGenerator
    {
        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;

        [Configurable]
        public string SpritePath = "sb/shake.png";

        [Configurable]
        public int StartTime = 0;

        [Configurable]
        public int EndTime = 10000;

        [Configurable]
        public OsbEasing MoveEasing = OsbEasing.None;

        [Configurable]
        public float StartX = 320;

        [Configurable]
        public float EndX = 320;

        [Configurable]
        public float StartY = 240;

        [Configurable]
        public float EndY = 240;

        [Configurable]
        public bool Shake = true;

        [Configurable]
        public int ShakeAmount = 100;

        [Configurable]
        public int radiusMin = 0;

        [Configurable]
        public int Radius = 20;

        [Configurable]
        public OsbEasing ShakeEasing = OsbEasing.None;

        [Configurable]
        public double Scale = 1;

        [Configurable]
        public double Fade = 1;

        [Configurable]
        public int FadeTime = 200;

        [Configurable]
        public bool randomRotate = false;

        [Configurable]
        public double MinRotation = 0f;

        [Configurable]
        public double MaxRotation = 0f;

        [Configurable]
        public bool Additive = false;

        [Configurable]
        public Color4 Color = Color4.Red;

        public Vector2 CirclePos(double angle, int radius)
        {
            var MoveX = new Vector2(StartX, EndX);
            var MoveY = new Vector2(StartY, EndY);

            double posX = StartX + (radius * Math.Cos(angle));
            double posY = StartY + (radius * Math.Sin(angle));

            return new Vector2((float)posX, (float)posY);
        }

        public override void Generate()
        {
            var spriteLayer = GetLayer("");
            var sprite = spriteLayer.CreateSprite(SpritePath, Origin);

            var MoveX = new Vector2(StartX, EndX);
            var MoveY = new Vector2(StartY, EndY);

            sprite.Fade(OsbEasing.None, StartTime, StartTime + FadeTime, 0, Fade);
            sprite.Fade(OsbEasing.None, EndTime - FadeTime, EndTime, Fade, 0);
            sprite.Scale(StartTime, Scale);
            sprite.Color(StartTime, Color);
            sprite.MoveX(MoveEasing, StartTime, EndTime, StartX, EndX);
            sprite.MoveY(MoveEasing, StartTime, EndTime, StartY, EndY);

            if (randomRotate)
                {
                    var angle = Random(MinRotation, MaxRotation);
                    sprite.Rotate(EndTime, MathHelper.DegreesToRadians(angle));
                    }

            if (Additive)
            {
                sprite.Additive(StartTime, EndTime);
            }

            if (Shake)
            {


                var angleCurrent = 0d;
                var radiusCurrent = 0;
                for (int i = StartTime; i < EndTime - ShakeAmount; i += ShakeAmount)
                {
                    var angle = Random(angleCurrent - Math.PI / 4, angleCurrent + Math.PI / 4);
                    var radius = Math.Abs(Random(radiusCurrent - Radius / 4, radiusCurrent + Radius / 4));

                    while (radius > Radius)
                    {
                    radius = Math.Abs(Random(radiusCurrent - Radius / 4, radiusCurrent + Radius / 4));
                    }

                    var start = sprite.PositionAt(i);
                    var end = CirclePos(angle, radius);

                    if (i + ShakeAmount >= EndTime)
                    {
                    sprite.Move(ShakeEasing, i, EndTime, start, end);
                    }
                    else
                    {
                    sprite.Move(ShakeEasing, i, i + ShakeAmount, start, end);
                    }

                    angleCurrent = angle;
                    radiusCurrent = radius;
                }
            }
        }
    }
}