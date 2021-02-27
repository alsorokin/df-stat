namespace Snay.DFStat.Watch
{
    public class LineAddedArgs
    {
        public string LineText { get; }

        public LineAddedArgs(string lineText) 
        { 
            LineText = lineText; 
        }
    }
}
