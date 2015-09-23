using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bankleitzahlen.GUI.PureWPF
{
    /// <summary>
    /// Interaction logic for HighlightableTextBlock.xaml
    /// </summary>
    public partial class HighlightableTextBlock : UserControl
    {
        public string Text { get; }

        public HighlightableTextBlock() : this("") { }

        public HighlightableTextBlock(string text)
        {
            InitializeComponent();

            Text = text;
            TextBlock.Text = text;
        }

        public bool Contains(string filter) => Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;

        public void HighlightText(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                TextBlock.Inlines.Clear();
                TextBlock.Text = Text;
                return;
            }

            var positionen = Regex.Matches(Text, filter, RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Select(m => m.Index);

            var runs = BuildRuns(Text, positionen, filter.Length);

            TextBlock.Text = string.Empty;
            TextBlock.Inlines.Clear();
            TextBlock.Inlines.AddRange(runs);
        }

        private IEnumerable<Run> BuildRuns(string text, IEnumerable<int> highlightPositions, int highlightLength)
        {
            var currentPos = 0;

            // Text: foobarfoobarabc
            // pos: 3 und 9, lenght: 3
            // -> HighlightTest: foo<b>bar</b>foo<b>bar</b>abc

            foreach (var pos in highlightPositions)
            {
                if (currentPos < pos)
                {
                    yield return new Run(text.Substring(currentPos, pos - currentPos));
                }

                yield return new Run
                {
                    Text = text.Substring(pos, highlightLength),
                    FontWeight = FontWeights.Bold
                };

                currentPos = pos + highlightLength;
            }

            if (currentPos < text.Length)
            {
                yield return new Run(text.Substring(currentPos, text.Length - currentPos));
            }
        }
    }
}
