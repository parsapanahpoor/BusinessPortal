﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Interfaces
{
    public interface IViewRenderService
    {
        string RenderToStringAsync(string viewName, object model);

    }
}
