using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;
using System.Drawing;
using System.IO;

namespace StorybrewScripts
{
    public class LyricsKala : StoryboardObjectGenerator
    {
        [Configurable]
        public OsbOrigin LyricsOrigin = OsbOrigin.Centre;

        [Configurable]
        public string FontName = "Verdana";

        [Configurable]
        public string OutputPath = "sb/lyrics/kala";

        [Configurable]
        public string CirclePath = "sb/circle.png";

        [Configurable]
        public string StripePath = "sb/stripe.png";

        [Configurable]
        public string LeftEdgePath = "sb/leftEdge.png";

        [Configurable]
        public string RightEdgePath = "sb/rightEdge.png";

        [Configurable]
        public float LetterSpacing = 4f;

        [Configurable]
        public float LyricsFade = 0.8f;

        [Configurable]
        public float LinesFade = 0.7f;

        [Configurable]
        public float CirclesMinFade = 0.1f;

        [Configurable]
        public float CirclesMaxFade = 0.5f;

        [Configurable]
        public bool ShowCircles = true;

        [Configurable]
        public bool ShowLines = true;

        [Configurable]
        public float MinCircleAmount = 3;

        [Configurable]
        public float MaxCircleAmount = 6;

        [Configurable]
        public float CircleMinScale = 0.5f;

        [Configurable]
        public float CircleMaxScale = 1.5f;

        [Configurable]
        public bool RandomRotate = true;

        [Configurable]
        public double CircleMinRotation = 0f;

        [Configurable]
        public double CircleMaxRotation = 1f;

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
        public Color4 GlowColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public bool AdditiveGlow = true;

        [Configurable]
        public int OutlineThickness = 0;

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public int ShadowThickness = 4;

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 200);

        [Configurable]
        public Vector2 Padding = Vector2.Zero;

        [Configurable]
        public bool TrimTransparency = true;

        [Configurable]
        public bool EffectsOnly = false;

        [Configurable]
        public bool Debug = false;

        [Configurable]
        public bool Additive = false;

        [Configurable]
        public Color4 LinesColor = Color4.Cyan;

        [Configurable]
        public Color4 CirclesColor = Color4.White;

        [Configurable]
        public bool RandomLyricsColor = false;

        [Configurable]
        public Color4 MinLyricsColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 MaxLyricsColor = new Color4(100, 100, 100, 255);

        public override void Generate()
        {
            var font = LoadFont(OutputPath, new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
                EffectsOnly = EffectsOnly,
                Debug = Debug,
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

            // Verse 1
            CreateLyrics(font, "-lyrics", FontName, FontSize, new Vector2(0, 400), 303, 23161);

            // Chorus
            CreateLyrics(font, "-lyrics2", FontName, FontSize, new Vector2(0, 400), 23161, 43161);
        }

        private void CreateLyrics(Vector2 position, int StartTime, int EndTime, float lineWidth, float lineHeight)
        {
            if (ShowCircles)
            {
                var CirclesLayer = GetLayer("Circles");
                for (var i = 0; i < lineWidth * Random(MinCircleAmount, MaxCircleAmount) / 280; i++) //Generate circles
                {
                    var RandomFade = Random(CirclesMinFade, CirclesMaxFade);
                    var CirclePosition = new Vector2(position.X + (float)Random(-(45 * Math.Ceiling(lineWidth / 45) / 2) - (45 / 2), (45 * Math.Ceiling(lineWidth / 45) / 2) + (45 / 2)), position.Y + (lineHeight / 2) + Random(-(45 / 2), (45 / 2)));
                    //var CirclePosition = new Vector2((float)Random(-lineWidth / 2f, lineWidth / 2f), (float)Random(-lineHeight / 2f, lineHeight / 2f)) + position;
                    var circle = CirclesLayer.CreateSprite(CirclePath, OsbOrigin.Centre, CirclePosition);

                    circle.Scale(StartTime, Random(CircleMinScale, CircleMaxScale) / 2f);
                    circle.Fade(StartTime, StartTime + 50 * (Math.Ceiling(lineWidth / 45) + 5), 0, RandomFade);
                    circle.Fade(EndTime, EndTime + 50 * (Math.Ceiling(lineWidth / 45) + 5), RandomFade, 0);
                    circle.Move(StartTime, EndTime + 50 * (Math.Ceiling(lineWidth / 45) + 5), CirclePosition.X, CirclePosition.Y, CirclePosition.X + Random(-16, 16), CirclePosition.Y + Random(-16, 16));
                    circle.Color(StartTime, CirclesColor);

                    if (RandomRotate)
                    {
                        var angle = Random(CircleMinRotation, CircleMaxRotation);
                        circle.Rotate(EndTime, MathHelper.DegreesToRadians(angle));
                    }
                }
            }

            if (ShowLines)
            {
                var LinesLayer = GetLayer("Lines");
                var LeftEdgePosition = new Vector2(position.X - (45 * (float)Math.Ceiling(lineWidth / 45) / 2), position.Y + (lineHeight / 2.5f) + 7);
                var RightEdgePosition = new Vector2(position.X - (45 * (float)Math.Ceiling(lineWidth / 45) / 2) + (45 / 2) + 45 * (float)Math.Ceiling(lineWidth / 45) - (45 / 2), position.Y + (lineHeight / 2) - 7);
                for (int i = 0; i < Math.Ceiling(lineWidth / 45); i++)
                {
                    var StripePosition = new Vector2((float)position.X - (45 * (float)Math.Ceiling(lineWidth / 45) / 2) + (45 / 2) + 45 * i, position.Y + (lineHeight / 2));

                    var stripes = LinesLayer.CreateSprite(StripePath, OsbOrigin.Centre, StripePosition);
                    stripes.ScaleVec(0, StartTime + 50 * (i + 1), StartTime + 50 * (i + 4), 0, 0.5, 0.5, 0.5);
                    stripes.Fade(0, StartTime + 50 * (i + 1), StartTime + 50 * (i + 4), 0, LinesFade);
                    stripes.ScaleVec(0, EndTime + 50 * (i + 1), EndTime + 50 * (i + 4), 0.5, 0.5, 0, 0.5);
                    stripes.Fade(0, EndTime + 50 * (i + 1), EndTime + 50 * (i + 4), LinesFade, 0);
                    stripes.Color(StartTime, LinesColor);
                }

                var LeftEdge = LinesLayer.CreateSprite(LeftEdgePath, OsbOrigin.BottomCentre, LeftEdgePosition);
                LeftEdge.ScaleVec(0, StartTime, StartTime + 50 * 4, 0, 0.5, 0.5, 0.5);
                LeftEdge.Fade(0, StartTime, StartTime + 50 * 4, 0, LinesFade);
                LeftEdge.ScaleVec(0, EndTime, EndTime + 50 * 4, 0.5, 0.5, 0, 0.5);
                LeftEdge.Fade(0, EndTime, EndTime + 50 * 4, LinesFade, 0);
                LeftEdge.Color(StartTime, LinesColor);

                var RightEdge = LinesLayer.CreateSprite(RightEdgePath, OsbOrigin.TopCentre, RightEdgePosition);
                RightEdge.ScaleVec(0, StartTime + 50 * (Math.Ceiling(lineWidth / 45) + 1), StartTime + 50 * (Math.Ceiling(lineWidth / 45) + 4), 0, 0.5, 0.5, 0.5);
                RightEdge.Fade(0, StartTime + 50 * (Math.Ceiling(lineWidth / 45) + 1), StartTime + 50 * (Math.Ceiling(lineWidth / 45) + 4), 0, LinesFade);
                RightEdge.ScaleVec(0, EndTime + 50 * (Math.Ceiling(lineWidth / 45) + 1), EndTime + 50 * (Math.Ceiling(lineWidth / 45) + 4), 0.5, 0.5, 0, 0.5);
                RightEdge.Fade(0, EndTime + 50 * (Math.Ceiling(lineWidth / 45) + 1), EndTime + 50 * (Math.Ceiling(lineWidth / 45) + 4), LinesFade, 0);
                RightEdge.Color(StartTime, LinesColor);
            }
        }


        private void CreateLyrics(FontGenerator font, string Sentence, string FontName, int FontSize, Vector2 position, int StartTime, int EndTime)
        {
            var LyricsLayer = GetLayer("Lyrics");
            var letterY = position.Y;
            var lineWidth = 0f;
            var lineHeight = 0f;
            var letterSpacing = LetterSpacing * FontScale;
            foreach (var letter in Sentence)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * FontScale + letterSpacing;
                lineHeight = Math.Max(lineHeight, texture.BaseHeight * FontScale);
            }

            CreateLyrics(position, StartTime, EndTime, lineWidth, lineHeight);
            var letterX = position.X - lineWidth * 0.5f;
            var timePerLetter = 30;
            var i = 0;
            foreach (var letter in Sentence)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var letterPos = new Vector2(letterX, letterY)
                        + texture.OffsetFor(LyricsOrigin) * FontScale;

                    var sprite = LyricsLayer.CreateSprite(texture.Path, LyricsOrigin, letterPos);
                    sprite.Scale(StartTime, FontScale);
                    sprite.Fade(StartTime + timePerLetter * i - 200, StartTime + timePerLetter * i, 0, LyricsFade);
                    sprite.Fade(EndTime - 200 + timePerLetter * i, EndTime + timePerLetter * i, LyricsFade, 0);

                    if (Additive)
                    {
                        sprite.Additive(StartTime, EndTime);
                    }

                    var RealColor1 = RandomLyricsColor ? new Color4((float)Random(MinLyricsColor.R, MaxLyricsColor.R),
                                                                (float)Random(MinLyricsColor.G, MaxLyricsColor.G),
                                                                (float)Random(MinLyricsColor.B, MaxLyricsColor.B), 255) : MinLyricsColor;
                    var RealColor2 = RandomLyricsColor ? new Color4((float)Random(MinLyricsColor.R, MaxLyricsColor.R),
                                                                (float)Random(MinLyricsColor.G, MaxLyricsColor.G),
                                                                (float)Random(MinLyricsColor.B, MaxLyricsColor.B), 255) : MaxLyricsColor;

                    sprite.Color(StartTime, EndTime, RealColor1, RealColor2);
                    i++;
                }
                letterX += texture.BaseWidth * FontScale + letterSpacing;
            }
            letterY += lineHeight;

            //var position = new Vector2(SubtitleX - texture.BaseWidth * FontScale * 0.5f, SubtitleY)
            //    + texture.OffsetFor(Origin) * FontScale;
        }
    }
}
