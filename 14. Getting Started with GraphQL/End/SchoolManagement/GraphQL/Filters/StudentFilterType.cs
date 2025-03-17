using HotChocolate.Data.Filters;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Filetrs;

public class StudentFilterType : FilterInputType<Student>
{
    protected override void Configure(IFilterInputTypeDescriptor<Student> descriptor)
    {
        // descriptor.BindFieldsExplicitly();
        // descriptor.Field(t => t.Id);
        // descriptor.Field(t => t.GroupId);
        // descriptor.Field(t => t.FirstName);
        // descriptor.Field(t => t.LastName);
        // descriptor.Field(t => t.DateOfBirth);

        // descriptor.BindFieldsImplicitly();
        // descriptor.Ignore(t => t.Group);
        // descriptor.Ignore(t => t.Courses);

        descriptor.Field(t => t.FirstName).Type<StudentStringOperationFilterInputType>();
        descriptor.Field(t => t.LastName).Type<StudentStringOperationFilterInputType>();
        descriptor.Field(t => t.Phone).Type<StudentStringOperationFilterInputType>();
    }
}