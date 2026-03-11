using dvd.bca.Samples;
using Xunit;

namespace dvd.bca.EntityFrameworkCore.Applications;

[Collection(bcaTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<bcaEntityFrameworkCoreTestModule>
{

}
