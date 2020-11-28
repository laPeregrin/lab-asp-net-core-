using ObjectContainer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_BusinessLogic_.Extensions
{
    public static class MethodIContactExtension
    {
        public static bool IsValid(this Contact contactBLL, Contact model)
        {
            if (model != null)
                if (model.id != null)
                    if (model.FirstName != null)
                        if (model.Email.Contains("@"))
                            return true;
            return false;
        }
    }
}
