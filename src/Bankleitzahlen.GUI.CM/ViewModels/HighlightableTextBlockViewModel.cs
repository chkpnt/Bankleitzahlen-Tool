using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Bankleitzahlen.GUI.CM.ViewModels
{
    class HighlightableTextBlockViewModel : ViewAware
    {
        #region Properties für den View
        public string Text { get; set; } = string.Empty;
        #endregion

        private TextBlock _textBlock;

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            var frameworkElement = view as FrameworkElement;
            _textBlock = frameworkElement?.FindName("TextBlock") as TextBlock;
        }

        #region Actions für den View
        public bool Contains(string filter) => Text.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;

        public void HighlightText(string filter)
        {
            if (_textBlock == null) { return; }

            if (string.IsNullOrEmpty(filter))
            {
                _textBlock.Inlines.Clear();
                _textBlock.Text = Text;
                return;
            }

            Execute.OnUIThread(() =>
            {
                var positionen = Regex.Matches(Text, filter, RegexOptions.IgnoreCase)
                    .Cast<Match>()
                    .Select(m => m.Index);

                var runs = BuildRuns(Text, positionen, filter.Length);

                _textBlock.Text = string.Empty;
                _textBlock.Inlines.Clear();
                _textBlock.Inlines.AddRange(runs);
            });
        }
        #endregion

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
