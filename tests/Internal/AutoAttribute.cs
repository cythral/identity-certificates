using System.Threading;

using Amazon.S3;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

internal class AutoAttribute : AutoDataAttribute
{
    public AutoAttribute()
        : base(Create)
    {
    }

    public static IFixture Create()
    {
        var fixture = new Fixture();
        fixture.Inject(new CancellationToken(false));
        fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        fixture.Customizations.Add(new OptionsRelay());
        fixture.Customizations.Insert(-1, new TargetRelay());
        fixture.Customizations.Add(new TypeOmitter<RequestCharged>());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
