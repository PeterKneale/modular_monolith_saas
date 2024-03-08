namespace Micro.Translations.Domain.TermAggregate;

public record TermId(Guid Value)
{
    public static TermId Create()=> new(Guid.NewGuid());
    public static implicit operator string(TermId d) => d.Value.ToString();
};