using FluentValidation;
using OrganizationService.DTOs.LegalDocument;

namespace OrganizationService.Validation.LegalDocument
{
    public class CreateLegalDocumentRequestValidator : AbstractValidator<CreateLegalDocumentRequest>
    {
        public CreateLegalDocumentRequestValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DocumentNumber).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdateLegalDocumentRequestValidator : AbstractValidator<UpdateLegalDocumentRequest>
    {
        public UpdateLegalDocumentRequestValidator()
        {
            RuleFor(x => x.DocumentType).MaximumLength(100);
            RuleFor(x => x.DocumentNumber).MaximumLength(100);
        }
    }
}