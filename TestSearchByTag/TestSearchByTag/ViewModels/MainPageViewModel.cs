using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSearchByTag.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private DelegateCommand<object> _fieldName;
        public DelegateCommand<object> CommandName =>
            _fieldName ?? (_fieldName = new DelegateCommand<object>(ExecuteCommandName));
        private string _result;
        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }
        void ExecuteCommandName(object parameter)
        {
            if (parameter?.ToString().Length > 0)
            {
                //split string base on space character
                var listword = parameter.ToString().Split(' ').ToList();
                //Start check string by tag
                int indexStart = 0;
                int indexEnd = 0;
                var dict = new Dictionary<string, string>();
                foreach (var keyword in listword.Where(str => str.Contains(":")))
                {
                    //get the name of tag, example "thisIsATag:A B C" return "thisIsATag"
                    string tag = keyword.Split(':')[0];
                    //if the tag not exist in the Dictionary, add it
                    if (!dict.ContainsKey(tag))
                    {
                        //find the start index of a tag
                        indexStart = listword.IndexOf(keyword);
                        //get the string behind ":" character as starting word
                        string word = string.Empty;
                        StringBuilder stringBuilder = new StringBuilder();
                        try
                        {
                            word = keyword.Split(':')[1];
                            stringBuilder.Append(word);
                        }
                        catch
                        {
                            stringBuilder.Append("");
                        }
                        //find the last index from the start index, indexEnd equal next tag index
                        //exclude the current tag
                        indexEnd = listword.Skip(indexStart).ToList().IndexOf(listword.Where(str =>
                            str.Contains(":") && string.Compare(str, keyword, true) != 0)
                            .FirstOrDefault() ?? "");
                        if (indexEnd == -1)
                            indexEnd = listword.Count;
                        //get all the word in between 2 tag
                        for (int i = indexStart + 1; i < indexEnd; i++)
                        {
                            stringBuilder.Append(" " + listword[i]);
                        }
                        //save to Dictionary
                        dict.Add(tag, stringBuilder.ToString());
                    }
                }
                //output the Result
                this.Result = JsonConvert.SerializeObject(dict);
            }

        }
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }
    }
}
