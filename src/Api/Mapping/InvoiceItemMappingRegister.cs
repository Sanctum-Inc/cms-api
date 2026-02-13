using Contracts.CourtCases.Responses;
using Contracts.Documents.Responses;
using Contracts.Invoice.Responses;
using Contracts.InvoiceItem.Requests;
using Contracts.InvoiceItem.Responses;
using Contracts.Lawyer.Responses;
using Contracts.User.Responses;
using Domain.CourtCases;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;
using Mapster;

namespace Api.Mappings;

// 🔹 Create explicit mapping registers for your models
public class InvoiceItemMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity -> Response
        config.NewConfig<InvoiceItem, InvoiceItemResponse>()
            .IgnoreNullValues(false) // ✅ Preserve null values
            .TwoWays(); // Also creates Response -> Entity mapping

        // Request -> Entity
        config.NewConfig<AddInvoiceItemRequest, InvoiceItem>()
            .IgnoreNullValues(false)
            .Map(dest => dest.Hours, src => src.Hours)
            .Map(dest => dest.CostPerHour, src => src.CostPerHour);

        config.NewConfig<UpdateInvoiceItemRequest, InvoiceItem>()
            .IgnoreNullValues(false)
            .Map(dest => dest.Hours, src => src.Hours)
            .Map(dest => dest.CostPerHour, src => src.CostPerHour);
    }
}

// Create similar registers for other entities
public class InvoiceMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Invoice, InvoiceResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class CourtCaseMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CourtCase, CourtCaseResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class LawyerMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Lawyer, LawyerResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class DocumentMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Document, DocumentResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class UserMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}
