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
        public string OutputPath = "sb/lyrics";

        [Configurable]
        public string CirclePath = "sb/circle.png";

        [Configurable]
        public string StripePath = "sb/stripe.png";

        [Configurable]
        public string LeftEdgePath = "sb/leftEdge.png";

        [Configurable]
        public string RightEdgePath = "sb/rightEdge.png";

        [Configurable]
        public float LinesFade = 0.7f;

        [Configurable]
        public float LyricsFade = 1;

        [Configurable]
        public float CirclesMinFade = 0.1f;

        [Configurable]
        public float CirclesMaxFade = 0.6f;

        [Configurable]
        public bool ShowCircles = true;

        [Configurable]
        public bool ShowLines = true;

        [Configurable]
        public float CircleAmount = 20;

        [Configurable]
        public float CircleMinScale = 0.5f;

        [Configurable]
        public float CircleMaxScale = 1.5f;

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
        public int OutlineThickness = 3;

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public int ShadowThickness = 0;

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 100);

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
        public Color4 LyricsColor = Color4.White;

        [Configurable]
        public Color4 LinesColor = Color4.Cyan;

        [Configurable]
        public Color4 CirclesColor = Color4.White;

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

            CreateLyrics(font, "これはとてもクールです", FontName, FontSize, 320, 400, 0, 6711);
            CreateLyrics(font, "Testing the second lyrics here", FontName, FontSize, 320, 400, 6711, 13304);
        }

        private void CreateLyrics(FontGenerator font, string Sentence, string FontName, int FontSize, float SubtitleX, float SubtitleY, int StartTime, int EndTime)
        {
            var CirclesLayer = GetLayer("Circles");
            var LinesLayer = GetLayer("Lines");
            var LyricsLayer = GetLayer("Lyrics");
            var texture = font.GetTexture(Sentence.ToString());
            var position = new Vector2(SubtitleX - texture.BaseWidth * FontScale * 0.5f, SubtitleY)
                + texture.OffsetFor(LyricsOrigin) * FontScale;
            var initHeight = (int)texture.BaseHeight * 0.5f;

            //Create the stripes and the circle thing... whatever that is :eyes:
            var lineWidth = texture.BaseWidth * FontScale;
            var lineHeight = texture.BaseHeight * FontScale;

            if (ShowCircles)
            {
                for (var i = 0; i < lineWidth / CircleAmount; i++) //Generate circles
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
                }
            }

            if (ShowLines)
            {
                var LeftEdgePosition = new Vector2(position.X - (45 * (float)Math.Ceiling(lineWidth / 45) / 2), SubtitleY + (initHeight / 2) + 7);
                var RightEdgePosition = new Vector2(position.X - (45 * (float)Math.Ceiling(lineWidth / 45) / 2) + (45 / 2) + 45 * (float)Math.Ceiling(lineWidth / 45) - (45 / 2), SubtitleY + (initHeight / 2) - 7);
                for (int i = 0; i < Math.Ceiling(lineWidth / 45); i++)
                {
                    double subtitleX = position.X + Random(-(45 * Math.Ceiling(lineWidth / 45) / 2) - (45 / 2), (45 * Math.Ceiling(lineWidth / 45) / 2) + (45 / 2));
                    var StripePosition = new Vector2((float)SubtitleX - (45 * (float)Math.Ceiling(lineWidth / 45) / 2) + (45 / 2) + 45 * i, SubtitleY + (initHeight / 2));

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

            //Create the text

            var sprite = LyricsLayer.CreateSprite(texture.Path, LyricsOrigin, position);
            sprite.Scale(StartTime, FontScale);
            sprite.Fade(StartTime - 200, StartTime, 0, LyricsFade);
            sprite.Fade(EndTime - 200, EndTime, LyricsFade, 0);
            sprite.Color(StartTime, LyricsColor);

            if (Additive)
            {
                sprite.Additive(StartTime, EndTime);
            }
        }
    }
}
