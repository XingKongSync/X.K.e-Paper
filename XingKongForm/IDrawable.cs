using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public interface IDrawable
    {
        void setName(string name);

        string getName();

        void setLeft(int left);

        int getLeft();

        void setTop(int top);

        int getTop();

        void Draw();

        bool needDraw();

        void ClearArea();

        /// <summary>
        /// 对于容器类控件，本方法会强制其所有内部控件重新绘制
        /// 对于普通控件，则等效于Draw方法
        /// </summary>
        void HardworkDraw();
    }
}
