using System.Collections.Generic;
using System.Linq;

namespace BlogManagement.Validation
{
    public class ValidationResult
    {
        public List<string> ErrorMessages { get; set; }

        public bool IsSuccessful => !ErrorMessages.Any();
    }
}