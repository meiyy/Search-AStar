using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Search_Algorithm
{
    public struct Node
    {
        public int x, y,f;
        public Node(int xx, int yy, int ff)
        {
            x = xx;
            y = yy;
            f = ff;
        }
    }
    public class NodeCmp : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if(x.f.CompareTo(y.f)==0)
            {
                if (x.x.CompareTo(y.x) != 0)
                    return x.x.CompareTo(y.x);
                return x.y.CompareTo(y.y);
            }
            return x.f.CompareTo(y.f);
        }
    }
}
