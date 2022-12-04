namespace _core.Script.Achieve
{
    public class AchieveSystem
    {

        private IMemento _memento;
        
        AchieveSystem()
        {
            _memento = new Memento();

            SetMemento(_memento);
        }

        public void CreateMemento()
        {
            
            //save this data
            _memento.SaveData();
        }


        private void SetMemento(IMemento memento)
        {
            //get data from the memento 
            
        }
    }
}