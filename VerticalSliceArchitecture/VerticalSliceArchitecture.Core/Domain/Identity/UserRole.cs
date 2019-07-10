using System.ComponentModel;

namespace VerticalSliceArchitecture.Core.Domain.Identity
{
    public enum UserRole
    {
        [Description("none")]
        none = 0,
        [Description("standard user")]
        StandardUser = 1,
        [Description("website administrator")]
        Admin = 2
    }
}