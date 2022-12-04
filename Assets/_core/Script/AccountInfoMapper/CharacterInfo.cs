namespace _core.AcountInfo
{
    public class CharacterInfo
    {


        private string _account;
        private string _name;
        private string _exp;
        

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Exp
        {
            get => _exp;
            set => _exp = value;
        }

        public string Account
        {
            get => _account;
            set => _account = value;
        }
    }
}