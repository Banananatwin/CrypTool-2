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
        private string _charactersToSkip = "";

        #endregion

        #region TaskPane Settings

        /// <summary>
        /// HOWTO: This is an example for a setting entity shown in the settings pane on the right of the CT2 main window.
        /// This example setting uses a number field input, but there are many more input types available, see ControlType enumeration.
        /// </summary>
        [TaskPane("Skip N-Gram Length?", "Do you want to move through the text by jumping the N-Gram length or by moving one letter over?", null, 1, false, ControlType.CheckBox, ValidationType.RangeInteger, 0, 1)]
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

		[TaskPane("Character to Skip?", "Specify the characters you want to skip during the analysis of N-Grams", null, 1, false, ControlType.TextBox)]
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
					// HOWTO: MUST be called every time a property value changes with correct parameter name
					OnPropertyChanged("CharactersToSkip");
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
