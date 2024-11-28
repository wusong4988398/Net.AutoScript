namespace AutoScript.Server
{
    public partial class Form1 : Form
    {
     
        public Form1()
        {
            InitializeComponent();
            Account.OnStatusChangedEvent += Account_OnStatusChangedEvent;
        }

        private void Account_OnStatusChangedEvent(object? sender, Account e)
        {
            Account account = (Account)sender;
            Console.WriteLine(account.Status);
            Console.WriteLine(account.UserName);
            Console.WriteLine(account.Password);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<Account> accounts = Account.Reload("Resource/users.txt");
            //foreach (var account in accounts)
            //{
                
            //}
            //accounts.FirstOrDefault().Status = "333";
        }
    }
}
