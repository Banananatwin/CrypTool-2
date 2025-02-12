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
using System;
using System.ComponentModel;
using CrypTool.PluginBase;
using CrypTool.PluginBase.Miscellaneous;

namespace CrypTool.Plugins.CommonNMGrammFinder
{
    // HOWTO: rename class (click name, press F2)
    public class CommonNMGrammFinderSettings : ISettings
    {
        #region Private Variables

        private bool _skipNGram = false;
        private bool _ignoreShort = true;
        private string _charactersToSkip = ""; 
        private string _charactersToReplace = "!?:;,.()[]{}";
        private string _charactersToSplit = "";
        private string _replacementCharacter = "";
        private int _lowerboundary = 0;
        private int _upperboundary = 1000000;
        private int _entriesToShow = 100;
        private int _nGramLength = 5;
        private int _mGramLength = 4;
        private bool _caseSensitive = true;

        #endregion

        #region TaskPane Settings

        /// <summary>
        /// HOWTO: This is an example for a setting entity shown in the settings pane on the right of the CT2 main window.
        /// This example setting uses a number field input, but there are many more input types available, see ControlType enumeration.
        /// </summary>
        [TaskPane("Skip N-Gram Length?", "Do you want to move through the text by jumping the N-Gram length or by moving one letter over?", "Analysis Settings", 1, false, ControlType.CheckBox, ValidationType.RangeInteger, 0, 1)]
        public bool SkipNGram
        {
            get
            {
                return _skipNGram;
            }
            set
            {
                if (_skipNGram != value)
                {
                    _skipNGram = value;
                    // HOWTO: MUST be called every time a property value changes with correct parameter name
                    OnPropertyChanged("SkipNGram");
                }
            }
        }

        [TaskPane("Case Sensitive?", "Do you want to differentiate between case?", "Analysis Settings", 2, false, ControlType.CheckBox, ValidationType.RangeInteger, 0, 1)]
        public bool CaseSensitive
        {
            get
            {
                return _caseSensitive;
            }
            set
            {
                if (_caseSensitive != value)
                {
                    _caseSensitive = value;
                    // HOWTO: MUST be called every time a property value changes with correct parameter name
                    OnPropertyChanged("CaseSensitive");
                }
            }
        }

        [TaskPane("Ignore short N-Grams?", "When you use the 'Character to Split on' option, it is possible that N-Grams would be created that are too shorter than you specified N- or M-Gram length. If this option is activated, those N-Grams will be discarded", "Analysis Settings", 3, false, ControlType.CheckBox, ValidationType.RangeInteger, 0, 1)]
        public bool IgnoreShort
        {
            get
            {
                return _ignoreShort;
            }
            set
            {
                if (_ignoreShort != value)
                {
                    _ignoreShort = value;
                    // HOWTO: MUST be called every time a property value changes with correct parameter name
                    OnPropertyChanged("IgnoreShort");
                }
            }
        }

        [TaskPane("N-Gram Length:", "Specify length of N-Grams", "Analysis Settings", 4, false, ControlType.TextBox)]
        public int NGramLength
        {
            get
            {
                return _nGramLength;
            }
            set
            {
                if (_nGramLength != value)
                {
                    _nGramLength = value;
                    OnPropertyChanged("NGramLength");
                }
            }
        }

        [TaskPane("M-Gram Length:", "Specify length of M-Grams", "Analysis Settings", 5, false, ControlType.TextBox)]
        public int MGramLength
        {
            get
            {
                return _mGramLength;
            }
            set
            {
                if (_mGramLength != value)
                {
                    _mGramLength = value;
                    OnPropertyChanged("MGramLength");
                }
            }
        }

        [TaskPane("Character to Skip?", "Specify the characters you want to skip during the analysis of N-Grams", "Analysis Settings", 6, false, ControlType.TextBox)]
        public string CharactersToSkip
        {
            get
            {
                return _charactersToSkip;
            }
            set
            {
                if (_charactersToSkip != value)
                {
                    _charactersToSkip = value;
                    OnPropertyChanged("CharactersToSkip");
                }
            }
        }

        [TaskPane("Character to Split on:", "Specify the characters you want to split on during the analysis of N-Grams", "Analysis Settings", 7, false, ControlType.TextBox)]
        public string CharactersToSplit
        {
            get
            {
                return _charactersToSplit;
            }
            set
            {
                if (_charactersToSplit != value)
                {
                    _charactersToSplit = value;
                    OnPropertyChanged("CharactersToSplit");
                }
            }
        }

        [TaskPane("Character to Replace:", "Specify the characters you want to replace during the analysis of N-Grams", "Analysis Settings", 8, false, ControlType.TextBox)]
        public string CharactersToReplace
        {
            get
            {
                return _charactersToReplace;
            }
            set
            {
                if (_charactersToReplace != value)
                {
                    _charactersToReplace = value;
                    OnPropertyChanged("CharactersToReplace");
                }
            }
        }


        [TaskPane("Replacement Character:", "Specify the characters you want to split on during the analysis of N-Grams", "Analysis Settings", 9, false, ControlType.TextBox)]
        public string ReplacementCharacter
        {
            get
            {
                return _replacementCharacter;
            }
            set
            {
                if (_replacementCharacter != value)
                {
                    _replacementCharacter = value;
                    OnPropertyChanged("ReplacementCharacter");
                }
            }
        }

        [TaskPane("Grid size?", "Specify how many entries are used to build the grid", "Visualization Settings", 1, false, ControlType.TextBox)]
        public int EntriesToShow
        {
            get
            {
                return _entriesToShow;
            }
            set
            {
                if (_entriesToShow != value)
                {
                    _entriesToShow = value;
                    OnPropertyChanged("EntriesToShow");
                }
            }
        }

        [TaskPane("Coloring Lower Boundary:", "Specify the lower boundary for coloring", "Visualization Settings", 2, false, ControlType.TextBox)]
        public int LowerBoundary
        {
            get
            {
                return _lowerboundary;
            }
            set
            {
                if (_lowerboundary != value)
                {
                    _lowerboundary = value;
                    OnPropertyChanged("LowerBoundary");
                }
            }
        }

        [TaskPane("Coloring Upper Boundary:", "Specify the upper boundary for coloring", "Visualization Settings", 3, false, ControlType.TextBox)]
        public int UpperBoundary
        {
            get
            {
                return _upperboundary;
            }
            set
            {
                if (_upperboundary != value)
                {
                    _upperboundary = value;
                    OnPropertyChanged("UpperBoundary");
                }
            }
        }
        
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            EventsHelper.PropertyChanged(PropertyChanged, this, propertyName);
        }

        #endregion

        public void Initialize()
        {

        }
    }
}
