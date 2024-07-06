using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public class GDIScreenFinder : IDeviceScreen
    {
        public IntPtr Hwnd { get; set; }
        public string PicPath { get; set; }
        public GDIScreenFinder(IntPtr hwnd)
        {
            PicPath = Config.AppConfig.Config_DM.Path;
            Hwnd = hwnd;
        }
        /// <summary>
        /// 窗口截图
        /// </summary>
        /// <returns></returns>
        public Bitmap 截屏()
        {
            IntPtr hdcSrc = API.GetWindowDC(Hwnd);
            // get the size
            API.RECT windowRect = new API.RECT();
            API.GetWindowRect(Hwnd, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = API.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = API.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = API.SelectObject(hdcDest, hBitmap);
            API.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, API.SRCCOPY);
            API.SelectObject(hdcDest, hOld);
            API.DeleteDC(hdcDest);
            API.ReleaseDC(Hwnd, hdcSrc);
            Bitmap img = Image.FromHbitmap(hBitmap);
            API.DeleteObject(hBitmap);
            GC.Collect();
            return img;
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
            IntPtr hdcSrc = API.GetWindowDC(Hwnd);
            API.RECT windowRect = new API.RECT();
            API.GetWindowRect(Hwnd, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = API.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = API.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = API.SelectObject(hdcDest, hBitmap);
            API.BitBlt(hdcDest, 0, 0, x2 - x1, y2 - y1, hdcSrc, x1, y1, API.SRCCOPY);
            API.SelectObject(hdcDest, hOld);
            API.DeleteDC(hdcDest);
            API.ReleaseDC(Hwnd, hdcSrc);
            Bitmap bmp = Image.FromHbitmap(hBitmap).Clone(new Rectangle(0, 0, x2 - x1, y2 - y1), PixelFormat.Format24bppRgb);
            API.DeleteObject(hBitmap);
            GC.Collect();
            return bmp;
        }

        public ImageInfo FindPic(ImageInfo imageInfo)
        {
            Bitmap bigBMP = 截屏(imageInfo.Range.x1, imageInfo.Range.y1, imageInfo.Range.x2, imageInfo.Range.y2);
            //Bitmap smallBmp = new Bitmap("D:\\gamescript\\CSharpScript\\AutoScript.Server\\bin\\Debug\\net8.0-windows\\Resource\\pic\\" + imageInfo.PicName + ".bmp");
            Bitmap smallBmp = new Bitmap(AppContext.BaseDirectory + this.PicPath + "\\" + imageInfo.PicName + ".bmp");

            // 获取应用程序所在目录（绝对，不受工作目录影响）

            // 也可以获取应用程序所在目录
            //string ss=AppContext.BaseDirectory;
   
    
            int similar = (int)((1 - imageInfo.Sim) * 255);
            List<Point> ret = FindPic(imageInfo.Range.x1, imageInfo.Range.y1, imageInfo.Range.x2 - imageInfo.Range.x1, imageInfo.Range.y2 - imageInfo.Range.y1, bigBMP, smallBmp, similar);
            if (ret.Count > 0)
            {
                imageInfo.Result = (ret[0].X, ret[0].Y-30, true);
            }
            else
            {
                imageInfo.Result = (-1, -1, false);
            }
            return imageInfo;
        }
        /// <summary>
        /// 相似找图
        /// </summary>
        /// <param name="S_Data">大图数据</param>
        /// <param name="P_Data">小图数据</param>
        /// <param name="similar">误差值</param>
        /// <returns></returns>
        private unsafe List<Point> _FindMultiColor(int left, int top, int width, int height, BitmapData S_Data, BitmapData P_Data, int similar)
        {
            List<Point> List = new List<Point>();
            int S_stride = S_Data.Stride;
            int P_stride = P_Data.Stride;
            IntPtr S_Iptr = S_Data.Scan0;
            IntPtr P_Iptr = P_Data.Scan0;
            byte* S_ptr;
            byte* P_ptr;
            bool IsOk = false;
            int _BreakW = width - P_Data.Width + 1;
            int _BreakH = height - P_Data.Height + 1;
            for (int h = top; h < _BreakH; h++)
            {
                for (int w = left; w < _BreakW; w++)
                {
                    P_ptr = (byte*)(P_Iptr);
                    for (int y = 0; y < P_Data.Height; y++)
                    {
                        for (int x = 0; x < P_Data.Width; x++)
                        {
                            S_ptr = (byte*)((int)S_Iptr + S_stride * (h + y) + (w + x) * 3);
                            P_ptr = (byte*)((int)P_Iptr + P_stride * y + x * 3);
                            if (ScanColor(S_ptr[0], S_ptr[1], S_ptr[2], P_ptr[0], P_ptr[1], P_ptr[2], similar))  //比较颜色
                            {
                                IsOk = true;
                            }
                            else
                            {
                                IsOk = false; break;
                            }
                        }
                        if (IsOk == false) { break; }
                    }
                    if (IsOk) { List.Add(new Point(w, h)); }
                    IsOk = false;
                }
            }
            return List;
        }
        /// <summary>
        /// 在大图里找小图
        /// </summary>
        /// <param name="S_bmp">大图</param>
        /// <param name="P_bmp">小图</param>
        /// <param name="similar">容错值 取值0--255，数值越高效率越低，不建议超过50</param>
        /// <returns></returns>
        private List<Point> FindPic(int left, int top, int width, int height, Bitmap S_bmp, Bitmap P_bmp, int similar)
        {
            if (P_bmp.PixelFormat != PixelFormat.Format24bppRgb) { throw new Exception("颜色格式只支持24位bmp"); }
            if (S_bmp.PixelFormat != PixelFormat.Format24bppRgb) { throw new Exception("颜色格式只支持24位bmp"); }
            int S_Width = S_bmp.Width;
            int S_Height = S_bmp.Height;
            int P_Width = P_bmp.Width;
            int P_Height = P_bmp.Height;
            //取出4个角的颜色
            int px1 = P_bmp.GetPixel(0, 0).ToArgb(); //左上角
            int px2 = P_bmp.GetPixel(P_Width - 1, 0).ToArgb(); //右上角
            int px3 = P_bmp.GetPixel(0, P_Height - 1).ToArgb(); //左下角
            int px4 = P_bmp.GetPixel(P_Width - 1, P_Height - 1).ToArgb(); //右下角
            Color BackColor = P_bmp.GetPixel(0, 0); //背景色
            BitmapData S_Data = S_bmp.LockBits(new Rectangle(0, 0, S_Width, S_Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData P_Data = P_bmp.LockBits(new Rectangle(0, 0, P_Width, P_Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            List<Point> List;
            if (px1 == px2 && px1 == px3 && px1 == px4) //如果4个角的颜色相同
            {
                //透明找图
                List = _FindPic(left, top, width, height, S_Data, P_Data, GetPixelData(P_Data, BackColor), similar);
            }
            else if (similar > 0)
            {
                //相似找图
                List = _FindPic(left, top, width, height, S_Data, P_Data, similar);
            }
            else
            {
                //全匹配找图效率最高
                List = _FindPic(left, top, width, height, S_Data, P_Data);
            }
            S_bmp.UnlockBits(S_Data);
            P_bmp.UnlockBits(P_Data);
            return List;
        }
        private static unsafe int[,] GetPixelData(BitmapData P_Data, Color BackColor)
        {
            byte B = BackColor.B, G = BackColor.G, R = BackColor.R;
            int Width = P_Data.Width, Height = P_Data.Height;
            int P_stride = P_Data.Stride;
            IntPtr P_Iptr = P_Data.Scan0;
            byte* P_ptr;
            int[,] PixelData = new int[Width * Height, 2];
            int i = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    P_ptr = (byte*)((int)P_Iptr + P_stride * y + x * 3);
                    if (B == P_ptr[0] & G == P_ptr[1] & R == P_ptr[2])
                    {

                    }
                    else
                    {
                        PixelData[i, 0] = x;
                        PixelData[i, 1] = y;
                        i++;
                    }
                }
            }
            int[,] PixelData2 = new int[i, 2];
            Array.Copy(PixelData, PixelData2, i * 2);
            return PixelData2;
        }
        /// <summary>
        /// 透明找图
        /// </summary>
        /// <param name="S_Data">大图数据</param>
        /// <param name="P_Data">小图数据</param>
        /// <param name="PixelData">小图中需要比较的像素数据</param>
        /// <param name="similar">误差值</param>
        /// <returns></returns>
        private unsafe List<Point> _FindPic(int left, int top, int width, int height, BitmapData S_Data, BitmapData P_Data, int[,] PixelData, int similar)
        {
            List<Point> List = new List<Point>();
            int Len = PixelData.GetLength(0);
            int S_stride = S_Data.Stride;
            int P_stride = P_Data.Stride;
            IntPtr S_Iptr = S_Data.Scan0;
            IntPtr P_Iptr = P_Data.Scan0;
            byte* S_ptr;
            byte* P_ptr;
            bool IsOk = false;
            int _BreakW = width - P_Data.Width + 1;
            int _BreakH = height - P_Data.Height + 1;
            int cnt = 0;
            for (int h = top; h < _BreakH; h++)
            {
                for (int w = left; w < _BreakW; w++)
                {
                    for (int i = 0; i < Len; i++)
                    {
                        S_ptr = (byte*)((int)S_Iptr + S_stride * (h + PixelData[i, 1]) + (w + PixelData[i, 0]) * 3);
                        P_ptr = (byte*)((int)P_Iptr + P_stride * PixelData[i, 1] + PixelData[i, 0] * 3);
                        if (ScanColor(S_ptr[0], S_ptr[1], S_ptr[2], P_ptr[0], P_ptr[1], P_ptr[2], similar))  //比较颜色
                        {
                            IsOk = true;
                        }
                        else
                        {
                            IsOk = false; break;
                        }
                    }
                    if (IsOk) { List.Add(new Point(w, h)); }
                    IsOk = false;
                }
            }
            return List;
        }
        /// <summary>
        /// 相似找图
        /// </summary>
        /// <param name="S_Data">大图数据</param>
        /// <param name="P_Data">小图数据</param>
        /// <param name="similar">误差值</param>
        /// <returns></returns>
        private unsafe List<Point> _FindPic(int left, int top, int width, int height, BitmapData S_Data, BitmapData P_Data, int similar)
        {
            Stopwatch sw = new Stopwatch();
            List<Point> List = new List<Point>();
            int S_stride = S_Data.Stride;
            int P_stride = P_Data.Stride;
            IntPtr S_Iptr = S_Data.Scan0;
            IntPtr P_Iptr = P_Data.Scan0;
            byte* S_ptr;
            byte* P_ptr;
            bool IsOk = false;
            int _BreakW = width - P_Data.Width + 1;
            int _BreakH = height - P_Data.Height + 1;
            sw.Start();
            for (int h = top; h < _BreakH; h++)
            {
                for (int w = left; w < _BreakW; w++)
                {
                    P_ptr = (byte*)(P_Iptr);
                    for (int y = 0; y < P_Data.Height; y++)
                    {
                        for (int x = 0; x < P_Data.Width; x++)
                        {
                            S_ptr = (byte*)((int)S_Iptr + S_stride * (h + y) + (w + x) * 3);
                            P_ptr = (byte*)((int)P_Iptr + P_stride * y + x * 3);
                            if (ScanColor(S_ptr[0], S_ptr[1], S_ptr[2], P_ptr[0], P_ptr[1], P_ptr[2], similar))  //比较颜色
                            {
                                IsOk = true;
                            }
                            else
                            {
                                IsOk = false; break;
                            }
                        }
                        if (IsOk == false) { break; }
                    }
                    if (IsOk) { List.Add(new Point(w, h)); }
                    IsOk = false;
                }
            }
            sw.Stop();
            Trace.WriteLine(sw.ElapsedMilliseconds);
            return List;
        }
        /// <summary>
        /// 全匹配找图
        /// </summary>
        /// <param name="S_Data">大图数据</param>
        /// <param name="P_Data">小图数据</param>
        /// <returns></returns>
        private unsafe List<Point> _FindPic(int left, int top, int width, int height, BitmapData S_Data, BitmapData P_Data)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Point> List = new List<Point>();
            int S_stride = S_Data.Stride;
            int P_stride = P_Data.Stride;
            IntPtr S_Iptr = S_Data.Scan0;
            IntPtr P_Iptr = P_Data.Scan0;
            byte* S_ptr;
            byte* P_ptr;
            bool IsOk = false;
            int _BreakW = width - P_Data.Width + 1;
            int _BreakH = height - P_Data.Height + 1;
            for (int h = top; h < _BreakH; h++)
            {
                for (int w = left; w < _BreakW; w++)
                {
                    P_ptr = (byte*)(P_Iptr);
                    for (int y = 0; y < P_Data.Height; y++)
                    {
                        for (int x = 0; x < P_Data.Width; x++)
                        {
                            S_ptr = (byte*)((int)S_Iptr + S_stride * (h + y) + (w + x) * 3);
                            P_ptr = (byte*)((int)P_Iptr + P_stride * y + x * 3);
                            if (S_ptr[0] == P_ptr[0] && S_ptr[1] == P_ptr[1] && S_ptr[2] == P_ptr[2])
                            {
                                IsOk = true;
                            }
                            else
                            {
                                IsOk = false; break;
                            }
                        }
                        if (IsOk == false) { break; }
                    }
                    if (IsOk) { List.Add(new Point(w, h)); }
                    IsOk = false;
                }
            }
            sw.Stop();
            Trace.WriteLine(sw.ElapsedMilliseconds);
            return List;
        }
        /// <summary>
        /// BGR偏色查找
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="g1"></param>
        /// <param name="r1"></param>
        /// <param name="b2"></param>
        /// <param name="g2"></param>
        /// <param name="r2"></param>
        /// <param name="similar"></param>
        /// <returns></returns>
        private unsafe bool ScanColor(byte b1, byte g1, byte r1, byte b2, byte g2, byte r2, int similar)
        {
            if ((Math.Abs(b1 - b2)) > similar) { return false; } //B
            if ((Math.Abs(g1 - g2)) > similar) { return false; } //G
            if ((Math.Abs(r1 - r2)) > similar) { return false; } //R
            return true;
        }
        /// <summary>
        /// BGR偏色查找
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="g1"></param>
        /// <param name="r1"></param>
        /// <param name="b2"></param>
        /// <param name="g2"></param>
        /// <param name="r2"></param>
        /// <param name="similar"></param>
        /// <returns></returns>
        private unsafe bool ScanColor(byte b1, byte g1, byte r1, byte b2, byte g2, byte r2, Color offsetColor)
        {
            if ((Math.Abs(b1 - b2)) > offsetColor.B) { return false; } //B
            if ((Math.Abs(g1 - g2)) > offsetColor.G) { return false; } //G
            if ((Math.Abs(r1 - r2)) > offsetColor.R) { return false; } //R
            return true;
        }
        /// <summary>
        /// BGR偏色查找
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="g1"></param>
        /// <param name="r1"></param>
        /// <param name="b2"></param>
        /// <param name="g2"></param>
        /// <param name="r2"></param>
        /// <param name="similar"></param>
        /// <returns></returns>
        private unsafe bool ScanColor(byte b1, byte g1, byte r1, byte b2, byte g2, byte r2, byte b3, byte g3, byte r3)
        {
            if ((Math.Abs(b1 - b2)) > b3) { return false; } //B
            if ((Math.Abs(g1 - g2)) > g3) { return false; } //G
            if ((Math.Abs(r1 - r2)) > r3) { return false; } //R
            return true;
        }
        /// <summary>
        /// 区域找色
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color">字符串:颜色 格式为"RRGGBB-DRDGDB",比如"123456-000000|aabbcc-202020".注意，这里只支持RGB颜色</param>
        /// <returns></returns>
        public unsafe Point FindColor(int x1, int y1, int x2, int y2, string colorStr)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<(Color color, Color offsetColor)> allColors = new List<(Color color, Color offsetColor)>();
            string[] colors = colorStr.Split('|');
            foreach (var c in colors)
            {
                var cs = (c + "-000000").Split('-');
                allColors.Add((Color.FromArgb(Convert.ToInt32("FF" + cs[0], 16)), Color.FromArgb(Convert.ToInt32("FF" + cs[1], 16))));
            }

            Bitmap S_bmp = 截屏(x1, y1, x2, y2);
            BitmapData S_Data = S_bmp.LockBits(new Rectangle(0, 0, S_bmp.Width, S_bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);//, PixelFormat.Format24bppRgb

            Rectangle re = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            if (re.IsEmpty)
            {
                re = new Rectangle(0, 0, S_bmp.Width, S_bmp.Height);
            }
            ////优化 unadfe 用指针计算像素
            ////优化 searchLeftTop 用指针计算像素 指定区域 X 坐标
            ////优化 searchSize 用指针计算像素  指定区域 X 坐标
            var searchLeftTop = re.Location;
            var searchSize = re.Size;

            //var iMax = searchLeftTop.Y + searchSize.Height;//行
            var iMax = searchSize.Height;//行
                                         // var jMax = searchLeftTop.X + searchSize.Width;//列
            var jMax = searchSize.Width;//列
            int pointX = -1; int pointY = -1;
            IntPtr _Iptr = S_Data.Scan0;
            byte* _ptr;
            //List<Point> List = new List<Point>();

            sw.Stop();
            Trace.WriteLine("耗时:" + sw.ElapsedMilliseconds.ToString() + "毫秒");
            for (int y = 0; y < iMax; y++)
            {
                for (int x = 0; x < jMax; x++)
                {
                    _ptr = (byte*)((int)_Iptr + S_Data.Stride * (y) + (x) * 3);
                    foreach (var c in allColors)
                    {
                        if (ScanColor(_ptr[0], _ptr[1], _ptr[2], c.color.B, c.color.G, c.color.R, c.offsetColor))
                        {
                            pointX = x + x1;
                            pointY = y + y1;

                            goto END;
                        }
                    }

                }
            }
        END:

            S_bmp.UnlockBits(S_Data);
            return new Point(pointX, pointY);
        }

        /// <summary>
        /// 将字符串转换为颜色对象
        /// </summary>
        /// <param name="colorStr"></param>
        /// <returns></returns>
        private (Color color, Color offsetColor) StrToColor(string colorStr)
        {
            string[] colors = (colorStr + "-000000").Split('-');
            return (Color.FromArgb(Convert.ToInt32("FF" + colors[0], 16)), Color.FromArgb(Convert.ToInt32("FF" + colors[1], 16)));
        }
        /// <summary>
        /// 多点找色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public unsafe ImageInfo FindMultiColor(ImageInfo info)
        {
            (Color color, Color offsetColor) firstColor = StrToColor(info.Color);
            List<(int x, int y, (Color color, Color offsetColor) color)> allColors = new List<(int x, int y, (Color color, Color offsetColor) color)>();
            string[] colors = info.Offset_color.Split(',');
            foreach (var c in colors)
            {
                var cs = c.Split('|');
                allColors.Add((int.Parse(cs[0]), int.Parse(cs[1]), StrToColor(cs[2])));
            }
            int offsetMaxH = allColors.Max(m => m.y);
            int offsetMaxW = allColors.Max(m => m.x);

            Bitmap S_bmp = 截屏(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2);
            BitmapData S_Data = S_bmp.LockBits(new Rectangle(0, 0, S_bmp.Width, S_bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);//, PixelFormat.Format24bppRgb

            Rectangle re = new Rectangle(info.Range.x1, info.Range.y1, info.Range.x2 - info.Range.x1, info.Range.y2 - info.Range.y1);
            if (re.IsEmpty)
            {
                re = new Rectangle(0, 0, S_bmp.Width, S_bmp.Height);
            }
            ////优化 unadfe 用指针计算像素
            ////优化 searchLeftTop 用指针计算像素 指定区域 X 坐标
            ////优化 searchSize 用指针计算像素  指定区域 X 坐标
            var searchLeftTop = re.Location;
            var searchSize = re.Size;

            //var iMax = searchLeftTop.Y + searchSize.Height;//行
            var iMax = searchSize.Height;//行
                                         // var jMax = searchLeftTop.X + searchSize.Width;//列
            var jMax = searchSize.Width;//列
            IntPtr _Iptr = S_Data.Scan0;
            byte* _ptr;

            //List<Point> List = new List<Point>();
            info.Result = (-1, -1, false);
            for (int y = 0; y < iMax - offsetMaxH; y++)
            {
                for (int x = 0; x < jMax - offsetMaxW; x++)
                {
                    _ptr = (byte*)((int)_Iptr + S_Data.Stride * (y) + (x) * 3);
                    if (ScanColor(_ptr[0], _ptr[1], _ptr[2], firstColor.color.B, firstColor.color.G, firstColor.color.R, firstColor.offsetColor))
                    {
                        int cnt = 0;
                        foreach (var offset in allColors)
                        {
                            _ptr = (byte*)((int)_Iptr + S_Data.Stride * (y + offset.y) + (x + offset.x) * 3);
                            if (ScanColor(_ptr[0], _ptr[1], _ptr[2], offset.color.color.B, offset.color.color.G, offset.color.color.R, offset.color.offsetColor)) cnt++;
                        }
                        //相似度
                        if (cnt >= allColors.Count * info.Sim)
                        {
                            //找到第一个颜色
                            info.Result.x = x + info.Range.x1;
                            info.Result.y = y + info.Range.y1;
                            info.Result.isFinded = true;
                            goto END;
                        }
                    }
                }
            }
        END:
            S_bmp.UnlockBits(S_Data);
            return info;
        }
        /// <summary>
        /// 根据图色信息查找位置
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ImageInfo 找屏(ImageInfo info)
        {
            if (info.PicName.Length > 0)
            {
                //找图
                return this.FindPic(info);
            }
            else
            {
                //多点找色
                return FindMultiColor(info);
            }
        }
    }
}
