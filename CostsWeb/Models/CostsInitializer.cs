﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CostsWeb.Models
{
    public class CostsInitializer : DropCreateDatabaseIfModelChanges<CostsContext>
    {

    }
}