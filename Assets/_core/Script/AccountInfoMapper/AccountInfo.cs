namespace _core.AcountInfo
{
    public class AccountInfo
    {
        private string _account;
        private string _sex;
        private string _bornYear;

        public string Account
        {
            get => _account;
            set => _account = value;
        }

        public string Sex
        {
            get => _sex;
            set => _sex = value;
        }

        public string BornYear
        {
            get => _bornYear;
            set => _bornYear = value;
        }
    }
}