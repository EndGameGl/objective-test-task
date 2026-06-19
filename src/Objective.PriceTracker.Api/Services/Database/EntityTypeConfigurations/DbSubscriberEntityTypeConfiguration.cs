using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objective.PriceTracker.Api.Models.Database;

namespace Objective.PriceTracker.Api.Services.Database.EntityTypeConfigurations;

public class DbSubscriberEntityTypeConfiguration : IEntityTypeConfiguration<DbSubscriber>
{
    public void Configure(EntityTypeBuilder<DbSubscriber> builder)
    {
        builder.ToTable("subscribers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired(true);

        builder.HasIndex(x => x.SubscriberMail).IsUnique();
        builder.Property(x => x.SubscriberMail).IsRequired(true);
    }
}
