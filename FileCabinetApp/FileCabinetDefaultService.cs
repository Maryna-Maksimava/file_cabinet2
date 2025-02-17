using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    class FileCabinetDefaultService : FileCabinetService
    {
        public FileCabinetDefaultService() : base(new DefaultValidator()) 
        {
        }

    }
}
