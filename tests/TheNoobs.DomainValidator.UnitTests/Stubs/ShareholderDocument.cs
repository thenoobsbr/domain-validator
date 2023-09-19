﻿using TheNoobs.DomainValidator.Abstractions.Rules;
using TheNoobs.DomainValidator.Rules;

namespace TheNoobs.DomainValidator.UnitTests.Stubs;

public class ShareholderDocument
{
    public string Number { get; set; } = null!;

    public DocumentType Type { get; set; }

    public DateTime Validity { get; set; }

    public DomainValidator DoValidation(DomainValidator domainValidator)
    {
        domainValidator
            .For(this)
            .AddRule("SHRDOC001", "Shareholder document number cannot be null or whitespace.", new IsNotEmptyRuleSpecification<ShareholderDocument>(x => x.Number))
            .AddRule("SHRDOC002", "Shareholder document should be valid date.", new ShareholderDocumentValiditySpecification());

        return domainValidator;
    }
    
    private class ShareholderDocumentValiditySpecification : IRuleSpecification<ShareholderDocument>
    {
        public bool IsSatisfiedBy(ShareholderDocument entity)
        {
            if (entity.Type == DocumentType.Id)
            {
                return entity.Validity >= DateTime.UtcNow;
            }

            if (entity.Type == DocumentType.Passport)
            {
                return entity.Validity >= DateTime.UtcNow.AddMonths(6);
            }

            return true;

        }
    }
}
