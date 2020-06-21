using EasyHttp.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lab3
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<User> UsersList { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }

        private string _curNickName;
        public string CurNickName
        {
            get { return _curNickName; }
            set
            {
                _curNickName = value;
                OnPropertyChanged("CurNickName");
            }
        }

        private Answer _yAnswer;
        public Answer YAnswer
        {
            get { return _yAnswer; }
            set
            {
                _yAnswer = value;
                OnPropertyChanged("YAnswer");
            }
        }

        private Category _curCategory;
        public Category CurCategory
        {
            get { return _curCategory; }
            set
            {
                _curCategory = value;
                OnPropertyChanged("CurCategory");
            }
        }

        private QuestionModel _curQuestion;
        public QuestionModel CurQuestion
        {
            get { return _curQuestion; }
            set
            {
                _curQuestion = value;
                OnPropertyChanged("CurQuestion");
            }
        }

        public ApplicationViewModel()
        {
            Answers = new ObservableCollection<Answer>();

            var client = new HttpClient();
            var response = client.Get("https://localhost:44350/api/questions");
            Categories = response.StaticBody<ObservableCollection<Category>>();

            response = client.Get("https://localhost:44350/api/users");
            UsersList = response.StaticBody<ObservableCollection<User>>();
        }

        private RelayCommand getQuestion;
        public RelayCommand GetQuestion
        {
            get
            {
                return getQuestion ??
                    (getQuestion = new RelayCommand(obj =>
                    {
                        Answers.Clear();

                        var client = new HttpClient();
                        var response = client.Get($"https://localhost:44350/api/questions/{CurNickName}/{CurCategory.id}");
                        CurQuestion = response.StaticBody<QuestionModel>();

                        response = client.Get($"https://localhost:44350/api/questions/ans/{CurQuestion.Id}");
                        var ans = response.StaticBody<ObservableCollection<Answer>>();
                        foreach (Answer temp in ans) Answers.Add(temp);
                    },
                    (obj) => (CurNickName != null && CurNickName != "") && CurCategory != null));
            }
        }

        private RelayCommand sendResult;
        public RelayCommand SendResult
        {
            get
            {
                return sendResult ??
                    (sendResult = new RelayCommand(obj =>
                    {
                        var client = new HttpClient();
                        var response = client.Get($"https://localhost:44350/api/questions/{CurNickName}/{CurQuestion.Id}/{CurQuestion.Category}/{YAnswer.Text}");
                        bool Result = response.StaticBody<bool>();

                        if (Result)
                        {
                            MessageBox.Show("Вы дали правильный ответ");
                        }
                        else
                            MessageBox.Show("Ответ неверный");

                        UsersList.Clear();
                        response = client.Get("https://localhost:44350/api/users");
                        var Users = response.StaticBody<ObservableCollection<User>>();

                        foreach (User user in Users) UsersList.Add(user);
                    },
                    (obj) => YAnswer != null));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
