using OpenCvSharp;
using Sdcb.PaddleOCR.Models.Local;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace AutoScript.Share
{
    public class OCRHelper
    {
        private static FullOcrModel model = LocalFullModels.ChineseV3;
        private static PaddleOcrAll all;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp">窗口截图</param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static (string text, Point point) Detect(Bitmap bmp, ImageInfo info)
        {
            all ??= new PaddleOcrAll(model)
            {
                AllowRotateDetection = true, /* 允许识别有角度的文字 */
                Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
            };
            var sub = bmp.GetSub(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2);
            byte[] sampleImageData = sub.ToBytes();
            using (Mat src = Cv2.ImDecode(sampleImageData, ImreadModes.Color))
            {
                var result = all.Run(src);  //OCR识别
                var lst = result.Regions.Where(r => r.Score > info.Sim)
                    .Select(r => (r.Text, r.Rect)).ToList();
                if (lst.Count > 0)
                {
                    Point p = new Point(((int)lst.First().Rect.Center.X), ((int)lst.First().Rect.Center.Y));
                    return (String.Join("", lst.Select(r => r.Text)), p);
                }
                else return ("", new Point(-1, -1));
            }
        }
    }
}
