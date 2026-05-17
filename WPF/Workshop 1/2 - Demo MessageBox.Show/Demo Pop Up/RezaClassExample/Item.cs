namespace RezaClassExample
{
    class Item
    {
        public string FirstData { get; set; }
        public string LastData { get; set; }

        public override string ToString()
        {
            return $"{FirstData} {LastData}";
        }

        public void Run()
        {
            System.Windows.MessageBox.Show("First WPF");
        }
    }
}
