using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public class DeviceScreenDm : IDeviceScreen
    {
        private IDevice device;
        private Dm.dmsoft dm;
        public DeviceScreenDm(IDevice device, Dm.dmsoft dm)
        {
            this.device = device;
            this.dm = dm;
        }
        public ImageInfo 找屏(ImageInfo info)
        {
            object x, y;
            if (info.PicName == "")
            {
             
                //没有图片名字的时候,是找色
                dm.FindMultiColor(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2, info.Color,
                    info.Offset_color, info.Sim, info.Dir, out  x, out  y);
            }
            else
            {   
                dm.FindPic(info.Range.x1, info.Range.y1, info.Range.x2, info.Range.y2, info.PicName + ".bmp",
                    info.Offset_color, info.Sim, info.Dir, out  x, out  y);
            }
           
            info.Result = (Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(x) > 0);
            return info;
        }
    }
}