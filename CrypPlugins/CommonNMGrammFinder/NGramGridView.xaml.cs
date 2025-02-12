using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrypTool.Plugins.CommonNMGrammFinder
{
    public partial class NGramGridView : UserControl, INotifyPropertyChanged
    {
        private Dictionary<string, int> _nGramPairDictionary;
        private int _lowerBoundary;
        private int _upperBoundary;

        public NGramGridView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public int LowerBoundary
        {
            get => _lowerBoundary;
            set
            {
                if (_lowerBoundary != value)
                {
                    _lowerBoundary = value;
                    OnPropertyChanged(nameof(LowerBoundary));
                    UpdateConverterBoundaries();
                }
            }
        }

        public int UpperBoundary
        {
            get => _upperBoundary;
            set
            {
                if (_upperBoundary != value)
                {
                    _upperBoundary = value;
                    OnPropertyChanged(nameof(UpperBoundary));
                    UpdateConverterBoundaries();
                }
            }
        }

        private void UpdateConverterBoundaries()
        {
            var converter = (FrequencyToColorConverter)Resources["FrequencyToColorConverter"];
            converter.LowerBoundary = LowerBoundary;
            converter.UpperBoundary = UpperBoundary;
        }

        public void SetData(Dictionary<string, int> nGramPairDictionary, int entriesToShow)
        {
            _nGramPairDictionary = nGramPairDictionary;

            NGramFrequencyGrid.Children.Clear();
            NGramFrequencyGrid.RowDefinitions.Clear();
            NGramFrequencyGrid.ColumnDefinitions.Clear();

            // Sort pair_dict by frequency in descending order and take the top entriesToShow entries
            var sortedPairs = nGramPairDictionary.OrderByDescending(pair => pair.Value).Take(entriesToShow).ToList();

            // Extract unique N-Grams and M-Grams
            var ngramKeys = sortedPairs.Select(pair => pair.Key.Split(' ')[0]).Distinct().ToList();
            var mgramKeys = sortedPairs.Select(pair => pair.Key.Split(' ')[1]).Distinct().ToList();

            // Create columns for ngramKeys
            NGramFrequencyGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Empty top-left cell
            foreach (var ngram in ngramKeys)
            {
                NGramFrequencyGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Create rows for mgramKeys
            NGramFrequencyGrid.RowDefinitions.Add(new RowDefinition()); // Empty top-left cell
            foreach (var mgram in mgramKeys)
            {
                NGramFrequencyGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Add ngramKeys to the first row
            for (int i = 0; i < ngramKeys.Count; i++)
            {
                var textBlock = new TextBlock
                {
                    Text = ngramKeys[i],
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0.5),
                    Child = textBlock
                };
                Grid.SetRow(border, 0);
                Grid.SetColumn(border, i + 1);
                NGramFrequencyGrid.Children.Add(border);
            }

            // Add mgramKeys to the first column
            for (int i = 0; i < mgramKeys.Count; i++)
            {
                var textBlock = new TextBlock
                {
                    Text = mgramKeys[i],
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0.5),
                    Child = textBlock
                };
                Grid.SetRow(border, i + 1);
                Grid.SetColumn(border, 0);
                NGramFrequencyGrid.Children.Add(border);
            }

            // Add frequency values to the grid
            for (int row = 0; row < mgramKeys.Count; row++)
            {
                for (int col = 0; col < ngramKeys.Count; col++)
                {
                    string key = $"{ngramKeys[col]} {mgramKeys[row]}";
                    var textBlock = new TextBlock
                    {
                        Text = nGramPairDictionary.TryGetValue(key, out int frequency) ? frequency.ToString() : string.Empty,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    var border = new Border
                    {
                        Background = (frequency >= LowerBoundary && frequency <= UpperBoundary)
                            ? (Brush)new FrequencyToColorConverter
                            {
                                LowerBoundary = LowerBoundary,
                                UpperBoundary = UpperBoundary
                            }.Convert(frequency, typeof(Brush), null, null)
                            : Brushes.Transparent,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0.5),
                        Child = textBlock
                    };
                    Grid.SetRow(border, row + 1);
                    Grid.SetColumn(border, col + 1);
                    NGramFrequencyGrid.Children.Add(border);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}





