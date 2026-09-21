

using System.ComponentModel.DataAnnotations;

namespace movie_booking.ValidationAttributes
{

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]  // here inherited is false because when parent class using this attribute is inherited by a child class then to the child class this attributes rule not getted affected.
    public class CheckMinimumRange : ValidationAttribute  // CheckMinimumRange inherits from the class ValidationAttribute so the namespace using System.ComponentModel.DataAnnotations; is required
    {
        public int MinimumRange { get; set; }
        public string LogMessege { get; set; }

        public CheckMinimumRange(int minimumRange) {
            this.MinimumRange = minimumRange;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext) {
            if (value is int toBeCheckedValue && toBeCheckedValue < MinimumRange) {
                return new ValidationResult($"the minimum range of should be {MinimumRange}");
            }
            return ValidationResult.Success;
        }
    }
}
