using SkiaSharp;
using System;
using System.Collections.Generic;

namespace Mi.Common {

    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public class CaptchaHelper {
        /// <summary>
        /// 干扰线的颜色集合
        /// </summary>
        private List<SKColor> colors { get; set; }
        public CaptchaHelper() {
            colors = new List<SKColor>();
            colors.Add(SKColor.Parse("#AAAAAA"));
            colors.Add(SKColor.Parse("#888888"));
            colors.Add(SKColor.Parse("#666666"));
            colors.Add(SKColor.Parse("#444444"));
            colors.Add(SKColor.Parse("#000000"));
            colors.Add(SKColor.Parse("#FFB7DD"));
            colors.Add(SKColor.Parse("#FF88C2"));
            colors.Add(SKColor.Parse("#FF44AA"));
            colors.Add(SKColor.Parse("#FF0088"));
            colors.Add(SKColor.Parse("#C10066"));
            colors.Add(SKColor.Parse("#A20055"));
            colors.Add(SKColor.Parse("#8C0044"));
            colors.Add(SKColor.Parse("#FFCCCC"));
            colors.Add(SKColor.Parse("#FF8888"));
            colors.Add(SKColor.Parse("#FF3333"));
            colors.Add(SKColor.Parse("#FF0000"));
            colors.Add(SKColor.Parse("#CC0000"));
            colors.Add(SKColor.Parse("#AA0000"));
            colors.Add(SKColor.Parse("#880000"));
            colors.Add(SKColor.Parse("#FFC8B4"));
            colors.Add(SKColor.Parse("#FFA488"));
            colors.Add(SKColor.Parse("#FF7744"));
            colors.Add(SKColor.Parse("#FF5511"));
            colors.Add(SKColor.Parse("#E63F00"));
            colors.Add(SKColor.Parse("#C63300"));
            colors.Add(SKColor.Parse("#A42D00"));
            colors.Add(SKColor.Parse("#FFDDAA"));
            colors.Add(SKColor.Parse("#FFBB66"));
            colors.Add(SKColor.Parse("#FFAA33"));
            colors.Add(SKColor.Parse("#FF8800"));
            colors.Add(SKColor.Parse("#EE7700"));
            colors.Add(SKColor.Parse("#CC6600"));
            colors.Add(SKColor.Parse("#BB5500"));
            colors.Add(SKColor.Parse("#FFBB00"));
            colors.Add(SKColor.Parse("#DDAA00"));
            colors.Add(SKColor.Parse("#AA7700"));
            colors.Add(SKColor.Parse("#886600"));
            colors.Add(SKColor.Parse("#EEEE00"));
            colors.Add(SKColor.Parse("#BBBB00"));
            colors.Add(SKColor.Parse("#888800"));
            colors.Add(SKColor.Parse("#BBFF00"));
            colors.Add(SKColor.Parse("#99DD00"));
            colors.Add(SKColor.Parse("#88AA00"));
            colors.Add(SKColor.Parse("#668800"));
            colors.Add(SKColor.Parse("#CCFF99"));
            colors.Add(SKColor.Parse("#BBFF66"));
            colors.Add(SKColor.Parse("#99FF33"));
            colors.Add(SKColor.Parse("#77FF00"));
            colors.Add(SKColor.Parse("#66DD00"));
            colors.Add(SKColor.Parse("#55AA00"));
            colors.Add(SKColor.Parse("#227700"));
            colors.Add(SKColor.Parse("#99FF99"));
            colors.Add(SKColor.Parse("#66FF66"));
            colors.Add(SKColor.Parse("#33FF33"));
            colors.Add(SKColor.Parse("#00FF00"));
            colors.Add(SKColor.Parse("#00DD00"));
            colors.Add(SKColor.Parse("#00AA00"));
            colors.Add(SKColor.Parse("#008800"));
            colors.Add(SKColor.Parse("#BBFFEE"));
            colors.Add(SKColor.Parse("#77FFCC"));
            colors.Add(SKColor.Parse("#33FFAA"));
            colors.Add(SKColor.Parse("#00FF99"));
            colors.Add(SKColor.Parse("#00DD77"));
            colors.Add(SKColor.Parse("#00AA55"));
            colors.Add(SKColor.Parse("#008844"));
            colors.Add(SKColor.Parse("#AAFFEE"));
            colors.Add(SKColor.Parse("#77FFEE"));
            colors.Add(SKColor.Parse("#33FFDD"));
            colors.Add(SKColor.Parse("#00FFCC"));
            colors.Add(SKColor.Parse("#00DDAA"));
            colors.Add(SKColor.Parse("#00AA88"));
            colors.Add(SKColor.Parse("#008866"));
            colors.Add(SKColor.Parse("#99FFFF"));
            colors.Add(SKColor.Parse("#66FFFF"));
            colors.Add(SKColor.Parse("#33FFFF"));
            colors.Add(SKColor.Parse("#00FFFF"));
            colors.Add(SKColor.Parse("#00DDDD"));
            colors.Add(SKColor.Parse("#00AAAA"));
            colors.Add(SKColor.Parse("#008888"));
            colors.Add(SKColor.Parse("#CCEEFF"));
            colors.Add(SKColor.Parse("#77DDFF"));
            colors.Add(SKColor.Parse("#33CCFF"));
            colors.Add(SKColor.Parse("#00BBFF"));
            colors.Add(SKColor.Parse("#009FCC"));
            colors.Add(SKColor.Parse("#0088A8"));
            colors.Add(SKColor.Parse("#007799"));
            colors.Add(SKColor.Parse("#CCDDFF"));
            colors.Add(SKColor.Parse("#99BBFF"));
            colors.Add(SKColor.Parse("#5599FF"));
            colors.Add(SKColor.Parse("#0066FF"));
            colors.Add(SKColor.Parse("#0044BB"));
            colors.Add(SKColor.Parse("#003C9D"));
            colors.Add(SKColor.Parse("#003377"));
            colors.Add(SKColor.Parse("#CCCCFF"));
            colors.Add(SKColor.Parse("#9999FF"));
            colors.Add(SKColor.Parse("#5555FF"));
            colors.Add(SKColor.Parse("#0000FF"));
            colors.Add(SKColor.Parse("#0000CC"));
            colors.Add(SKColor.Parse("#0000AA"));
            colors.Add(SKColor.Parse("#000088"));
            colors.Add(SKColor.Parse("#CCBBFF"));
            colors.Add(SKColor.Parse("#9F88FF"));
            colors.Add(SKColor.Parse("#7744FF"));
            colors.Add(SKColor.Parse("#5500FF"));
            colors.Add(SKColor.Parse("#4400CC"));
            colors.Add(SKColor.Parse("#2200AA"));
            colors.Add(SKColor.Parse("#220088"));
            colors.Add(SKColor.Parse("#D1BBFF"));
            colors.Add(SKColor.Parse("#B088FF"));
            colors.Add(SKColor.Parse("#9955FF"));
            colors.Add(SKColor.Parse("#7700FF"));
            colors.Add(SKColor.Parse("#5500DD"));
            colors.Add(SKColor.Parse("#4400B3"));
            colors.Add(SKColor.Parse("#3A0088"));
            colors.Add(SKColor.Parse("#E8CCFF"));
            colors.Add(SKColor.Parse("#D28EFF"));
            colors.Add(SKColor.Parse("#B94FFF"));
            colors.Add(SKColor.Parse("#9900FF"));
            colors.Add(SKColor.Parse("#7700BB"));
            colors.Add(SKColor.Parse("#66009D"));
            colors.Add(SKColor.Parse("#550088"));
            colors.Add(SKColor.Parse("#F0BBFF"));
            colors.Add(SKColor.Parse("#E38EFF"));
            colors.Add(SKColor.Parse("#E93EFF"));
            colors.Add(SKColor.Parse("#CC00FF"));
            colors.Add(SKColor.Parse("#A500CC"));
            colors.Add(SKColor.Parse("#7A0099"));
            colors.Add(SKColor.Parse("#660077"));
            colors.Add(SKColor.Parse("#FFB3FF"));
            colors.Add(SKColor.Parse("#FF77FF"));
            colors.Add(SKColor.Parse("#FF3EFF"));
            colors.Add(SKColor.Parse("#FF00FF"));
            colors.Add(SKColor.Parse("#CC00CC"));
            colors.Add(SKColor.Parse("#990099"));
            colors.Add(SKColor.Parse("#770077"));
            colors.Add(SKColor.Parse("#DDDDDD"));
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="captchaText">验证码文字</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="lineNum">干扰线数量</param>
        /// <param name="lineStrookeWidth">干扰线宽度</param>
        /// <returns></returns>
        public byte[] GetCaptcha(string captchaText, int width = 120, int height = 37) {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int randAngle = 40;

            //创建bitmap位图
            using (SKBitmap bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul)) {
                //创建画笔
                using (SKCanvas canvas = new SKCanvas(bitmap)) {
                    canvas.Clear(SKColors.Transparent);

                    var paint = new SKPaint() { Color = colors[rand.Next(colors.Count)], };
                    for (int i = 0; i < 50; i++) {
                        int x = rand.Next(0, bitmap.Width);
                        int y = rand.Next(0, bitmap.Height);

                        canvas.DrawRect(new SKRect(x, y, x + 1, y + 1), paint);
                    }

                    canvas.Translate(-4, 0);

                    char[] chars = captchaText.ToCharArray();
                    string[] fonts = { "Arial", "Arvo", "Bahnschrift", "Baskerville Old face", "Berlin Sans FB", "Bookman Old style", "Cascadia Code", "Century", "Century Gothic", "Comic Sans MS", "Eras Demi ITC", "Gill Sans MT" };
                    for (int i = 0; i < chars.Length; i++) {
                        var fontColor = colors[rand.Next(colors.Count)];
                        var foneSize = rand.Next(22, 28);
                        float angle = rand.Next(-randAngle, randAngle);

                        SKPoint point = new SKPoint(width / captchaText.Length - 2, height / 2 + 8);

                        canvas.Translate(point);
                        canvas.RotateDegrees(angle);

                        SKTypeface font = SKTypeface.FromFamilyName(fonts[rand.Next(fonts.Length)], SKFontStyleWeight.Normal, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
                        var textPaint = new SKPaint() {
                            TextAlign = SKTextAlign.Center,
                            Color = fontColor,
                            TextSize = foneSize,
                            IsAntialias = true,
                            Typeface = font,
                        };

                        canvas.DrawText(chars[i].ToString(), new SKPoint(0, 0), textPaint);
                        canvas.RotateDegrees(-angle);
                        canvas.Translate(0, -point.Y);
                    }

                    canvas.Translate(-width, 0);

                    //画随机干扰线
                    using (SKPaint drawStyle = new SKPaint()) {
                        for (int i = 0; i < 4; i++) {
                            drawStyle.Color = colors[rand.Next(colors.Count)];
                            drawStyle.StrokeWidth = 1;
                            canvas.DrawLine(rand.Next(0, width), rand.Next(0, height), rand.Next(0, width), rand.Next(0, height), drawStyle);
                        }
                    }

                    using (var image = SKImage.FromBitmap(bitmap)) {
                        using (SKData p = image.Encode(SKEncodedImageFormat.Png, 100)) {
                            return p.ToArray();
                        }
                    }
                }
            }
        }
    }
}
