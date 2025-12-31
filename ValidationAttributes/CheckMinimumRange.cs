

using System.ComponentModel.DataAnnotations;

namespace movie_booking.ValidationAttributes
{

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class CheckMinimumRange : ValidationAttribute
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
