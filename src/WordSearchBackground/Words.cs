using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearchBackground
{
    public class Words
    {
        public class CurrentState
        {
            public int LinesCounted;
            public int WordsCounted;
        }

        public string SourceFile;
        public string CompareString;
        private int WordCount;
        private int LinesCounted;

        public void CountWords(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            CurrentState state = new CurrentState();
            string line = "";
            int elapsedTime = 20;
            DateTime lastReportDateTime = DateTime.Now;

            if (string.IsNullOrEmpty(CompareString))
            {
                throw new Exception("CompareString not specified.");
            }

            // Open a new stream
            using (System.IO.StreamReader myStream = new System.IO.StreamReader(SourceFile))
            {
                while (!myStream.EndOfStream)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        line = myStream.ReadLine();
                        WordCount += CountInString(line, CompareString);
                        LinesCounted += 1;

                        int compare = DateTime.Compare(DateTime.Now, lastReportDateTime.AddMilliseconds(elapsedTime));
                        if (compare > 0)
                        {
                            state.LinesCounted = LinesCounted;
                            state.WordsCounted = WordCount;
                            worker.ReportProgress(0, state);
                            lastReportDateTime = DateTime.Now;
                        }
                    }

                    // Uncomment for testing.
                    //System.Threading.Thread.Sleep(5);
                }

                // Report the final count values.
                state.LinesCounted = LinesCounted;
                state.WordsCounted = WordCount;
                worker.ReportProgress(0, state);
            }
        }

        private int CountInString(string sourceString, string compareString)
        {
            // This function counts the number of times a word is found in line.
            if (sourceString == null)
            {
                return 0;
            }

            string EscapedCompareString = System.Text.RegularExpressions.Regex.Escape(compareString);
            System.Text.RegularExpressions.Regex regex;
            regex = new System.Text.RegularExpressions.Regex(
                // To count all occurrences of the string, even within words, remove both instances of @"\b" from the following line.
                @"\b" + EscapedCompareString + @"\b",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );

            System.Text.RegularExpressions.MatchCollection matches;
            matches = regex.Matches(sourceString);
            return matches.Count;
        }
    }
}
