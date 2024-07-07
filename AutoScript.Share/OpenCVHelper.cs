using OpenCvSharp;
using OpenCvSharp.Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;

namespace AutoScript.Share
{
    public class OpenCVHelper
    {
        // 计算SSIM指数，范围[0,1]，值越大代表两张图片越相似
        public static double CalculateImageSimilarity(string imageFile1, string imageFile2)
        {
            Mat image1 = Cv2.ImRead(imageFile1, ImreadModes.AnyColor);
            Mat image2 = Cv2.ImRead(imageFile2, ImreadModes.AnyColor);
            // 四个通道，前三通道是红绿蓝计算出来的值，范围[0,1],越高越相似，第四个值为0，不做计算
            var ssim = CalculateSSIM(image1, image2);
            var averageValue = (ssim[0] + ssim[1] + ssim[2]) / 3;
            return averageValue;
        }
        public static List<System.Drawing.Point> FindImage(Bitmap bmp, ImageInfo info)
        {
     
            return FindImage(bmp.ToMat(), info.Pic.ToMat());
        }
        /// <summary>
        /// 模板匹配
        /// </summary>
        /// <param name="src">大图</param>
        /// <param name="sub">小图</param>
        /// <param name="sim">相似度</param>
        /// <returns>返回大图中匹配的坐标</returns>
        public static List<System.Drawing.Point> FindImage(Mat src, Mat sub, double sim = 0.940)
        {
            Mat result = new Mat();
            List<System.Drawing.Point> ret = new();
            try
            {
                int result_cols = src.Cols - sub.Cols + 1;
                int result_rows = src.Rows - sub.Rows + 1;
                result.Create(result_cols, result_rows, MatType.CV_32FC1);

                //CCorrNormed比较 越接近1越相似
                Cv2.MatchTemplate(src, sub, result, TemplateMatchModes.CCorrNormed);
                //归一化计算
                //Cv2.Normalize(result, result, 0, 1, NormTypes.MinMax, -1, new Mat());
                double minVal = -1, maxVal;
                Point minLoc, maxLoc;
                Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc, new Mat());
                if (maxVal > sim) ret.Add(new System.Drawing.Point(maxLoc.X, maxLoc.Y));    //高于相似度.返回坐标
            }
            catch (Exception) { }
            return ret;
        }

        /// <summary>
        /// SSIM (structual similarity, SSIM) 结构相似性,用来判断图片相似度
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static Scalar CalculateSSIM(Mat pic1, Mat pic2)
        {
            const double C1 = 6.5025, C2 = 58.5225;
  
            /***************************** INITS **********************************/
            MatType d = MatType.CV_32F;

            Mat I1 = new Mat(), I2 = new Mat();
            pic1.ConvertTo(I1, d);           // cannot calculate on one byte large values
            pic2.ConvertTo(I2, d);

            Mat I2_2 = I2.Mul(I2);        // I2^2
            Mat I1_2 = I1.Mul(I1);        // I1^2
            Mat I1_I2 = I1.Mul(I2);        // I1 * I2

            /***********************PRELIMINARY COMPUTING ******************************/

            Mat mu1 = new Mat(), mu2 = new Mat();   //
            Cv2.GaussianBlur(I1, mu1, new OpenCvSharp.Size(11, 11), 1.5);
            Cv2.GaussianBlur(I2, mu2, new OpenCvSharp.Size(11, 11), 1.5);

            Mat mu1_2 = mu1.Mul(mu1);
            Mat mu2_2 = mu2.Mul(mu2);
            Mat mu1_mu2 = mu1.Mul(mu2);

            Mat sigma1_2 = new Mat(), sigma2_2 = new Mat(), sigma12 = new Mat();

            Cv2.GaussianBlur(I1_2, sigma1_2, new OpenCvSharp.Size(11, 11), 1.5);
            sigma1_2 -= mu1_2;

            Cv2.GaussianBlur(I2_2, sigma2_2, new OpenCvSharp.Size(11, 11), 1.5);
            sigma2_2 -= mu2_2;

            Cv2.GaussianBlur(I1_I2, sigma12, new OpenCvSharp.Size(11, 11), 1.5);
            sigma12 -= mu1_mu2;

            ///////////////////////////////// FORMULA ////////////////////////////////
            Mat t1, t2, t3;

            Scalar C1Mat = new Scalar(C1);
            Scalar C2Mat = new Scalar(C2);

            t1 = 2 * mu1_mu2 + C1Mat;
            t2 = 2 * sigma12 + C2Mat;
            t3 = t1.Mul(t2);              // t3 = ((2*mu1_mu2 + C1).*(2*sigma12 + C2))

            t1 = mu1_2 + mu2_2 + C1Mat;
            t2 = sigma1_2 + sigma2_2 + C2Mat;
            t1 = t1.Mul(t2);               // t1 =((mu1_2 + mu2_2 + C1).*(sigma1_2 + sigma2_2 + C2))

            Mat ssim_map = new Mat();
            Cv2.Divide(t3, t1, ssim_map);      // ssim_map =  t3./t1;
            Scalar mssim = Cv2.Mean(ssim_map);// mssim = average of ssim map
            return mssim;
        }
    }

    public static class BitmapExtension
    {
        public static Mat ToMat(this Bitmap bitmap)
        {
            return BitmapConverter.ToMat(bitmap);
        }
        public static byte[] ToBytes(this Bitmap bmp)//从bitmap获取Byte[]
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Bmp);
            byte[] buf = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return buf;
        }
        /// <summary>
        /// 获取一幅图像的一部分
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static Bitmap GetSub(this Bitmap bmp, int x1, int y1, int x2, int y2)//从bitmap获取Byte[]
        {
            Rectangle cropRect = new Rectangle(x1, y1, x2, y2);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, target.Width, target.Height),
                      cropRect,
                      GraphicsUnit.Pixel);
            }
            return target;
        }
    }
}
