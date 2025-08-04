namespace EmployeeManagement.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject 
    {
        public string Number { get; private set; }
        public string Type { get; private set; } 

        private PhoneNumber() { }

        public PhoneNumber(string number, string type)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentException("Phone number cannot be empty.", nameof(number));
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Phone type cannot be empty.", nameof(type));
            }

            
            Number = number;
            Type = type;
        }

        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return Type;
        }

        public override string ToString()
        {
            return $"{Type}: {Number}";
        }
    }
}