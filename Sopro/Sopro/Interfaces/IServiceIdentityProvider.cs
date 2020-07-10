using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces
{
    public interface IServiceIdentityProvider
    {
        UserType getUserPriority(string email);
    }
}
