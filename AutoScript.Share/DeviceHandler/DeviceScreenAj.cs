using AoJiaLib;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace AutoScript.Share
{
    public class DeviceScreenAj : IDeviceScreen
    {
        private IDevice device;
        private AoJiaD aj;
        public DeviceScreenAj(IDevice device, AoJiaD aj)
        {
            this.device = device;
            this.aj = aj;
            
        }
        public ImageInfo 找屏(ImageInfo info)
        {
            object x, y,pic;
            if (info.PicName.Length > 0)
            {
                aj.FindPic(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2, info.PicName + ".bmp",
                info.Offset_color, info.Sim, info.Dir, 0, out pic, out x, out y);
                info.Result = (Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(x) > 0);
                return info;

          
            }
            else if (info.Offset_color != "000000")
            {
                //没有图片名字的时候,是找色
                aj.FindMultiColor(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2, info.Color,
                    info.Offset_color, info.Sim, info.Dir, info.Sim, info.Sim, out x, out y);
                info.Result = (Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(x) > 0);
                return info;
            }
            else
            {
                return OCR(info);
            }
           
           
        }





        public unsafe ImageInfo OCR(ImageInfo info)
        {
       
            Bitmap S_bmp = 截屏( info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2);
            string base64 = Utility.ImgToBase64String(S_bmp);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string result = Utility.SendPost("http://127.0.0.1:1224/api/ocr", null, null, "{\"base64\":\"" + base64 + "\"}");
            JObject jobject = JObject.Parse(result);
            if (jobject["code"].ToString() == "101")
            {
                return info;
            }
            OcrResult ocrResult = JsonConvert.DeserializeObject<OcrResult>(result);
            foreach (var item in ocrResult.Data)
            {
                if (item.Score > info.Sim && !string.IsNullOrEmpty(info.ContainText) && item.Text.Contains(info.ContainText))
                {
                    int x = item.Box[0][0] + info.Range.x1;
                    int y = item.Box[0][1] + info.Range.y1;
                    info.Result.x = x;
                    info.Result.y = y;
                    info.Result.isFinded = true;
                    break;
                    //info.OcrString = item.Text;
                }
                else if (item.Score > info.Sim && string.IsNullOrEmpty(info.ContainText))
                {
                    int x = item.Box[0][0] + info.Range.x1;
                    int y = item.Box[0][1] + info.Range.y1;
                    info.Result.x = x;
                    info.Result.y = y;
                    info.Result.isFinded = true;
                    info.OcrString = item.Text;
                    break;

                }
            }
            sw.Stop();
            Trace.WriteLine("OCR耗时:" + sw.ElapsedMilliseconds.ToString() + "毫秒");
            //(string text,Point point) ret=OCRHelper.Detect(S_bmp, info);
            //info.Result.x = ret.point.X;
            //info.Result.y = ret.point.Y;
            //info.Result.isFinded = ret.point.X>=0;
            return info;

        }


        /// <summary>
        /// 窗口区域截图
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public Bitmap 截屏(int x1, int y1, int x2, int y2)
        {
            y1 = y1 + 31; y2 = y2 + 31;//固定减去31 去掉标题栏的31像素
            IntPtr hdcSrc = API.GetWindowDC(this.device.Hwnd);
            API.RECT windowRect = new API.RECT();
            API.GetWindowRect(this.device.Hwnd, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = API.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = API.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = API.SelectObject(hdcDest, hBitmap);
            API.BitBlt(hdcDest, 0, 0, x2 - x1, y2 - y1, hdcSrc, x1, y1, API.SRCCOPY);
            API.SelectObject(hdcDest, hOld);
            API.DeleteDC(hdcDest);
            API.ReleaseDC(this.device.Hwnd, hdcSrc);
            Bitmap bmp = Image.FromHbitmap(hBitmap).Clone(new Rectangle(0, 0, x2 - x1, y2 - y1), PixelFormat.Format24bppRgb);
            API.DeleteObject(hBitmap);
            GC.Collect();
            return bmp;
        }
    }
}