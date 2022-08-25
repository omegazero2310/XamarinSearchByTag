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
                //tách dấu cách
                var listword = parameter.ToString().Split(' ').ToList();
                //kiếm tra chuỗi có tag
                int indexStart = 0;
                int indexEnd = 0;
                var dict = new Dictionary<string, string>();
                foreach (var keyword in listword.Where(str => str.Contains(":")))
                {
                    //nếu gặp tag
                    if (keyword.Contains(":"))
                    {
                        //lấy tên tag
                        string tag = keyword.Split(':')[0];
                        //chưa có tag thì thêm vào Dictionary
                        if (!dict.ContainsKey(tag))
                        {
                            //lấy index của tag
                            indexStart = listword.IndexOf(keyword);
                            //lấy chuỗi sau dấu ":" của tag và gẵn vào chuỗi đầu
                            string word = keyword.Split(':')[1];
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append(word);
                            //từ index này tìm index cuối cho đến khi gặp keyword khác
                            indexEnd = listword.Skip(indexStart).ToList().IndexOf(listword.Where(str =>
                                str.Contains(":") && string.Compare(str, keyword, true) != 0)
                                .FirstOrDefault() ?? "");
                            if (indexEnd == -1)
                                indexEnd = listword.Count;
                            //lấy chuỗi trong indexstart và indexend
                            for (int i = indexStart + 1; i < indexEnd; i++)
                            {
                                stringBuilder.Append(" " + listword[i]);
                            }
                            //lưu lại vào từ điển 
                            dict.Add(tag, stringBuilder.ToString());
                        }

                    }

                }
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
