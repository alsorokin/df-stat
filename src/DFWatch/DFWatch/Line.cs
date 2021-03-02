using System.Collections.Generic;
using System.Linq;

namespace Snay.DFStat.Watch
{
    public class Line
    {
        public readonly LineType LnType;
        public readonly string Text;
        public readonly List<string> Traits;

        public Line (LineType lnType, string text, IEnumerable<string> traits = null)
        {
            LnType = lnType;
            Text = text;
            Traits = traits.ToList() ?? new List<string>();
        }
    }
}
