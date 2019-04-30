using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// Image文件
    /// </summary>
    public class Model_Image : Model_Base
    {
        /// <summary>
        /// 图片尺寸
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
        }
        /// <summary>
        /// 宽度（以像素为单位）
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度（以像素为单位）
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 位深度
        /// </summary>
        public int BitDepth { get; set; }
        /// <summary>
        /// 图片格式
        /// </summary>
        public string ImageFormat { get; set; }
    }
}
