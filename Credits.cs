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
using System.Drawing;

namespace StorybrewScripts
{
    public class Credits : StoryboardObjectGenerator
    {
        public enum Style{Basic, LettersfromTop, LettersfromBot, LettersfromCenter, LettersfromDot, Line}
        
        [Configurable]
        public string Sentence;
        
        [Configurable]
        public Style Type;

        [Configurable]
        public int Start = 0;

        [Configurable]
        public int End = 0;

        [Configurable]
        public string FontName = "Verdana";

        [Configurable]
        public string SpritesPath = "sb/Sentences";

        [Configurable]
        public int FontSize = 26;

        [Configurable]
        public float FontScale = 0.5f;

        [Configurable]
        public Color4 FontColor = Color4.White;

        [Configurable]
        public FontStyle FontStyle = FontStyle.Regular;

        [Configurable]
        public int GlowRadius = 0;

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 100);

        [Configurable]
        public bool AdditiveGlow = true;

        [Configurable]
        public int OutlineThickness = 0;

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public int ShadowThickness = 0;

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 100);

        [Configurable]
        public Vector2 Padding = Vector2.Zero;

        [Configurable]
        public float SubtitleY = 400;

        [Configurable]
        public float letterX = 400;

        [Configurable]
        public bool PerCharacter = true;

        [Configurable]
        public bool TrimTransparency = true;



        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;
        public override void Generate()
        {
           var font = LoadFont(SpritesPath, new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
            },
            new FontGlow()
            {
                Radius = AdditiveGlow ? 0 : GlowRadius,
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });
              
            generatePerCharacter(font, false);
        }
        public void generatePerCharacter(FontGenerator font, bool additive)
        {
            double Timing = Start;
            double Timing2 = Start;
            double Beat = Beatmap.GetTimingPointAt(Start).BeatDuration;
            
            var letterY = 0;
            var lineWidth = 0f;
            var lineHeight = 0f;
                
            if(Type == Style.Line)
            {
                var line = GetLayer("Sentence").CreateSprite("sb/Particles/pixel.png",OsbOrigin.CentreLeft);
                line.MoveY(Start, SubtitleY);
                line.Fade(Start, End, 1, 1);
                line.MoveX(Start, -107);
                line.ScaleVec(Start, Start + 500, 0, 1, 850, 1);
                
            }
                foreach (var letter in Sentence)
                {
                    var texture = font.GetTexture(letter.ToString());
                    lineWidth += texture.BaseWidth * FontScale;
                    lineHeight = Math.Max(lineHeight, texture.BaseHeight * FontScale);
                }
            var letterCenter = letterX;
            letterX = letterX - lineWidth * 0.5f;
            foreach (var letter in Sentence)
            {
                
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var position = new Vector2(letterX, letterY)
                        + texture.OffsetFor(Origin) * FontScale;

                    var sprite = GetLayer("Sentence").CreateSprite(texture.Path, Origin, position);

                    sprite.Scale(Start, FontScale);


                    if(Type == Style.Basic)
                    {
                        sprite.Fade(Start - Beat, Start, 0, 1);
                        sprite.Fade(End, End + Beat, 1, 0);
                        sprite.MoveY(Start, SubtitleY);
                        sprite.MoveX(Start, position.X);
                    }

                    else if(Type == Style.LettersfromTop)
                    {
                        sprite.Fade(Timing, Timing + Beat, 0, 1);
                        sprite.Fade(End, End + Beat, 1, 0);
                        sprite.MoveX(Start, position.X);  
                        sprite.MoveY(OsbEasing.OutCirc, Timing, Timing + Beat, SubtitleY - 10, SubtitleY);  
                        Timing += Beat/16;
                    }

                    else if(Type == Style.LettersfromBot)
                    {
                        sprite.Fade(Timing, Timing + Beat, 0, 1);
                        sprite.Fade(End, End + Beat, 1, 0);
                        sprite.MoveX(Start, position.X);  
                        sprite.MoveY(OsbEasing.OutCirc, Timing, Timing + Beat, SubtitleY + 10, SubtitleY);  
                        Timing += Beat/2;
                    }
                    
                    else if(Type == Style.LettersfromCenter)
                    {
                        sprite.Fade(Timing - Beat, Timing, 0, 1);
                        sprite.Fade(End, End + Beat, 1, 0);
                        sprite.MoveX(OsbEasing.OutBack, Timing - Beat, Timing + Beat, letterCenter, position.X);  
                        sprite.MoveY(Start, SubtitleY);
                        Timing += Beat/4;
                    }
                    
                    else if(Type == Style.LettersfromDot)
                    {
                        sprite.Fade(Timing, Timing + Beat, 0, 1);
                        sprite.Fade(End, End + Beat, 1, 0);
                        sprite.MoveX(Start, position.X);
                        sprite.MoveY(OsbEasing.OutCirc, Timing, Timing + Beat, SubtitleY + 10, SubtitleY);
                        sprite.Scale(OsbEasing.OutBack, Timing, Timing + Beat*4, 0, FontScale);
                        Timing += Beat/4;
                    }
                    
                    else if(Type == Style.Line)
                    {
                        sprite.Fade(Timing, Timing + Beat, 0, 1);
                        sprite.Fade(End, End + Beat, 1, 0);
                        sprite.MoveX(Start, position.X);
                        sprite.MoveY(Timing, SubtitleY);
                    }
                    if (additive) sprite.Additive(Start - 200, End);


                }
                letterX += texture.BaseWidth * FontScale;
            }
        }
    }
}
