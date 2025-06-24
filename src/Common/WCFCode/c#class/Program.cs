namespace Name
{
    public class Person
    {
        public Person()
        {
            Console.WriteLine("Hello from constructor");
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello from Main");
            Person p = new Person(); // Creating object to invoke constructor
        }
    }
    
}