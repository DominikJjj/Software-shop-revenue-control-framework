using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project_Tests.Helpers;

public static class ModelValidationHelper
{
    public static IEnumerable<ValidationResult> Validate(object model)
    {
        var res = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);

        Validator.TryValidateObject(model, context, res, true);

        return res;
    }
}