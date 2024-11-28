using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public class DeviceScreenDm : IDeviceScreen
    {
        private IDevice device;
        private DmSoftClass dm;
        public DeviceScreenDm(IDevice device, DmSoftClass dm)
        {
            this.device = device;
            this.dm = dm;
        }
        public ImageInfo 找屏(ImageInfo info)
        {
            int x, y;
            if (info.PicName.Length > 0)
            {
                dm.FindPic(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2, info.PicName + ".bmp",
                info.Offset_color, info.Sim, info.Dir, out x, out y);
                info.Result = (Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(x) > 0);
                return info;

            }
            else if (info.Offset_color != "000000")
            {
                //没有图片名字的时候,是找色
                dm.FindMultiColor(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2, info.Color,
                    info.Offset_color, info.Sim, info.Dir, out x, out y);
                info.Result = (Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(x) > 0);
                return info;
            }
            else
            {

                //return new GDIScreenFinder(this.device.Hwnd).OCR(info);
                return this.OCR(info);
            }

        
        }


        public unsafe ImageInfo OCR(ImageInfo info)
        {
            this.dm.Capture(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2,"1.png");
            //Bitmap S_bmp = 截屏(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2);
            // 图片文件路径
            string imagePath = @"Resource/pic/1.png";

            // 读取图片文件为字节数组
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            Console.WriteLine("32323");
            //Bitmap S_bmp = null;
            //string base64 = Utility.ImgToBase64String(S_bmp);
            string base64 = Convert.ToBase64String(imageBytes);

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
    }
}