namespace TestGraphQL.GraphQlExtensions;

public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
{
    protected override void OnConfigure(
        IDescriptorContext context,
        IObjectFieldDescriptor descriptor,
        MemberInfo member)
    {
        descriptor.UseDbContext<ApplicationDbContext>();
    }
}