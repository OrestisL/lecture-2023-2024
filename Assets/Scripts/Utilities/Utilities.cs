namespace Lecture
{
    public class Person 
    {
        public string Name { get { return _name; } private set { _name = value; } }
        private string _name;
        public string LastName { get { return _lastName; } private set { _lastName = value; } }
        private string _lastName;
        public int Age { get { return _age; } private set { _age = value; } }
        private int _age;

        public Person() { }

        public Person(string name, string lastname, int age)
        {
            Name = name;
            LastName = lastname;
            Age = age;
        }
    }

}