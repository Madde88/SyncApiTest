using Riok.Mapperly.Abstractions;

namespace SyncApiTest.Helpers;

[Mapper]
public partial class MapperlyMapper
{
    public partial Dog Map(Dog dog);
    public partial Owner Map(Owner owner);
}