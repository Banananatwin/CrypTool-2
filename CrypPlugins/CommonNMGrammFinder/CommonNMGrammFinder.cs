/*
   Copyright CrypTool 2 Team <ct2contact@cryptool.org>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using CrypTool.PluginBase;
using CrypTool.PluginBase.Miscellaneous;
using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Documents;

namespace CrypTool.Plugins.CommonNMGrammFinder
{
    [Author("Thomas Brimmers", "thomas.brimmers@stud.hn.de", "Hochschule Niederrhein Master Project", "hsnr.de")]
    [PluginInfo(
        "NM-Gram Finder",
        "Finds common N-M-Grams in text",
        "CommonNMGrammFinder/userdoc.xml",
        new[] { "CrypWin/images/default.png" })]
    // HOWTO: Change category to one that fits to your plugin. Multiple categories are allowed.
    [ComponentCategory(ComponentCategory.ToolsMisc)]
    public class CommonNMGrammFinder : ICrypComponent
    {
        #region Private Variables

        // HOWTO: You need to adapt the settings class as well, see the corresponding file.
        private readonly CommonNMGrammFinderSettings _settings = new CommonNMGrammFinderSettings();
        private string inputTextData;
        private NGramGridView _nGramGridView;

        public CommonNMGrammFinder()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _nGramGridView = new NGramGridView();
            });
        }


        #endregion

        #region Data Properties

        /// <summary>
        /// HOWTO: Input interface to read the input data. 
        /// You can add more input properties of other type if needed.
        /// </summary>
        [PropertyInfo(Direction.InputData, "Text Input", "Text Input", true)]
        public string InputString
        {
            get => inputTextData;
            set
            {
                if (value != inputTextData)
                {
                    inputTextData = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [PropertyInfo(Direction.OutputData, "Highlighted Text", "Shows N-gram highlighted in the provided text")]
        public string HighlightedTextOutput
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [PropertyInfo(Direction.OutputData, "N-Gram Table", "Show table for frequency of N-M-Gram pairs")]
        public string TableOutput
        {
            get;
            set;
        }

        #endregion

        #region IPlugin Members

        /// <summary>
        /// Provide plugin-related parameters (per instance) or return null.
        /// </summary>
        public ISettings Settings => _settings;


        /// <summary>
        /// Provide custom presentation to visualize the execution or return null.
        /// </summary>
        public UserControl Presentation => _nGramGridView;

        /// <summary>
        /// Called once when workflow execution starts.
        /// </summary>
        public void PreExecution()
        {
        }

        /// <summary>
        /// Sanitizes the input string by removing all characters that are in _settings.CharactersToSkip.
        /// Also replaces characters in _settings.CharactersToReplace with _settings.ReplacementCharacter and removes line breaks.
        /// </summary>
        /// <param name="inputString">The input string to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        public string SanitizeInputString(string inputString)
        {
            StringBuilder sanitizedString = new StringBuilder();
            foreach (char c in inputString)
            {
                if (!_settings.CharactersToSkip.Contains(c))
                {
                    if (_settings.CharactersToReplace.Contains(c))
                    {
                        sanitizedString.Append(_settings.ReplacementCharacter);
                    }
                    else
                    {
                        sanitizedString.Append(c);
                    }
                }
            }

            string result = sanitizedString.ToString().Replace("\r", "").Replace("\n", "");

            if (!_settings.CaseSensitive)
            {
                result = result.ToLower();
            }

            return result;
        }

        /// <summary>
        /// Generates a dictionary of N-Gram and M-Gram pairs with their frequencies.
        /// </summary>
        /// <param name="inputString">The input string to analyze.</param>
        /// <param name="nGramLength">The length of the N-Gram.</param>
        /// <param name="mGramLength">The length of the M-Gram.</param>
        /// <param name="skipNGram">If true, moves through the text by jumping the N-Gram length; otherwise, moves one letter at a time.</param>
        /// <returns>A tuple containing three dictionaries: PairDictionary, nGramDictionary, and mGramDictionary.</returns>
        public (Dictionary<string, int> PairDictionary, Dictionary<string, int> nGramDictionary, Dictionary<string, int> mGramDictionary) GenerateNGramPairDictionary(string inputString, int nGramLength, int mGramLength, bool skipNGram)
        {
            Dictionary<string, int> PairDictionary = new Dictionary<string, int>();
            Dictionary<string, int> nGramDictionary = new Dictionary<string, int>();
            Dictionary<string, int> mGramDictionary = new Dictionary<string, int>();

            int step = skipNGram ? nGramLength : 1;
            int i = 0;
            while (i < inputString.Length - nGramLength - mGramLength + 1)
            {
                string nGram = GetNGram(inputString, i, nGramLength, out int nGramEndIndex);
                string mGram = GetNGram(inputString, i + nGramLength, mGramLength, out int mGramEndIndex);

                if (_settings.IgnoreShort && ((nGram.Length < nGramLength) || (mGram.Length < mGramLength)))
                {
                    i = Math.Max(nGramEndIndex, mGramEndIndex) + 1;
                    continue;
                }

                string nGramPair = nGram + " " + mGram;

                if (PairDictionary.ContainsKey(nGramPair))
                {
                    PairDictionary[nGramPair]++;
                }
                else
                {
                    PairDictionary.Add(nGramPair, 1);
                }

                if (nGramDictionary.ContainsKey(nGram))
                {
                    nGramDictionary[nGram]++;
                }
                else
                {
                    nGramDictionary.Add(nGram, 1);
                }

                if (mGramDictionary.ContainsKey(mGram))
                {
                    mGramDictionary[mGram]++;
                }
                else
                {
                    mGramDictionary.Add(mGram, 1);
                }

                i += step;
            }
            return (PairDictionary, nGramDictionary, mGramDictionary);
        }

        /// <summary>
        /// Extracts an N-Gram from the input string starting at the specified index and up to the specified length.
        /// Terminates early if a character in _settings.CharactersToSplit is encountered.
        /// </summary>
        /// <param name="inputString">The input string to extract the N-Gram from.</param>
        /// <param name="startIndex">The starting index for the N-Gram.</param>
        /// <param name="length">The maximum length of the N-Gram.</param>
        /// <param name="endIndex">The end index of the N-Gram.</param>
        /// <returns>The extracted N-Gram.</returns>
        private string GetNGram(string inputString, int startIndex, int length, out int endIndex)
        {
            StringBuilder nGram = new StringBuilder();
            endIndex = startIndex;
            for (int i = startIndex; i < startIndex + length && i < inputString.Length; i++)
            {
                if (_settings.CharactersToSplit.Contains(inputString[i]))
                {
                    endIndex = i;
                    break;
                }
                nGram.Append(inputString[i]);
                endIndex = i;
            }
            return nGram.ToString();
        }

        /// <summary>
        /// Generates a string representation of the N-Gram pair dictionary.
        /// </summary>
        /// <param name="nGramPairDictionary">The dictionary containing N-Gram pairs and their frequencies.</param>
        /// <returns>A string representation of the N-Gram pair dictionary.</returns>
        public string GenerateNGramTable(Dictionary<string, int> nGramPairDictionary)
        {
            StringBuilder table = new StringBuilder();
            table.Append("N-Gram\tM-Gram\tFrequency\n");
            foreach (KeyValuePair<string, int> entry in nGramPairDictionary.OrderByDescending(e => e.Value))
            {
                string[] nGramPair = entry.Key.Split(' ');
                table.Append(nGramPair[0] + " |\t" + nGramPair[1] + " |\t" + entry.Value + " |\n");
            }
            return table.ToString();
        }


        /// <summary>
        /// Called every time this plugin is run in the workflow execution.
        /// </summary>
        public void Execute()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressChanged(0, 3);
                var sanitizedInputString = SanitizeInputString(InputString);
                ProgressChanged(1, 3);

                HighlightedTextOutput = sanitizedInputString;
                OnPropertyChanged("HighlightedTextOutput");
                ProgressChanged(2, 3);


                var (pair_dict, ngram_dict, mgram_dict) = GenerateNGramPairDictionary(sanitizedInputString, _settings.NGramLength, _settings.MGramLength, _settings.SkipNGram);
                _nGramGridView.LowerBoundary = _settings.LowerBoundary;
                _nGramGridView.UpperBoundary = _settings.UpperBoundary;
                _nGramGridView.SetData(pair_dict, _settings.EntriesToShow + 1, InputString);
                TableOutput = GenerateNGramTable(pair_dict);
                OnPropertyChanged("TableOutput");

                ProgressChanged(3, 3);
            });
        }

        /// <summary>
        /// Called once after workflow execution has stopped.
        /// </summary>
        public void PostExecution()
        {
        }

        /// <summary>
        /// Triggered time when user clicks stop button.
        /// Shall abort long-running execution.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Called once when plugin is loaded into editor workspace.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Called once when plugin is removed from editor workspace.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region Event Handling

        public event StatusChangedEventHandler OnPluginStatusChanged;

        public event GuiLogNotificationEventHandler OnGuiLogNotificationOccured;

        public event PluginProgressChangedEventHandler OnPluginProgressChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private void GuiLogMessage(string message, NotificationLevel logLevel)
        {
            EventsHelper.GuiLogMessage(OnGuiLogNotificationOccured, this, new GuiLogEventArgs(message, this, logLevel));
        }

        private void OnPropertyChanged(string name)
        {
            EventsHelper.PropertyChanged(PropertyChanged, this, new PropertyChangedEventArgs(name));
        }

        private void ProgressChanged(double value, double max)
        {
            EventsHelper.ProgressChanged(OnPluginProgressChanged, this, new PluginProgressEventArgs(value, max));
        }

        #endregion
    }
}
