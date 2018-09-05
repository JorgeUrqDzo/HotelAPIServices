using System.Collections.Generic;
using Hotel.Entities;

namespace Hotel.Services.Interfaces
{
    public interface IMenuItemService
    {
        IEnumerable<Menu> GetMenuByInstance();

        bool HasPermission(string endPoint);
    }
}