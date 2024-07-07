using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public class OcrResult
    {
        public int Code { get; set; }

        public List<OcrItem> Data { get; set; }
        
        public double Score { get; set; }
        public double Time { get; set; }
        public double Timestamp { get; set; }
    }

    public class OcrItem
    {
        public int[][] Box { get; set; }
        public double Score { get; set; }
        public string Text { get; set; } // 注意：这里的文本可能包含Unicode编码，需要解码处理
        public string End { get; set; }
    }
}
