using AutoMapper;
using OrganizationService.DTOs.LegalDocument;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles
{
    public class LegalDocumentProfile : Profile
    {
        public LegalDocumentProfile()
        {
            CreateMap<CreateLegalDocumentRequest, LegalDocument>();
            CreateMap<UpdateLegalDocumentRequest, LegalDocument>();
            CreateMap<LegalDocument, LegalDocumentResponse>();
        }
    }
}