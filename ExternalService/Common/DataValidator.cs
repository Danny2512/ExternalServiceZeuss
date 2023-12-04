using System.ComponentModel.DataAnnotations;

namespace ExternalService.Common
{
    public class DataValidator
    {
        public static List<string> ValidateModel<T>(T model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            return validationResults
                .Select(result => result.ErrorMessage)
                .ToList();
        }
    }
}
