using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoProject.Core
{
    /// <summary>
    /// 音符颜色（红 / 蓝），以后可以扩展更多类型。
    /// </summary>
    public enum NoteColor
    {
        Red,
        Blue
    }

    /// <summary>
    /// 表示一条谱面上的音符。
    /// 目前只包含：出现时间 + 颜色。
    /// 以后可以加：判定状态（是否已经被击中）、位置等。
    /// </summary>
    public class Note
    {
        /// <summary>
        /// 该音符应该被击中的时间（单位：秒，从歌曲开始算起）。
        /// B 会用它来和当前时间比较，决定 Perfect / Good / Bad。
        /// </summary>
        public double Time { get; set; }

        /// <summary>
        /// 该音符是红鼓还是蓝鼓。
        /// C 在画 UI 的时候，也可以根据颜色选不同图片。
        /// </summary>
        public NoteColor Color { get; set; }
    }
}