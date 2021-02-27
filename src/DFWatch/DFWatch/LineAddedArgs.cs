namespace Snay.DFStat.Watch
{
    public class LineAddedArgs
    {
        public string LnText { get; }
        public LineType LnType { get; }

        public LineAddedArgs(string lineText, LineType lineType) 
        { 
            LnText = lineText;
            LnType = lineType;
        }
    }
}
