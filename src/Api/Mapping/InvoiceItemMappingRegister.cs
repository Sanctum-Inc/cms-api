using Mapster;
using Domain.InvoiceItems;
using Contracts.InvoiceItem.Responses;
using Contracts.InvoiceItem.Requests;

namespace Api.Mappings;

// 🔹 Create explicit mapping registers for your models
public class InvoiceItemMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Entity -> Response
        config.NewConfig<InvoiceItem, InvoiceItemResponse>()
            .IgnoreNullValues(false)  // ✅ Preserve null values
            .TwoWays();  // Also creates Response -> Entity mapping

        // Request -> Entity
        config.NewConfig<AddInvoiceItemRequest, InvoiceItem>()
            .IgnoreNullValues(false)
            .Map(dest => dest.Hours, src => src.Hours)
            .Map(dest => dest.CostPerHour, src => src.CostPerHour)
            .Map(dest => dest.DayFeeAmount, src => src.DayFeeAmount)
            .Map(dest => dest.IsDayFee, src => src.IsDayFee);

        config.NewConfig<UpdateInvoiceItemRequest, InvoiceItem>()
            .IgnoreNullValues(false)
            .Map(dest => dest.Hours, src => src.Hours)
            .Map(dest => dest.CostPerHour, src => src.CostPerHour)
            .Map(dest => dest.DayFeeAmount, src => src.DayFeeAmount)
            .Map(dest => dest.IsDayFee, src => src.IsDayFee);
    }
}

// Create similar registers for other entities
public class InvoiceMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Invoices.Invoice, Contracts.Invoice.Responses.InvoiceResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class CourtCaseMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.CourtCases.CourtCase, Contracts.CourtCases.Responses.CourtCasesResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class LawyerMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Lawyers.Lawyer, Contracts.Lawyer.Responses.GetLawyerResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class DocumentMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Documents.Document, Contracts.Documents.Responses.DocumentResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}

public class UserMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Users.User, Contracts.User.Responses.UserResponse>()
            .IgnoreNullValues(false)
            .TwoWays();
    }
}
